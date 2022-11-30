using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class BattleHandler : MonoBehaviour
{
    public PlayerBattleCtrl player;
    public SkeletonBattleCtrl enemy;

    public GameObject battleEndText;
    private float endTimer = 0;
    private float timerLimit = 2.5f;

    public Button AttackButton;
    public Button ChargeAttackButton;
    public Button FireballButton;
    public Button HealButton;    

    public Button TargetButton;

    public GameObject ActionMenu;
    public TextMeshProUGUI statusText;

    public SceneInfo sceneInfo;
    public PlayerStats playerStats;

    private State state;

    private enum State
    {
        PlayerTurn,
        PlayerAction,
        SelectingTarget,
        EnemyTurn,
        EnemyAction,
        BattleEnd
    }

    private PlayerAct playerAct;

    private enum PlayerAct
    {
        Attack,
        ChargeAttack,
        Fireball
    }

    private void Start()
    {
        enemy.SetTarget(player);

        state = State.PlayerTurn;

        AttackButton.onClick.AddListener(OnAttackClick);
        ChargeAttackButton.onClick.AddListener(OnChargeAttackClick);
        FireballButton.onClick.AddListener(OnFireballClick);
        HealButton.onClick.AddListener(OnHealClick);

        TargetButton.onClick.AddListener(OnTargetClick);

        TargetActive(false);
    }

    private void Update()
    {
        switch (state)
        {
            case State.PlayerTurn:
                {                    
                    if (player.GetState() == PlayerBattleCtrl.State.Dead)
                    {
                        state = State.BattleEnd;
                        EndMessage("Defeated!");
                    }

                    if (player.GetState() == PlayerBattleCtrl.State.Idle)
                    {
                        if (player.GetCharged())
                        {
                            state = State.PlayerAction;
                            player.ChargeAttack();
                            break;
                        }

                        ActionMenuActive(true);
                        player.ChangeState(PlayerBattleCtrl.State.WaitingForAction);
                    }
                    break;
                }

            case State.SelectingTarget:
                break;

            case State.PlayerAction:
                { 
                    if (player.GetState() == PlayerBattleCtrl.State.FinishedActing)
                    {
                        player.ChangeState(PlayerBattleCtrl.State.Idle);
                        state = State.EnemyTurn;
                    }
                    break;
                }

            case State.EnemyTurn:
                {
                    if (enemy.GetState() == SkeletonBattleCtrl.State.Dead)
                    {
                        state = State.BattleEnd;
                        EndMessage("Victory!");
                    }

                    if (enemy.GetState() == SkeletonBattleCtrl.State.Idle)
                        enemy.ChangeState(SkeletonBattleCtrl.State.WaitingForAction);

                    if (enemy.GetState() == SkeletonBattleCtrl.State.WaitingForAction)
                    {
                        state = State.EnemyAction;                    
                        enemy.Act();
                    }
                    break;
                }

            case State.EnemyAction:
                {
                    if (enemy.GetState() == SkeletonBattleCtrl.State.FinishedActing)
                    {
                        enemy.ChangeState(SkeletonBattleCtrl.State.Idle);                        
                        state = State.PlayerTurn;
                    }
                    break;
                }

            case State.BattleEnd:
                {
                    endTimer += Time.deltaTime;

                    if (endTimer > timerLimit)
                    {
                        SceneManager.LoadScene("Area1");
                    }
                    break;
                }
        }
    }

    private void EndMessage(string message)
    {
        GameObject BattleEndObject = Instantiate(battleEndText, transform.position, Quaternion.identity);
        BattleEndObject.GetComponentInChildren<TextMesh>().text = message;

        if (enemy.GetState() == SkeletonBattleCtrl.State.Dead)
        {
            sceneInfo.skele1Dead = true;
            statusText.text = enemy.GetName() + " defeated!";
        }
        else
        {
            statusText.text = "You have been vanquished.";
        }
    }

    private void OnAttackClick()
    {
        if (player.GetState() == PlayerBattleCtrl.State.WaitingForAction)
        {
            if (playerStats.PlayerStamina >= playerStats.BaseAttackStamCost)
            {
                state = State.SelectingTarget;
                playerAct = PlayerAct.Attack;
                ActionMenuActive(false);
                TargetActive(true);
            }
            else
                statusText.text = "Not enough stamina.";
        }
    }

    private void OnChargeAttackClick()
    {
        if (player.GetState() == PlayerBattleCtrl.State.WaitingForAction)
        {
            if (playerStats.PlayerStamina >= playerStats.BaseChargeStamCost)
            {
                state = State.SelectingTarget;
                playerAct = PlayerAct.ChargeAttack;
                ActionMenuActive(false);
                TargetActive(true);
            }
            else
                statusText.text = "Not enough stamina.";
        }
    }

    private void OnFireballClick()
    {
        if (player.GetState() == PlayerBattleCtrl.State.WaitingForAction)
        {
            if (playerStats.PlayerMagic >= playerStats.BaseFireballCost)
            {
                state = State.SelectingTarget;
                playerAct = PlayerAct.Fireball;
                ActionMenuActive(false);
                TargetActive(true);
            }
            else
                statusText.text = "Not enough magic.";
        }
    }

    private void OnHealClick()
    {
        if (player.GetState() == PlayerBattleCtrl.State.WaitingForAction)
        {
            if (playerStats.PlayerMagic >= playerStats.BaseHealCost)
            {
                state = State.PlayerAction;
                player.Heal();
                ActionMenuActive(false);
            }
            else
                statusText.text = "Not enough magic.";
        }
    }

    private void OnTargetClick()
    {
        player.SetTarget(0);
        state = State.PlayerAction;
        TargetActive(false);

        switch (playerAct)
        {
            case PlayerAct.Attack:
                player.Attack();
                break;

            case PlayerAct.ChargeAttack:
                player.Charge();
                break;

            case PlayerAct.Fireball:
                player.FireballAttack();
                break;
        }
    }

    private void ActionMenuActive(bool newSet)
    {
        ActionMenu.SetActive(newSet);

        if (newSet)
        {
            AttackButton.Select();
        }
    }

    private void TargetActive(bool newSet)
    {
        TargetButton.gameObject.SetActive(newSet);

        TargetButton.Select();
    }
}
