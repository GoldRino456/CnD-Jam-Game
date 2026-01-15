using UnityEngine;

public class AmbienceManager : MonoBehaviour
{
    [SerializeField] private FMODUnity.EventReference _ambienceEvent;
    private void Start()
    {
        FMODUnity.RuntimeManager.PlayOneShot(_ambienceEvent, transform.position);
    }
}