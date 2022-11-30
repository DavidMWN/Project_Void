using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{  
    public Image filler;
    public float fillPercentage = 1f;
    public float changeTime = .5f;
    private bool reduce = false;
    private bool increase = false;

    private void Update()
    {
        if (reduce)
        {
            filler.fillAmount -= .5f / changeTime * Time.deltaTime;

            if (filler.fillAmount < fillPercentage)
            {
                filler.fillAmount = fillPercentage;
                reduce = false;
            }
        }

        if (increase)
        {
            filler.fillAmount += .5f / changeTime * Time.deltaTime;

            if (filler.fillAmount > fillPercentage)
            {
                filler.fillAmount = fillPercentage;
                increase = false;
            }
        }
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
