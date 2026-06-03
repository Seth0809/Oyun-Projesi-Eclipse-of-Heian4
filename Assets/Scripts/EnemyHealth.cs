using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;
    int currentHealth;

    public float knockbackForce = 2f;
    public float maxKnockbackSpeed = 3f;
    public float friction = 6f;

    Rigidbody2D rb;

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        ApplyVelocityClamp();
    }

    public void TakeDamage(int damage, Vector2 hitDir)
    {
        currentHealth -= damage;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(hitDir.normalized * knockbackForce, ForceMode2D.Impulse);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void ApplyVelocityClamp()
    {
        if (rb == null) return;

        // hız limiti (uçmayı bitiren şey)
        rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, maxKnockbackSpeed);

        // doğal yavaşlama (fake friction)
        rb.linearVelocity = Vector2.Lerp(
            rb.linearVelocity,
            Vector2.zero,
            friction * Time.deltaTime
        );
    }

    void Die()
    {
        Destroy(gameObject);
    }
}