using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private float health;
    private float lerpTimer;
    public float maxHealth = 100;
    public float chipSpeed = 2f;

    public Image frontHP;
    public Image backHP;
    public TextMeshProUGUI healthText;
    void Start()
    {
        health = maxHealth;
    }

    
    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();
      



    }

    public void UpdateHealthUI()
    {
        Debug.Log("health: " + health);

        float fillF = frontHP.fillAmount;
        float fillB = backHP.fillAmount;
        float hFraction = health / maxHealth;

        if (fillB > hFraction)
        {
            frontHP.fillAmount = hFraction;
            backHP.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            backHP.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
          //  Debug.Log("dmg tmr "+ lerpTimer);

        }

        if (fillF < hFraction)
        {
            backHP.color = Color.yellow;
            backHP.fillAmount = hFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            frontHP.fillAmount = Mathf.Lerp(fillF, hFraction, percentComplete);
            //Debug.Log("hel tmr " + lerpTimer);
        }

    }


    public void TakeDamage(float damage)
    {
        health -= damage;
        lerpTimer = 0f;
       
    }

    public void RestoreHealth(float healAmount)
    {
        health += healAmount;
        lerpTimer = 0f;
    }

}
