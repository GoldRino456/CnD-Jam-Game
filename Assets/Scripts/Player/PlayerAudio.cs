using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private FMODUnity.EventReference _footstepEvent;

    public void PlayFootstepSound()
    {
        if(_playerMovement._grounded) FMODUnity.RuntimeManager.PlayOneShot(_footstepEvent, transform.position);
    }
}