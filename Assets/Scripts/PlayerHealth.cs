using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;

    public Slider hpBar;
    public float smoothSpeed = 6f;

    private bool isDead;
    private PlayerMovement movement;

    void Start()
    {
        currentHealth = maxHealth;
        movement = GetComponent<PlayerMovement>();

        if (hpBar != null)
        {
            hpBar.maxValue = maxHealth;
            hpBar.value = maxHealth;
        }
    }

    void Update()
    {
        if (hpBar != null)
        {
            hpBar.value = Mathf.Lerp(hpBar.value, currentHealth, Time.deltaTime * smoothSpeed);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;

        if (movement != null)
            movement.Die();
    }

    public bool IsDead()
    {
        return isDead;
    }
}