using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{  
    public Image filler;
    public TextMeshProUGUI healthText;
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

            healthText.text = (Mathf.RoundToInt(filler.fillAmount * playerStats.PlayerMaxHealth)) + " / " + playerStats.PlayerMaxHealth;

            if (filler.fillAmount < fillPercentage)
            {
                filler.fillAmount = fillPercentage;
                reduce = false;
            }
        }

        if (increase)
        {
            filler.fillAmount += .5f / changeTime * Time.deltaTime;

            healthText.text = (Mathf.RoundToInt(filler.fillAmount * playerStats.PlayerMaxHealth)) + " / " + playerStats.PlayerMaxHealth;

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

        healthText.text = playerStats.PlayerHealth + " / " + playerStats.PlayerMaxHealth;
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
