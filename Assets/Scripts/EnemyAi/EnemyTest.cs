using System.Collections;
using UnityEngine;

public class EnemyTest : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] private float activeRange = 20f;
    [SerializeField] private float edgeDetectDistance = 1f;
    [SerializeField] private float edgeDetectHeight = 1f;
    [SerializeField] private float playerDetectDistance = 10f;
    [SerializeField] private float playerDetectResolution = 15f;
    [SerializeField] protected LayerMask playerLayer;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected LayerMask obstacleLayer;

    [Header("Movement Settings")]
    [SerializeField] protected float deceleration = 10f;
    [SerializeField] protected float acceleration = 10f;
    [SerializeField] private float patrolSpeed = 3f;
    [SerializeField] protected float chaseSpeed = 6f;

    [Header("Idle Settings")]
    [SerializeField] private float idleTimeMin = 0.5f;
    [SerializeField] private float idleTimeMax = 2f;

    [Header("Patrol Settings")]
    [SerializeField] private float patrolTime = 2f;

    [Header("Suspicion Settings")]
    [SerializeField] protected float suspicionThreshold = 2f;
    [SerializeField] protected float suspicionDecayFactor = 2f;

    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected SpriteRenderer spRen;
    [SerializeField] protected Animator anim;
    [SerializeField] private Transform suspicionMeter;
    protected bool _active;
    protected float _suspicion;
    protected float _facingDirection;
    protected Vector2 _lastPlayerPosition;
    protected enum State
    {
        Idle = 0,
        Patrol = 1,
        Suspicious = 2,
        Aggro = 3
    }
    [SerializeField] protected State _currentState;
    private Coroutine _currentCoroutine;
    private void Start()
    {
        _currentCoroutine = StartCoroutine(IdleState());
        _facingDirection = Random.value < 0.5f ? -1f : 1f;
        spRen.flipX = _facingDirection == 1f ? true : false;
    }
    private IEnumerator IdleState()
    {
        var cols = Physics2D.OverlapCircleAll(transform.position, activeRange, playerLayer);

        _active = cols.Length > 0;

        _currentState = State.Idle;

        while (rb.linearVelocity.x > 0.1f || rb.linearVelocity.x < -0.1f)
        {
            rb.linearVelocity = new Vector2(Mathf.MoveTowards(rb.linearVelocity.x, 0f, deceleration * Time.deltaTime), rb.linearVelocity.y);
            yield return null;
        }

        anim.SetBool("moving", false);

        yield return new WaitForSeconds(Random.Range(idleTimeMin, idleTimeMax));

        if (_active) _currentCoroutine = StartCoroutine(PatrolState());
        else _currentCoroutine = StartCoroutine(IdleState());
    }

    private IEnumerator PatrolState()
    {
        _currentState = State.Patrol;
        anim.SetBool("moving", true);

        float elapsed = 0f;

        while (elapsed < patrolTime)
        {
            rb.linearVelocity = new Vector2(Mathf.MoveTowards(rb.linearVelocity.x, _facingDirection * patrolSpeed, acceleration * Time.deltaTime), rb.linearVelocity.y);
            elapsed += Time.deltaTime;

            float edgeDistance = transform.position.x + (_facingDirection * edgeDetectDistance);

            RaycastHit2D edgeHit =
                Physics2D.Raycast(new Vector2(edgeDistance, transform.position.y + edgeDetectHeight), Vector2.down, edgeDetectHeight + 0.1f, groundLayer);

            RaycastHit2D obstacleHit =
                Physics2D.Raycast(new Vector2(edgeDistance, transform.position.y), Vector2.right * _facingDirection, edgeDetectDistance + 0.1f, obstacleLayer);

            if (edgeHit.collider == null || obstacleHit.collider != null)
            {
                float breakChance = Random.value;

                if (breakChance < 0.5f)
                {
                    _facingDirection *= -1f;
                    spRen.flipX = _facingDirection == 1f ? true : false;
                }
                else break;
            }

            elapsed += Time.deltaTime;

            yield return null;
        }

        _currentCoroutine = StartCoroutine(IdleState());
    }

    protected IEnumerator SuspicionState()
    {
        _currentState = State.Suspicious;
        anim.SetBool("moving", false);

        while (_suspicion > 0f)
        {
            if (RaycastSweep()) _suspicion += Time.deltaTime;
            else _suspicion -= Time.deltaTime * (suspicionThreshold / suspicionDecayFactor);

            suspicionMeter.localScale = new Vector3(Mathf.Clamp01(_suspicion / suspicionThreshold) * 0.95f, 0.5f, 1f);

            _facingDirection = _lastPlayerPosition.x > transform.position.x ? 1f : -1f;
            spRen.flipX = _facingDirection == 1f ? true : false;

            if (_suspicion >= suspicionThreshold)
            {
                if (_currentCoroutine != null) StopCoroutine(_currentCoroutine);

                AggroState();
                yield break;
            }

            if (_suspicion <= 0f) break;

            yield return null;
        }

        _suspicion = 0f;
        _currentCoroutine = StartCoroutine(IdleState());
    }

    protected virtual void AggroState()
    {
        _currentState = State.Aggro;
    }

    private void Update()
    {
        if (_active && (int)_currentState < 2)
        {
            if (RaycastSweep())
            {
                if (_currentCoroutine != null) StopCoroutine(_currentCoroutine);

                _suspicion += 0.25f;

                _currentCoroutine = StartCoroutine(SuspicionState());
            }
        }
        else if (!_active && _currentState != State.Idle)
        {
            if (_currentCoroutine != null) StopCoroutine(_currentCoroutine);

            _currentCoroutine = StartCoroutine(IdleState());
        }
    }

    protected bool RaycastSweep()
    {
        for (int i = 0; i < playerDetectResolution; i++)
        {
            float angle = Mathf.Lerp(-45f, 45f, (float)i / (playerDetectResolution - 1));

            RaycastHit2D hit =
                Physics2D.Raycast(transform.position + Vector3.up * 1f, Quaternion.Euler(0, 0, angle) * Vector3.right * _facingDirection, playerDetectDistance, playerLayer | obstacleLayer | groundLayer);

            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                _lastPlayerPosition = hit.collider.transform.position;
                return true;
            }
        }

        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, activeRange);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * _facingDirection * playerDetectDistance);

        for (int i = 0; i < playerDetectResolution; i++)
        {
            float angle = Mathf.Lerp(-45f, 45f, (float)i / (playerDetectResolution - 1));
            Vector3 dir = Quaternion.Euler(0, 0, angle) * Vector3.right * _facingDirection;

            Gizmos.DrawLine(transform.position, transform.position + Vector3.up * 1f + dir * playerDetectDistance);
        }

        Gizmos.color = Color.red;
        Vector3 edgeDetectPos = transform.position + Vector3.right * _facingDirection * edgeDetectDistance;
        Gizmos.DrawLine(new Vector3(edgeDetectPos.x, edgeDetectPos.y + edgeDetectHeight, 0f), new Vector3(edgeDetectPos.x, edgeDetectPos.y - 0.1f, 0f));
    }
}