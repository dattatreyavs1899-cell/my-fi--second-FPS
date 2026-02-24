using UnityEngine;
using DG.Tweening;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("Damage Feedback")]
    public Renderer enemyRenderer;
    public Color damageColor = Color.red;
    public float flashDuration = 0.15f;

    private Color originalColor;

    void Awake()
    {
        if (enemyRenderer != null)
        {
            originalColor = enemyRenderer.material.GetColor("_BaseColor");
        }
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;

        if (enemyRenderer != null)
        {
            enemyRenderer.material.DOKill();
            enemyRenderer.material.SetColor("_BaseColor", originalColor);
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        FlashRed();

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void FlashRed()
    {
        if (enemyRenderer != null)
        {
            enemyRenderer.material.DOKill();

            enemyRenderer.material.DOColor(damageColor, "_BaseColor", flashDuration / 2f)
                .SetLoops(2, LoopType.Yoyo);
        }
    }

    void Die()
    {
        if (WaveManager.Instance != null)
        {
            WaveManager.Instance.OnEnemyDied();
        }

        gameObject.SetActive(false);
    }
}