using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public Transform player;

    public float attackRange = 2f;
    public float stopRange = 3.5f;
    public float attackCooldown = 1f;
    public int damage = 1;

    private Animator anim;
    private float lastAttackTime;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);

        if (dist <= attackRange)
        {
            TryAttack();
            return;
        }

        if (dist <= stopRange)
        {
            anim.SetFloat("Speed", 0);
            return;
        }

        anim.SetFloat("Speed", 1);
    }

    void TryAttack()
    {
        if (Time.time < lastAttackTime + attackCooldown) return;

        lastAttackTime = Time.time;

        anim.SetTrigger("Attack");

        PlayerHealth ph = player.GetComponent<PlayerHealth>();
        PlayerMovement pm = player.GetComponent<PlayerMovement>();

        if (ph != null)
            ph.TakeDamage(damage);

        if (pm != null)
            pm.Knockback(transform);
    }
}