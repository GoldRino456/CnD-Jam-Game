using UnityEngine;
using UnityEngine.Serialization;
public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private FMODUnity.EventReference _footstepEvent;
    [FMODUnity.ParamRef]
    [FormerlySerializedAs("parameter")]
    public string _parameter;
    [SerializeField] private FMODUnity.EventReference _jumpEvent;
    void Start()
    {
        _playerMovement.Jumped += PlayJumpSound;
        _playerMovement.GroundedChanged += PlayLandingSound; ;
    }
    public void PlayFootstepSound()
    {
        if (_playerMovement._grounded)
        {
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName(_parameter, 0);
            FMODUnity.RuntimeManager.PlayOneShot(_footstepEvent, transform.position);
        }
    }
    private void PlayJumpSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot(_jumpEvent, transform.position);
    }
    private void PlayLandingSound(bool grounded, float timeSinceChanged)
    {
        if (grounded)
        {
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName(_parameter, 1);
            FMODUnity.RuntimeManager.PlayOneShot(_footstepEvent, transform.position);
        }
    }
}