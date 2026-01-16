using System;
using System.Collections;
using UnityEngine;

[RequireComponent (typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour
{
    private PlayerMovement _movement;

    [Header("Infection Stats")]
    [SerializeField] private int _infectionProgress;
    [SerializeField][Range(25, 75)] private int _startingInfectionProgress = 25;
    [SerializeField] private int _infectionIncrement = 5;
    [SerializeField] private float _infectionRate = 2.5f;
    [SerializeField] private int _infectionThreshold1 = 50;
    [SerializeField] private int _infectionThreshold2 = 75;
    private enum Form
    {
        Human = 0,
        Middle = 1,
        Werewolf = 2
    };
    [SerializeField] private Form currentForm;
    private Form _currentForm
    {
        get => currentForm;
        set
        {
            if(value != currentForm)
            {
                transformParticle.Play();
                anim.SetInteger("form", (int)value);
                
                currentForm = value;
            }
        }
    }
    [SerializeField] private ParticleSystem transformParticle;
    [SerializeField] private Animator anim;
    private float _infectionTimer;

    [Header("Movement Settings")]
    [SerializeField] private PlayerMoveSettings _humanMoveSettings;
    [SerializeField] private PlayerMoveSettings _werewolfMoveSettings;

    private void Awake()
    {
        _movement = GetComponent<PlayerMovement>();

        _infectionProgress = _startingInfectionProgress;
        _infectionTimer = _infectionRate;
    }

    private void Update()
    {
        ProcessInfection();
    }

    private void ProcessFormChange()
    {
        switch(_infectionProgress)
        {
            case int n when (n >= _infectionThreshold2):
                _currentForm = Form.Werewolf;
                _movement.UpdateMoveSettings(_werewolfMoveSettings);
                break;
            case int n when (n >= _infectionThreshold1):
                _currentForm = Form.Middle;
                _movement.UpdateMoveSettings(_humanMoveSettings);
                break;
            default:
                _currentForm = Form.Human;
                _movement.UpdateMoveSettings(_humanMoveSettings);
                break;
        }
    }

    private void ProcessInfection()
    {
        _infectionTimer -= Time.deltaTime;

        if(_infectionTimer < 0)
        {
            _infectionProgress += _infectionIncrement;
            ResetInfectionTimer();
        }

        ProcessFormChange();
    }

    public void ChangeInfection(int amount)
    {
        _infectionProgress += amount;
        _infectionProgress = Mathf.Clamp(_infectionProgress, 0, 100);

        ResetInfectionTimer();

        ProcessFormChange();
    }

    private void ResetInfectionTimer()
    {
        _infectionTimer = _infectionRate;
    }
}
