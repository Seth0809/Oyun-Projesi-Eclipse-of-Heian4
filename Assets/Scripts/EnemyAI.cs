using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float speed = 2f;

    public float attackRange = 1.8f;
    public float stopRange = 3.5f;
    public float attackCooldown = 1f;
    public int damage = 1;

    private Animator anim;
    private Vector3 originalScale;
    private float lastAttackTime;

    private bool isAttacking;
    private bool isStunned;

    void Start()
    {
        anim = GetComponent<Animator>();
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (isStunned)
        {
            anim.SetFloat("Speed", 0f);
            return;
        }

        float dist = Vector2.Distance(transform.position, player.position);

        if (isAttacking)
        {
            anim.SetFloat("Speed", 0f);
            return;
        }

        if (dist <= attackRange)
        {
            Attack();
            anim.SetFloat("Speed", 0f);
            return;
        }

        if (dist <= stopRange)
        {
            anim.SetFloat("Speed", 0f);
            return;
        }

        Vector2 direction = (player.position - transform.position).normalized;

        transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            speed * Time.deltaTime
        );

        if (direction.x > 0)
            transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
        else
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);

        anim.SetFloat("Speed", 1f);
    }

    void Attack()
    {
        if (isAttacking) return;
        if (Time.time < lastAttackTime + attackCooldown) return;

        lastAttackTime = Time.time;
        isAttacking = true;

        anim.SetTrigger("Attack");

        PlayerMovement pm = player.GetComponent<PlayerMovement>();
        PlayerHealth ph = player.GetComponent<PlayerHealth>();

        if (pm != null && pm.isParrying)
        {
            pm.SuccessfulParry();
            isAttacking = false;
            return;
        }

        if (ph != null)
            ph.TakeDamage(damage);

        Invoke(nameof(ResetAttack), attackCooldown);
    }

    void ResetAttack()
    {
        isAttacking = false;
    }

    public void Stun(float duration)
    {
        StartCoroutine(StunRoutine(duration));
    }

    IEnumerator StunRoutine(float duration)
    {
        isStunned = true;

        anim.SetTrigger("Stunned");

        yield return new WaitForSeconds(duration);

        isStunned = false;
    }
}