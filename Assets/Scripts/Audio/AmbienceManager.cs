using UnityEngine;

public class AmbienceManager : MonoBehaviour
{
    [SerializeField] private FMODUnity.EventReference _ambienceEvent;
    private FMOD.Studio.EventInstance _ambienceInstance;
    private void Start()
    {
        _ambienceInstance = FMODUnity.RuntimeManager.CreateInstance(_ambienceEvent);
        _ambienceInstance.start();
    }
    public void StopAmbience()
    {
        _ambienceInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}