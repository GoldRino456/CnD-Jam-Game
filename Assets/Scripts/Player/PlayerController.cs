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
    [SerializeField] private int _infectionThreshold = 50;
    private bool _isHumanForm = true; //We can swap this for an enum if we want more than two forms
    private bool _isFormChangeThisFrame = false;
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
        CheckForPlayerFormChange();
        ProcessFormChange();
    }

    private void CheckForPlayerFormChange()
    {
        if(_isHumanForm && _infectionProgress > _infectionThreshold)
        {
            _isFormChangeThisFrame = true;
        }
        else if(!_isHumanForm && _infectionProgress <= _infectionThreshold)
        {
            _isFormChangeThisFrame = true;
        }
    }

    private void ProcessFormChange()
    {
        if(!_isFormChangeThisFrame) { return; }

        _isHumanForm = !_isHumanForm; 

        //Swap for switch statement if more than two states
        if(_isHumanForm)
        {
            //Do change to Human stuff here
            _movement.UpdateMoveSettings(_humanMoveSettings);
        }
        else
        {
            //Do change to werewolf stuff here
            _movement.UpdateMoveSettings(_werewolfMoveSettings);
        }

        _isFormChangeThisFrame = false;
    }

    private void ProcessInfection()
    {
        _infectionTimer -= Time.deltaTime;

        if(_infectionTimer < 0)
        {
            _infectionProgress += _infectionIncrement;
            ResetInfectionTimer();
        }
    }

    public void ChangeInfection(int amount)
    {
        _infectionProgress += amount;
        _infectionProgress = Mathf.Clamp(_infectionProgress, 0, 100);

        ResetInfectionTimer();
    }

    private void ResetInfectionTimer()
    {
        _infectionTimer = _infectionRate;
    }
}
