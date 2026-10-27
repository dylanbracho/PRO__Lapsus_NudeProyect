using UnityEngine;

public class CoagulitoSController : MonoBehaviour
{
    enum State { Falling, Chasing, Resting }
    State state = State.Falling;
    float stateTimer;
    public float enemySpeed = 3f;
    public Rigidbody2D enemyRb;
    public bool isFacingRight;
    public Transform target;                    // the player
    public float chaseDuration = 2f;            // seconds chasing
    public float restDuration = 1.5f;           // seconds stopped between chases
    public float landDelay = 0.5f;              // pause after landing
    public float hitRestDuration = 2f;          // pause after hitting the player
    public float stopDistance = 0.5f;           // stops this close to the player (X only)
    [SerializeField] private float enemyDamage;
    [SerializeField] private float enemyStrength;

    void Start()
    {
        enemyRb = GetComponent<Rigidbody2D>();
        isFacingRight = false;

        PlayerController pc = FindFirstObjectByType<PlayerController>();
        if (pc != null)
            target = pc.transform;
    }

    void FixedUpdate()
    {
        // Only X is controlled here. Y stays untouched so gravity works.
        Vector2 v = enemyRb.linearVelocity;   // Unity 2022 or older: enemyRb.velocity

        switch (state)
        {
            case State.Falling:
                v.x = 0f;
                break;

            case State.Chasing:
                {
                    if (target != null)
                    {
                        float dx = target.position.x - transform.position.x;

                        if (Mathf.Abs(dx) > stopDistance)
                        {
                            float dir = Mathf.Sign(dx);
                            FaceDirection(dir);
                            v.x = dir * enemySpeed;
                        }
                        else
                        {
                            v.x = 0f;
                        }
                    }
                    else
                    {
                        v.x = 0f;
                    }

                    stateTimer -= Time.fixedDeltaTime;
                    if (stateTimer <= 0f) StartResting(restDuration);
                    break;
                }

            case State.Resting:
                v.x = 0f;
                stateTimer -= Time.fixedDeltaTime;
                if (stateTimer <= 0f)
                {
                    state = State.Chasing;
                    stateTimer = chaseDuration;
                }
                break;
        }

        enemyRb.linearVelocity = v;           // Unity 2022 or older: enemyRb.velocity = v
    }

    void StartResting(float duration)
    {
        state = State.Resting;
        stateTimer = duration;
    }

    void FaceDirection(float dir)
    {
        if ((dir < 0 && isFacingRight) || (dir > 0 && !isFacingRight))
            Flip();
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Landed? (touching something below us)
        if (state == State.Falling)
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y > 0.5f)
                {
                    StartResting(landDelay);
                    break;
                }
            }
        }

        // Hit the player
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            player.TakeDamage(enemyDamage);
            player.hitTime = 0.5f;
            player.hitForce = enemyStrength;
            player.hitFromRight = collision.transform.position.x <= transform.position.x;

            // Back off for a moment instead of pushing into the player
            StartResting(hitRestDuration);
        }
    }
}