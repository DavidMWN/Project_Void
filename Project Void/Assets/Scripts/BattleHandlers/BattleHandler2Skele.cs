using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class BattleHandler2Skele : MonoBehaviour
{
    public PlayerBattleCtrl player;
    public SkeletonBattleCtrl enemy1;
    public SkeletonBattleCtrl enemy2;

    public GameObject battleEndText;
    private float endTimer = 0;
    private float timerLimit = 2.5f;

    public Button AttackButton;
    public Button ChargeAttackButton;
    public Button FireballButton;
    public Button HealButton;

    public Button Target1Button;
    public Button Target2Button;
    public Button TargetAllButton;

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
        Enemy1Turn,
        Enemy1Action,
        Enemy2Turn,
        Enemy2Action,
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
        enemy1.SetTarget(player);
        enemy2.SetTarget(player);

        state = State.PlayerTurn;

        AttackButton.onClick.AddListener(OnAttackClick);
        ChargeAttackButton.onClick.AddListener(OnChargeAttackClick);
        FireballButton.onClick.AddListener(OnFireballClick);
        HealButton.onClick.AddListener(OnHealClick);

        Target1Button.onClick.AddListener(OnTarget1Click);
        Target2Button.onClick.AddListener(OnTarget2Click);
        TargetAllButton.onClick.AddListener(OnTargetAllClick);

        TargetsActive(false);
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
                        state = State.Enemy1Turn;
                    }
                    break;
                }

            case State.Enemy1Turn:
                {
                    if (enemy1.GetState() == SkeletonBattleCtrl.State.Dead)
                    {
                        if (enemy2.GetState() == SkeletonBattleCtrl.State.Dead)
                        {
                            state = State.BattleEnd;
                            EndMessage("Victory!");
                            break;
                        }
                        else
                        {
                            state = State.Enemy2Turn;
                        }

                    }

                    if (enemy1.GetState() == SkeletonBattleCtrl.State.Idle)
                        enemy1.ChangeState(SkeletonBattleCtrl.State.WaitingForAction);

                    if (enemy1.GetState() == SkeletonBattleCtrl.State.WaitingForAction)
                    {
                        state = State.Enemy1Action;
                        enemy1.Act();
                    }
                    break;
                }

            case State.Enemy1Action:
                {
                    if (enemy1.GetState() == SkeletonBattleCtrl.State.FinishedActing)
                    {
                        enemy1.ChangeState(SkeletonBattleCtrl.State.Idle);                        
                        state = State.Enemy2Turn;
                    }
                    break;
                }

            case State.Enemy2Turn:
                {
                    if (enemy2.GetState() == SkeletonBattleCtrl.State.Dead)
                    {
                        if (enemy1.GetState() == SkeletonBattleCtrl.State.Dead)
                        {
                            state = State.BattleEnd;
                            EndMessage("Victory!");
                            break;
                        }
                        else
                        {
                            state = State.PlayerTurn;
                        }
                        break;
                    }

                    if (enemy2.GetState() == SkeletonBattleCtrl.State.Idle)
                        enemy2.ChangeState(SkeletonBattleCtrl.State.WaitingForAction);

                    if (enemy2.GetState() == SkeletonBattleCtrl.State.WaitingForAction)
                    {
                        state = State.Enemy2Action;
                        enemy2.Act();
                    }
                    break;
                }

            case State.Enemy2Action:
                {
                    if (enemy2.GetState() == SkeletonBattleCtrl.State.FinishedActing)
                    {
                        enemy2.ChangeState(SkeletonBattleCtrl.State.Idle);
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

        if (player.GetState() == PlayerBattleCtrl.State.Dead)
        {
            statusText.text = "You have been vanquished.";
        }
        else
        {
            sceneInfo.skele2Dead = true;
            statusText.text = "All enemies defeated!";
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
                TargetsActive(true);
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
                TargetsActive(true);
            }
            else
                statusText.text = "Not enough stamina.";
        }
    }

    private void OnFireballClick()
    {
        if (player.GetState() == PlayerBattleCtrl.State.WaitingForAction)
        {
            if (player.GetState() == PlayerBattleCtrl.State.WaitingForAction)
            {
                state = State.SelectingTarget;
                playerAct = PlayerAct.Fireball;
                ActionMenuActive(false);
                TargetsActive(true);
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

    private void OnTarget1Click()
    {
        player.SetTarget(0);
        state = State.PlayerAction;
        TargetsActive(false);

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

    private void OnTarget2Click()
    {
        player.SetTarget(1);
        state = State.PlayerAction;
        TargetsActive(false);

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

    private void OnTargetAllClick()
    {
        player.SetAttackAll(true);
        state = State.PlayerAction;
        TargetsActive(false);

        player.FireballAttack();
    }

    private void ActionMenuActive(bool newSet)
    {
        ActionMenu.SetActive(newSet);

        if (newSet)
        {
            AttackButton.Select();
        }
    }

    private void TargetsActive(bool newSet)
    {
        if (playerAct == PlayerAct.Fireball)
        {
            if ((enemy1.GetState() != SkeletonBattleCtrl.State.Dead) && (enemy2.GetState() != SkeletonBattleCtrl.State.Dead))
                TargetAllButton.gameObject.SetActive(newSet);            
        }

        if (enemy1.GetState() != SkeletonBattleCtrl.State.Dead)
            Target1Button.gameObject.SetActive(newSet);

        if (enemy2.GetState() != SkeletonBattleCtrl.State.Dead)
            Target2Button.gameObject.SetActive(newSet);

        if (newSet)
        {
            if (enemy1.GetState() != SkeletonBattleCtrl.State.Dead)
                Target1Button.Select();
            else
                Target2Button.Select();
        }
    }
}
