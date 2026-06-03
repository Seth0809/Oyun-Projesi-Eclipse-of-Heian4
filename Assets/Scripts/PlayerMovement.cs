using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public Animator anim;
    public float speed = 5f;

    public Hitbox hitbox;
    public Rigidbody2D rb;

    bool isAttacking;
    bool isDead;

    bool isKnocked;

    public bool isParrying;
    bool parryLocked;

    public float parryWindow = 0.25f;

    public float knockbackForce = 8f;
    public float knockbackDuration = 0.2f;

    Vector3 originalScale;

    public EnemyAI currentEnemy;

    public PlayerHealth health;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        originalScale = transform.localScale;

        if (hitbox != null)
            hitbox.DisableHitbox();
    }

    void Update()
    {
        if (isDead) return;

        HandleFlip();
        HandleAttack();

        if (Input.GetMouseButtonDown(1) && !parryLocked && !isDead)
        {
            StartCoroutine(Parry());
        }
    }

    void FixedUpdate()
    {
        if (isDead) return;

        Move();
    }

    void Move()
    {
        if (isAttacking || isKnocked) return;

        float x = Input.GetAxisRaw("Horizontal");

        rb.linearVelocity = new Vector2(x * speed, rb.linearVelocity.y);

        anim.SetBool("isRunning", x != 0);
    }

    void HandleFlip()
    {
        float x = Input.GetAxisRaw("Horizontal");

        if (x > 0)
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        else if (x < 0)
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
    }

    void HandleAttack()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking && !isDead)
        {
            isAttacking = true;

            anim.SetTrigger("Attack");

            Invoke(nameof(EnableHitbox), 0.15f);
            Invoke(nameof(DisableHitbox), 0.35f);

            Invoke(nameof(ResetAttack), 0.6f);
        }
    }

    IEnumerator Parry()
    {
        parryLocked = true;
        isParrying = true;

        anim.ResetTrigger("Parry");
        anim.SetTrigger("Parry");

        yield return new WaitForSeconds(parryWindow);

        isParrying = false;

        yield return new WaitForSeconds(0.1f);

        parryLocked = false;
    }

    public void SuccessfulParry()
    {
        if (currentEnemy != null)
        {
            currentEnemy.Stun(2f);
        }
    }

    void EnableHitbox()
    {
        if (hitbox != null)
            hitbox.EnableHitbox();
    }

    void DisableHitbox()
    {
        if (hitbox != null)
            hitbox.DisableHitbox();
    }

    void ResetAttack()
    {
        isAttacking = false;
    }

    public void Knockback(Transform enemy)
    {
        StartCoroutine(KnockbackRoutine(enemy));
    }

    IEnumerator KnockbackRoutine(Transform enemy)
    {
        isKnocked = true;

        Vector2 dir = (transform.position - enemy.position).normalized;

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(knockbackDuration);

        isKnocked = false;
    }

    public void Die()
    {
        isDead = true;

        rb.linearVelocity = Vector2.zero;

        anim.SetTrigger("Death");

        if (hitbox != null)
            hitbox.DisableHitbox();

        this.enabled = false;
    }
}