using UnityEngine;
using TMPro;

public class SkeletonBattleCtrl : MonoBehaviour
{
    public float slideSpeed = 5.0f;

    private int health;
    private int maxHealth = 10;

    private string charName = "Skeleton";

    private float actionTimer = 0;
    private float timerLimit = 2f;

    Animator anim;

    PlayerBattleCtrl player;

    private int baseAttackAmt = -1;
    private int attackAmount;

    private int baseHealAmt = 1;

    public GameObject healthPopUp;
    public EnemyHealthBar healthBar;

    public TextMeshProUGUI statusText;

    public enum State
    {
        Idle,
        Acting,
        FinishedActing,
        WaitingForAction,
        Dead
    }

    private State state;

    private void Awake()
    {
        state = State.Idle;
    }

    private void Start()
    {
        health = maxHealth;
        anim = GetComponent<Animator>();
    }

    public void SetTarget(PlayerBattleCtrl opponent)
    {
        player = opponent;
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
                        state = State.FinishedActing;
                        actionTimer = 0;
                    }
                }
                break;
            case State.WaitingForAction:
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

    public void Act()
    {
        if ((UnityEngine.Random.Range(0f, 1f) < 0.2f) && (health < maxHealth))
        {
            state = State.Acting;
            ChangeHealth(baseHealAmt);
        }
        else
            Attack();
    }

    // *** Attack Management ***

    public void Attack()
    {
        state = State.Acting;
        anim.SetTrigger("Attack");
        if (UnityEngine.Random.Range(0f, 1f) > 0.4f)
            attackAmount = baseAttackAmt;
        else
            attackAmount = 0;
    }

    public void CauseDamage()
    {
        // Triggered via event in attack animation

        player.ChangeHealth(attackAmount);

        if (attackAmount < 0)
            statusText.text = charName + " attacks you for " + Mathf.Abs(attackAmount) + " damage.";
        else if (attackAmount == 0)
            statusText.text = charName + " misses their attack on you.";
    }


    // *** Health Management ***

    public void ChangeHealth(int changeAmt)
    {
        if (changeAmt > 0)
        {
            anim.SetTrigger("Magic");
            HPPopUp("+" + changeAmt);

            if ((health + changeAmt) <= maxHealth)
            {
                health += changeAmt;
                statusText.text = charName + " recovers " + changeAmt + " HP.";
            }
            else
            {
                health = maxHealth;
                statusText.text = charName + " recovers " + (maxHealth - health) + " HP.";
            }
        }
        else if (changeAmt < 0)
        {
            HPPopUp(changeAmt.ToString());

            if ((health + changeAmt) > 0)
            {
                anim.SetTrigger("Hit");
                health += changeAmt;
            }
            else
            {
                health = 0;
                anim.SetTrigger("Dead");
                state = State.Dead;
            }
        }
        else
        {
            HPPopUp("Miss");
        }

        healthBar.SetFillPercentage(health / (float)maxHealth);
    }

    private void HPPopUp(string hpChangeAmt)
    {
        GameObject healthObject = Instantiate(healthPopUp, transform.position, Quaternion.identity);
        healthObject.GetComponentInChildren<TextMesh>().text = hpChangeAmt;
    }
}