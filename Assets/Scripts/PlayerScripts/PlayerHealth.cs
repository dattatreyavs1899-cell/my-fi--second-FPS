using System.IO;
using TMPro;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    
    private float health;
    private float lerpTimer;

    [Header("Health Bar")]
    public float maxHealth = 100;
    public float chipSpeed = 2f;
    public Image frontHP;
    public Image backHP;
    public TextMeshProUGUI healthText;

    [Header("Overlay")]
    public Image DmgOverlay;
    public Image healOverlay;
    public float duration;
    public float fadeSpeed;

    private float durationTimer;
    void Start()
    {
        health = maxHealth;
        DmgOverlay.color = new Color(DmgOverlay.color.r, DmgOverlay.color.g, DmgOverlay.color.b, 0);
        healOverlay.color = new Color(healOverlay.color.r, healOverlay.color.g, healOverlay.color.b, 0);
    }

    
    void FixedUpdate()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();
        if (DmgOverlay.color.a > 0)
        {
            if (health < 30)
                return;
            durationTimer += Time.deltaTime;
            if (durationTimer > duration)
            {
                float tempAlpha = DmgOverlay.color.a;
                tempAlpha -=Time.deltaTime * fadeSpeed;
                DmgOverlay.color = new Color(DmgOverlay.color.r, DmgOverlay.color.g, DmgOverlay.color.b, tempAlpha);
            }
        }


        if (healOverlay.color.a > 0)
        {
            
            durationTimer += Time.deltaTime;
            if (durationTimer > duration)
            {
                float tempAlpha = healOverlay.color.a;
                tempAlpha -= Time.deltaTime * fadeSpeed;
                healOverlay.color = new Color(healOverlay.color.r, healOverlay.color.g, healOverlay.color.b, tempAlpha);
            }
        }
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
        durationTimer = 0;
        DmgOverlay.color = new Color(DmgOverlay.color.r, DmgOverlay.color.g, DmgOverlay.color.b, 0.4f);
        if (health <= 0)
        {
            QuitGame();
        }
    }

    public void RestoreHealth(float healAmount)
    {
        health += healAmount;
        lerpTimer = 0f;
        durationTimer = 0;
        if (health >= 100)
            return;
        healOverlay.color = new Color(healOverlay.color.r, healOverlay.color.g, healOverlay.color.b, 0.2f);
    }

    public void QuitGame()
    {
        // In a built game
        Application.Quit();

        // In the editor (so the button actually "works" during testing)
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }


}
