using System.Collections;
using UnityEngine;

public class Wolf : EnemyTest
{
    [Header("Wolf Settings")]
    [SerializeField] private float pounceDistance = 2.5f;
    [SerializeField] private float pounceForce = 10f;
    [SerializeField] private float pounceCooldown = 1f;
    [SerializeField] private float damageRadius = 0.5f;
    [SerializeField] private int infectionDamage = 10;
    [Header("Pounce Arc Settings")]
    [SerializeField] private float pounceArcTimeStep = 0.05f;
    [SerializeField] private float pounceArcMaxTime = 1.25f;
    [SerializeField] private float pounceArcRadius = 0.2f;
    [SerializeField] private float minPounceForwardDistance = 0.25f;
    private bool _pouncing;
    protected override void AggroState()
    {
        base.AggroState();
        StartCoroutine(AggroBehavior());
    }
    private IEnumerator AggroBehavior()
    {
        while (RaycastSweep())
        {
            _facingDirection = _lastPlayerPosition.x > transform.position.x ? 1f : -1f;
            spRen.flipX = _facingDirection == 1f ? true : false;

            float distanceToPlayer = Vector2.Distance(transform.position, _lastPlayerPosition);
            bool canLandSafely = CanPounceToSafeLanding();

            if (distanceToPlayer > pounceDistance)
            {
                if (IsEdgeOrObstacleAhead())
                {
                    if (canLandSafely) yield return PerformPounce();
                    else yield return Stop();
                }
                else
                {
                    rb.linearVelocity = new Vector2(Mathf.MoveTowards(rb.linearVelocity.x, _facingDirection * chaseSpeed, acceleration * Time.deltaTime), rb.linearVelocity.y);
                    anim.SetBool("moving", true);
                }
            }
            else
            {
                if (IsEdgeOrObstacleAhead() && !canLandSafely) yield return Stop();
                else yield return PerformPounce();
            }

            yield return null;
        }

        StartCoroutine(SuspicionState());
    }

    private IEnumerator PerformPounce()
    {
        yield return Stop();

        _pouncing = true;
        rb.AddForce(GetPounceImpulse(), ForceMode2D.Impulse);

        float elapsed = 0f;

        while (elapsed < pounceCooldown)
        {
            if (_pouncing)
            {
                ContactFilter2D filter = new ContactFilter2D();
                filter.layerMask = playerLayer;
                filter.useTriggers = false;

                Collider2D[] hits = new Collider2D[1];

                Physics2D.OverlapCircle(transform.position, damageRadius, filter, hits);

                if (hits[0] != null)
                {
                    PlayerController player = hits[0].GetComponent<PlayerController>();
                    if (player != null)
                    {
                        player.ChangeInfection(infectionDamage);
                    }

                    _pouncing = false;
                }
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        _facingDirection = _lastPlayerPosition.x > transform.position.x ? 1f : -1f;
        spRen.flipX = _facingDirection == 1f ? true : false;
        _pouncing = false;
    }

    private bool CanPounceToSafeLanding()
    {
        Vector2 prevPos = transform.position;

        for (float t = pounceArcTimeStep; t <= pounceArcMaxTime; t += pounceArcTimeStep)
        {
            Vector2 nextPos =
                (Vector2)transform.position + (rb.linearVelocity + GetPounceImpulse() / rb.mass) * (t + 0.5f) * (Physics2D.gravity.y * rb.gravityScale) * t * t * Vector2.up;

            Vector2 delta = nextPos - prevPos;

            if (delta.magnitude > 0f)
            {
                RaycastHit2D hit = Physics2D.CircleCast(prevPos, pounceArcRadius, delta / delta.magnitude, delta.magnitude, groundLayer | obstacleLayer);
                if (hit.collider != null)
                {
                    if (hit.distance <= 0.001f)
                    {
                        prevPos = nextPos;
                        continue;
                    }

                    if ((groundLayer.value & (1 << hit.collider.gameObject.layer)) != 0)
                    {
                        float flatness = Vector2.Dot(hit.normal, Vector2.up);
                        float forwardDistance = (hit.point.x - transform.position.x) * _facingDirection;
                        return flatness >= 0.9f && forwardDistance >= minPounceForwardDistance;
                    }

                    return false;
                }
            }

            prevPos = nextPos;
        }

        return false;
    }

    private Vector2 GetPounceImpulse()
    {
        return new Vector2(_facingDirection * pounceForce, pounceForce / 2f);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (_currentState == State.Aggro)
        {
            anim.SetBool("moving", true);
        }
    }
}
