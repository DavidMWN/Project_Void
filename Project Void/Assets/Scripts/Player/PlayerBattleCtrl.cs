using System;
using UnityEngine;
using TMPro;

public class PlayerBattleCtrl : MonoBehaviour
{
    public enum State { Idle, Acting, FinishedActing, WaitingForAction, Dead }
    private State state;

    Animator anim;

    public HealthBar healthBar;
    public GameObject healthPopUp;
    public MagicBar magicBar;
    public StaminaBar staminaBar;

    public TextMeshProUGUI statusText;

    public PlayerStats playerStats;

    [SerializeField] private SkeletonBattleCtrl[] enemies;

    private string charName = "Player";

    private bool charged;

    private bool staminaRecharged = true;
    
    private float actionTimer = 0;
    private float timerLimit = 2f;
        
    private int enemiesIndex;
    private bool attackAll = false;

    private int attackAmount;
    

    private void Awake()
    {
        state = State.Idle;
    }

    private void Start()
    {
        healthBar.SetUp(playerStats.PlayerHealth / (float)playerStats.PlayerMaxHealth);        

        magicBar.SetUp(playerStats.PlayerMagic / (float)playerStats.PlayerMaxMagic);

        staminaBar.SetUp(playerStats.PlayerStamina / (float)playerStats.PlayerMaxStamina);

        charged = false;
        anim = GetComponent<Animator>();
    }


    private void Update()
    {
        switch (state)
        {
            case State.Idle:
                break;
            case State.Acting:
                {
                    actionTimer += Time.deltaTime;

                    if (actionTimer > timerLimit)
                    {
                        staminaRecharged = false;
                        state = State.FinishedActing;
                        actionTimer = 0;
                    }
                }
                break;
            case State.WaitingForAction:
                {
                    if (!staminaRecharged)
                    {
                        staminaRecharged = true;

                        if (!charged)
                        {
                            if ((playerStats.PlayerStamina + playerStats.PlayerStaminaRecharge) <= playerStats.PlayerMaxStamina)
                            {
                                playerStats.PlayerStamina += playerStats.PlayerStaminaRecharge;
                                staminaBar.SetFillPercentage(playerStats.PlayerStamina / (float)playerStats.PlayerMaxStamina);
                            }
                        }
                    }
                }
                break;
            case State.FinishedActing:
                break;
            case State.Dead:
                break;
        }
    }

    public string GetName()
    {
        return charName;
    }

    public State GetState()
    {
        return state;
    }

    public void ChangeState(State newState)
    {
        state = newState;
    }

    public bool GetCharged()
    {
        return charged;
    }

    public void SetAttackAll(bool newSet)
    {
        attackAll = newSet;
    }

    public void SetTarget(int index)
    {
        enemiesIndex = index;
    }

    // *** Attack Management ***


    public void Attack()
    {
        state = State.Acting;
        anim.SetTrigger("Attack");
        if (UnityEngine.Random.Range(0f, 1f) < playerStats.BaseAttackAccuracy)
            attackAmount = playerStats.BaseAttack;
        else
            attackAmount = 0;

        playerStats.PlayerStamina -= playerStats.BaseAttackStamCost;
        staminaBar.SetFillPercentage(playerStats.PlayerStamina / (float)playerStats.PlayerMaxStamina);
    }

    public void Charge()
    {
        state = State.Acting;
        anim.SetTrigger("Magic");
        charged = true;
        statusText.text = "You prepare for a powerful attack.";
    }

    public void ChargeAttack()
    {
        state = State.Acting;
        anim.SetTrigger("SkillAttack");
        if (UnityEngine.Random.Range(0f, 1f) < playerStats.BaseChargeAccuracy)
            attackAmount = playerStats.BaseChargeAttack;
        else
            attackAmount = 0;
        charged = false;

        playerStats.PlayerStamina -= playerStats.BaseChargeStamCost;
        staminaBar.SetFillPercentage(playerStats.PlayerStamina / (float)playerStats.PlayerMaxStamina);

    }

    public void FireballAttack()
    {
        state = State.Acting;
                
        anim.SetTrigger("MagicAttack");

        playerStats.PlayerMagic -= playerStats.BaseFireballCost;
        magicBar.SetFillPercentage(playerStats.PlayerMagic / (float)playerStats.PlayerMaxMagic);

        if (UnityEngine.Random.Range(0f, 1f) < playerStats.BaseFireballAccuracy)
            attackAmount = playerStats.BaseFireballAttack;
        else
            attackAmount = 0;        
    }

    public void TriggerDamage()
    {
        // Triggered via event in attack animations

        if (attackAll)
        {
            CauseDamageAll();
            attackAll = false;
        }
        else
            CauseDamage();
    }

    private void CauseDamage()
    {
        enemies[enemiesIndex].ChangeHealth(attackAmount);

        if (attackAmount < 0)
            statusText.text = "You attack " + enemies[enemiesIndex].GetName() + " for " + Mathf.Abs(attackAmount) + " damage.";
        else if (attackAmount == 0)
            statusText.text = "Your attack on " + enemies[enemiesIndex].GetName() + " misses.";
    }

    private void CauseDamageAll()
    {
        attackAmount = Convert.ToInt32(attackAmount * playerStats.AttackAllPenalty);

        foreach (SkeletonBattleCtrl e in enemies)
            e.ChangeHealth(attackAmount);

        statusText.text = "You attack all enemies for " + Mathf.Abs(attackAmount) + " damage.";
    }


    // *** Health Management ***

    public void Heal()
    {
        state = State.Acting;

        ChangeHealth(playerStats.BaseHeal);

        playerStats.PlayerMagic -= playerStats.BaseHealCost;
        magicBar.SetFillPercentage(playerStats.PlayerMagic / (float)playerStats.PlayerMaxMagic);
    }

    public void ChangeHealth(int changeAmt)
    {
        if (changeAmt > 0)
        {
            anim.SetTrigger("Magic");
            HPPopUp("+" + changeAmt);

            if ((playerStats.PlayerHealth + changeAmt) <= playerStats.PlayerMaxHealth)
            {
                playerStats.PlayerHealth += changeAmt;
                statusText.text = "You recover " + changeAmt + " HP.";
            }
            else
            {
                statusText.text = "You recover " + (playerStats.PlayerMaxHealth - playerStats.PlayerHealth) + " HP.";
                playerStats.PlayerHealth = playerStats.PlayerMaxHealth;
            }
        }
        else if (changeAmt < 0)
        {
            HPPopUp(changeAmt.ToString());

            if ((playerStats.PlayerHealth + changeAmt) > 0)
            {
                anim.SetTrigger("Hit");
                playerStats.PlayerHealth += changeAmt;
            }
            else
            {
                playerStats.PlayerHealth = 0;
                anim.SetTrigger("Dead");
                state = State.Dead;
            }
        }
        else
        {
            HPPopUp("Miss");
        }

        healthBar.SetFillPercentage(playerStats.PlayerHealth / (float)playerStats.PlayerMaxHealth);
    }

    private void HPPopUp(string hpChangeAmt)
    {
        GameObject healthObject = Instantiate(healthPopUp, transform.position, Quaternion.identity);
        healthObject.GetComponentInChildren<TextMesh>().text = hpChangeAmt;
    }
}