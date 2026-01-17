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

            if (Vector2.Distance(transform.position, _lastPlayerPosition) > pounceDistance)
            {
                rb.linearVelocity = new Vector2(Mathf.MoveTowards(rb.linearVelocity.x, _facingDirection * chaseSpeed, acceleration * Time.deltaTime), rb.linearVelocity.y);
                anim.SetBool("moving", true);
            }
            else
            {
                while (rb.linearVelocity.x > 0.1f || rb.linearVelocity.x < -0.1f)
                {
                    rb.linearVelocity = new Vector2(Mathf.MoveTowards(rb.linearVelocity.x, 0f, deceleration * Time.deltaTime), rb.linearVelocity.y);
                    yield return null;
                }

                anim.SetBool("moving", false);

                _pouncing = true;
                rb.AddForce(new Vector2(_facingDirection * pounceForce, pounceForce / 4), ForceMode2D.Impulse);

                float elapsed = 0f;

                while (elapsed < pounceCooldown)
                {
                    if (Physics2D.OverlapCircle(transform.position, damageRadius, playerLayer) != null && _pouncing)
                    {
                        var player = Physics2D.OverlapCircle(transform.position, damageRadius, playerLayer).GetComponent<PlayerController>();
                        player.ChangeInfection(infectionDamage);

                        _pouncing = false;
                    }

                    elapsed += Time.deltaTime;
                    yield return null;
                }

                _pouncing = false;
            }

            yield return null;
        }

        StartCoroutine(SuspicionState());
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(_currentState == State.Aggro) anim.SetBool("moving", true);
    }
}