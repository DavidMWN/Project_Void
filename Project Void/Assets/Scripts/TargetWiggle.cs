using UnityEngine;
using TMPro;

public class TargetWiggle : MonoBehaviour
{
    Animator anim;
    public TextMeshProUGUI statusText;
    public SkeletonBattleCtrl target;
    public TargetWiggle otherIcon1;
    public TargetWiggle otherIcon2;
    public PlayerStats playerStats;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void OnSelect()
    {
        anim.SetTrigger("Selected");

        if (target != null)
            statusText.text = "Target " + target.GetName();
        else
        {
            statusText.text = "Target All Enemies\n(" 
                + ((1f - playerStats.AttackAllPenalty) * 100) 
                + "% Damage Penalty)";

            otherIcon1.anim.SetTrigger("Selected");
            otherIcon2.anim.SetTrigger("Selected");
        }
    }

    public void OnDeselcted()
    {
        anim.SetTrigger("Deselected");

        if (otherIcon1 != null)
            otherIcon1.anim.SetTrigger("Deselected");
        if (otherIcon2 != null)
            otherIcon2.anim.SetTrigger("Deselected");
    }
}
