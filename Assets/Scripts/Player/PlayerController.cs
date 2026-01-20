using System;
using System.Collections;
using UnityEngine;
public enum Form
{
    Human = 0,
    Middle = 1,
    Werewolf = 2
};
[RequireComponent(typeof(PlayerMovement), typeof(PlayerTracking))]
public class PlayerController : MonoBehaviour
{
    private PlayerMovement _movement;
    private PlayerTracking _tracking;
    private bool _onlyOneTransition = true;

    [Header("Infection Stats")]
    [SerializeField] private int _infectionProgress;
    [SerializeField][Range(25, 75)] private int _startingInfectionProgress = 25;
    [SerializeField] private int _infectionIncrement = 5;
    [SerializeField] private float _infectionRate = 2.5f;
    [SerializeField] private int _infectionThreshold1 = 50;
    [SerializeField] private int _infectionThreshold2 = 75;
    public Action<int> OnInfectionProgressChanged;
    private float _infectionTimer;

    [Header("Transformation Settings")]
    public Form _currentForm;
    private Form lastForm;
    [SerializeField] private ParticleSystem transformParticle;
    [SerializeField] private FMODUnity.EventReference transformSFX;
    [SerializeField] private Animator anim;
    [SerializeField] private Transform throwPoint;

    [Header("Movement Settings")]
    [SerializeField] private PlayerMoveSettings _humanMoveSettings;
    [SerializeField] private PlayerMoveSettings _werewolfMoveSettings;
    [SerializeField] private AmbienceManager _ambMan;
    private void Awake()
    {
        _movement = GetComponent<PlayerMovement>();
        _tracking = GetComponent<PlayerTracking>();

        _infectionProgress = _startingInfectionProgress;
        _infectionTimer = _infectionRate;
    }

    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame(); //Ensures that this will invoke event once anything else has had time to subscribe
        OnInfectionProgressChanged?.Invoke(_infectionProgress);
    }

    private void Update()
    {
        if (_infectionProgress >= 100 || _infectionProgress <= 0)
        {
            if (_onlyOneTransition)
            {
                Debug.Log("cdc");
                GameObject.FindWithTag("GameManager").GetComponent<GameManager>().CheckLoseCondition();
                _onlyOneTransition = false;
                _ambMan.StopAmbience();
            }

        }
        ProcessInfection();
    }

    private void ProcessFormChange()
    {
        switch (_infectionProgress)
        {
            case int n when (n >= _infectionThreshold2):
                _currentForm = Form.Werewolf;
                _tracking.SetTrackingEnabled(true);
                _movement.UpdateMoveSettings(_werewolfMoveSettings);
                throwPoint.localPosition = new Vector3(throwPoint.localPosition.x, 1.45f, 0);
                break;
            case int n when (n >= _infectionThreshold1):
                _currentForm = Form.Middle;
                _tracking.SetTrackingEnabled(true);
                _movement.UpdateMoveSettings(_humanMoveSettings);
                throwPoint.localPosition = new Vector3(throwPoint.localPosition.x, 0.55f, 0);
                break;
            default:
                _currentForm = Form.Human;
                _tracking.SetTrackingEnabled(false);
                _movement.UpdateMoveSettings(_humanMoveSettings);
                throwPoint.localPosition = new Vector3(throwPoint.localPosition.x, 0.55f, 0);
                break;
        }
    }

    private void ProcessInfection()
    {
        _infectionTimer -= Time.deltaTime;

        if (_infectionTimer < 0)
        {
            ChangeInfection(_infectionIncrement);
        }

        if (lastForm != _currentForm && !anim.GetBool("throwing"))
        {
            transformParticle.Play();
            anim.SetInteger("form", (int)_currentForm);
            FMODUnity.RuntimeManager.PlayOneShot(transformSFX, transform.position);

            lastForm = _currentForm;
        }
    }

    public void ChangeInfection(int amount)
    {
        _infectionProgress += amount;
        _infectionProgress = Mathf.Clamp(_infectionProgress, 0, 100);
        OnInfectionProgressChanged?.Invoke(_infectionProgress);

        ResetInfectionTimer();

        ProcessFormChange();
    }

    private void ResetInfectionTimer()
    {
        _infectionTimer = _infectionRate;
    }
}
