using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "PlayerPersistence")]

public class PlayerStats : ScriptableObject
{
    // ***  Player Health  ***

    private int playerHealth;
    public int PlayerHealth
    {
        get => playerHealth;
        set
        {
            if (value > PlayerMaxHealth)
                playerHealth = playerMaxHealth;
            else
                playerHealth = value;
        }
    }

    private int playerMaxHealth = 20;
    public int PlayerMaxHealth { get => playerMaxHealth; }

    // ***  Player Magic  ***

    private int playerMagic;
    public int PlayerMagic
    {
        get => playerMagic;
        set
        {
            if (value > PlayerMaxMagic)
                playerMagic = playerMaxMagic;
            else
                playerMagic = value;
        }
    }

    private int playerMaxMagic = 20;
    public int PlayerMaxMagic { get => playerMaxMagic; }

    // *** Player Stamina  ***

    private int playerStamina;
    public int PlayerStamina
    {
        get => playerStamina;
        set
        {
            if (value > PlayerMaxStamina)
                playerStamina = playerMaxStamina;
            else
                playerStamina = value;
        }
    }

    private int playerMaxStamina = 10;
    public int PlayerMaxStamina { get => playerMaxStamina; }

    private int playerStaminaRecharge = 2;
    public int PlayerStaminaRecharge { get => playerStaminaRecharge; }

    // ***  Attack  ***

    private int baseAttack = -2;
    public int BaseAttack { get => baseAttack; }

    private float baseAttackAccuracy = .9f;
    public float BaseAttackAccuracy { get => baseAttackAccuracy; }

    private int baseAttackStamCost = 1;
    public int BaseAttackStamCost { get => baseAttackStamCost; }

    // ***  Charged Attack  ***

    private int baseChargeAttack = -5;
    public int BaseChargeAttack { get => baseChargeAttack; }

    private float baseChargeAccuracy = .7f;
    public float BaseChargeAccuracy { get => baseChargeAccuracy; }
    
    private int baseChargeStamCost = 7;
    public int BaseChargeStamCost { get => baseChargeStamCost; }

    // ***  Fireball Attack  ***

    private int baseFireballAttack = -4;
    public int BaseFireballAttack { get => baseFireballAttack; }

    private float baseFireballAccuracy = .8f;
    public float BaseFireballAccuracy { get => baseFireballAccuracy; }

    private int baseFireballCost = 4;
    public int BaseFireballCost { get => baseFireballCost; }

    private float attackAllPenalty = .75f;
    public float AttackAllPenalty { get => attackAllPenalty; }

    // ***  Healing  ***

    private int baseHeal = 2;
    public int BaseHeal { get => baseHeal; }

    private int baseHealCost = 2;
    public int BaseHealCost { get => baseHealCost; }
    

    private void OnEnable()
    {
        playerHealth = PlayerMaxHealth;
        playerMagic = PlayerMaxMagic;
        playerStamina = playerMaxStamina;
    }
}
