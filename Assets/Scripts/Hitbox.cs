using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public int damage = 1;
    public Vector2 size = new Vector2(1.2f, 1f);

    bool hasHit;

    Transform player;

    void Start()
    {
        player = transform.root; // player referansı
    }

    public void EnableHitbox()
    {
        hasHit = false;
        gameObject.SetActive(true);
    }

    public void DisableHitbox()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (!gameObject.activeSelf) return;

        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, size, 0);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy") && !hasHit)
            {
                hasHit = true;

                EnemyHealth hp = hit.GetComponent<EnemyHealth>();
                if (hp != null)
                {
                    Vector2 dir = (hit.transform.position - player.position).normalized;

                    hp.TakeDamage(damage, dir);
                }

                Debug.Log("Hit Enemy");
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, size);
    }
}