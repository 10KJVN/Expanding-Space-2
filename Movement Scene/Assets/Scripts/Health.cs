using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    private float timer = 0.0f;
    public float delay = 1.0f;
    public Image healthBar;
    public float healthAmount = 100;

    [System.Obsolete]
    private void Update()
    {
        timer += Time.deltaTime;

        if(healthAmount <= 0)
        {
            Application.LoadLevel(Application.loadedLevel);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            TakeDamage(10);
        }

        if (healthAmount < 100)
        {
            if(timer > delay)
            {
                Healing(1);
                timer = 0;
            }
        }

        if (healthAmount < 90)
        {
           if(timer > delay)
            {
                Healing(2);
                timer = 0;
            } 
           
        }
    }


    public void TakeDamage(float Damage)
    {
        healthAmount -= Damage;
        healthBar.fillAmount = healthAmount / 100;
    }



    public void Healing(float healPoints)
    {
        healthAmount += healPoints;
        healthAmount = Mathf.Clamp(healthAmount, 0, 100);

        healthBar.fillAmount = healthAmount / 100;
    }
}
