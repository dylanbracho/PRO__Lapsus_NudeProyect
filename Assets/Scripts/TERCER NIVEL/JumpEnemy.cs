using System.Collections;
using UnityEngine;

// Wind-up -> leap up and follow player -> freeze -> slam onto shadow -> cooldown
// Assumes the enemy's pivot is at its feet.
public class JumpingEnemy : MonoBehaviour
{
    public Transform player;
    public Transform shadow;          // dark ellipse sprite (not a child of the enemy)
    public LayerMask groundLayer;
    public LayerMask playerLayer;

    public float triggerRange = 7f;
    public float windUp = 0.5f;
    public float trackTime = 1.5f;    // time spent rising and following the player
    public float lockTime = 0.4f;     // frozen: the player's chance to dodge
    public float recoverTime = 1f;
    public float cooldown = 2f;       // delay before it can attack again
    public float hoverHeight = 4f;
    public float moveSpeed = 8f;
    public float slamSpeed = 25f;
    public float landRadius = 1.5f;
    public int damage = 20;

    bool busy;
    float nextAttackTime;
    Vector3 shadowBaseScale;
    PlayerController playerController;

    void Start()
    {
        shadowBaseScale = shadow.localScale;
        shadow.gameObject.SetActive(false);
        playerController = player.GetComponent<PlayerController>();
    }

    void Update()
    {
        if (busy || Time.time < nextAttackTime) return;

        if (Vector2.Distance(transform.position, player.position) <= triggerRange)
            StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        busy = true;
        yield return new WaitForSeconds(windUp);   // crouch animation here

        // Leap up and follow the player; shadow shows the landing spot
        shadow.gameObject.SetActive(true);
        float hoverY = GroundY(player.position) + hoverHeight;
        for (float t = 0f; t < trackTime; t += Time.deltaTime)
        {
            Vector2 goal = new Vector2(player.position.x, hoverY);
            transform.position = Vector2.MoveTowards(transform.position, goal, moveSpeed * Time.deltaTime);
            shadow.position = new Vector2(transform.position.x, GroundY(transform.position));
            ScaleShadow();
            yield return null;
        }

        // Freeze so the player can dodge
        yield return new WaitForSeconds(lockTime);

        // Slam straight down
        Vector2 landing = shadow.position;
        while ((Vector2)transform.position != landing)
        {
            transform.position = Vector2.MoveTowards(transform.position, landing, slamSpeed * Time.deltaTime);
            ScaleShadow();
            yield return null;
        }
        shadow.gameObject.SetActive(false);

        Collider2D hit = Physics2D.OverlapCircle(transform.position, landRadius, playerLayer);
        if (hit != null && !playerController.isInvincible)
        {
            // hitFromRight = the hit came from the right, so the player gets pushed left
            playerController.hitFromRight = transform.position.x > player.position.x;
            playerController.hitTime = 0.3f;   // stun/knockback duration
            playerController.TakeDamage(damage);
        }

        yield return new WaitForSeconds(recoverTime);
        nextAttackTime = Time.time + cooldown;
        busy = false;
    }

    // Shadow is small while the enemy is high and grows as it gets closer to the ground
    void ScaleShadow()
    {
        float height01 = Mathf.Clamp01((transform.position.y - shadow.position.y) / hoverHeight);
        shadow.localScale = shadowBaseScale * Mathf.Lerp(1f, 0.5f, height01);
    }

    float GroundY(Vector2 pos)
    {
        RaycastHit2D hit = Physics2D.Raycast(pos + Vector2.up * 0.5f, Vector2.down, 100f, groundLayer);
        return hit ? hit.point.y : pos.y;
    }
}