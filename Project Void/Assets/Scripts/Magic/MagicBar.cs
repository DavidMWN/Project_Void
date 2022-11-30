using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MagicBar : MonoBehaviour
{  
    public Image filler;
    public TextMeshProUGUI magicText;
    public PlayerStats playerStats;

    private float fillPercentage = 1f;
    private float changeTime = .5f;
    private bool reduce = false;
    private bool increase = false;

    private void Update()
    {
        if (reduce)
        {
            filler.fillAmount -= .5f / changeTime * Time.deltaTime;

            magicText.text = (Mathf.RoundToInt(filler.fillAmount * playerStats.PlayerMaxMagic)) + " / " + playerStats.PlayerMaxMagic;

            if (filler.fillAmount < fillPercentage)
            {
                filler.fillAmount = fillPercentage;
                reduce = false;
            }
        }

        if (increase)
        {
            filler.fillAmount += .5f / changeTime * Time.deltaTime;

            magicText.text = (Mathf.RoundToInt(filler.fillAmount * playerStats.PlayerMaxMagic)) + " / " + playerStats.PlayerMaxMagic;

            if (filler.fillAmount > fillPercentage)
            {
                filler.fillAmount = fillPercentage;
                increase = false;
            }
        }
    }

    public void SetUp(float value)
    {
        filler.fillAmount = value;

        magicText.text = playerStats.PlayerMagic + " / " + playerStats.PlayerMaxMagic;
    }

    public void SetFillPercentage(float value)
    {
        if (filler.fillAmount > value)
            reduce = true;
        if (filler.fillAmount < value)
            increase = true;
        
        fillPercentage = value;        
    }
}
