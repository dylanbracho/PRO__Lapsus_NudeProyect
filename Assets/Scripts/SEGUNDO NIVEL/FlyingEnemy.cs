using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float attackDelay = 1f;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float enemyStrength = 5f;

    private bool isWaiting = false;
    private bool isAttacking = false;
    private float timer = 0f;

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange && !isAttacking && !isWaiting)
        {
            isWaiting = true;
            timer = 0f;
        }

        if (isWaiting)
        {
            timer += Time.deltaTime;
            if (timer >= attackDelay)
            {
                isWaiting = false;
                isAttacking = true;
            }
        }

        if (isAttacking)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

            if (distanceToPlayer > detectionRange * 1.5f)
            {
                isAttacking = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController pc = collision.gameObject.GetComponent<PlayerController>();
        if (pc != null)
        {
            pc.hitTime = 0.5f;
            pc.hitForce = enemyStrength;

            if (collision.transform.position.x < transform.position.x)
            {
                pc.hitFromRight = true;
            }
            else
            {
                pc.hitFromRight = false;
            }
        }
    }
}
public struct HitData
{
    public float damage;
    public float strengthX;
    public float strengthY;
    public float hitTime;
    public Vector2 direction;

    public HitData(float dmg, float sx, float sy, float time, Vector2 dir)
    {
        damage = dmg;
        strengthX = sx;
        strengthY = sy;
        hitTime = time;
        direction = dir;
    }
}