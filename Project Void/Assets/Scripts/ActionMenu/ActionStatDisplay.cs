using UnityEngine;
using TMPro;

public class ActionStatDisplay : MonoBehaviour
{
    public TextMeshProUGUI type;
    public TextMeshProUGUI power;
    public TextMeshProUGUI accuracy;
    public TextMeshProUGUI cost;
    public TextMeshProUGUI statusText;
    public PlayerStats playerStats;

    public void OnAttackSelect()
    {
        type.text = "Melee";
        power.text = Mathf.Abs(playerStats.BaseAttack).ToString();
        accuracy.text = ((playerStats.BaseAttackAccuracy * 100) + " %");
        cost.text = (playerStats.BaseAttackStamCost) + " SP";
        statusText.text = "A simple sword attack.";
    }

    public void OnChargeSelect()
    {
        type.text = "Melee";
        power.text = Mathf.Abs(playerStats.BaseChargeAttack).ToString();
        accuracy.text = ((playerStats.BaseChargeAccuracy * 100) + " %");
        cost.text = (playerStats.BaseChargeStamCost) + " SP"; ;
        statusText.text = "Gather your energy for a devestating blow on your next turn.";
    }

    public void OnFireballSelect()
    {
        type.text = "Magic";
        power.text = Mathf.Abs(playerStats.BaseFireballAttack).ToString();
        accuracy.text = ((playerStats.BaseFireballAccuracy * 100) + " %");
        cost.text = (playerStats.BaseFireballCost + " MP");
        statusText.text = "Harness arcane energy into an explosive blast of fire.";
    }

    public void OnHealSelect()
    {
        type.text = "Magic";
        power.text = (playerStats.BaseHeal).ToString();
        accuracy.text = "N/A";
        cost.text = (playerStats.BaseHealCost + " MP");
        statusText.text = "Direct your magic inwards to regain HP.";
    }
}
