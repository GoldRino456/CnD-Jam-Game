using UnityEngine;
using UnityEngine.Serialization;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private float lerpSpeed;
    [SerializeField] private PlayerController pCon;
    [SerializeField] private FMODUnity.EventReference _musicEvent;
    [FMODUnity.ParamRef]
    [FormerlySerializedAs("parameter")]
    public string _parameter;
    private float _paramValue;
    private FMOD.Studio.EventInstance _musicInstance;
    private void Start()
    {
        _musicInstance = FMODUnity.RuntimeManager.CreateInstance(_musicEvent);
        _musicInstance.start();
    }
    public void StopMusic()
    {
        _musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
    void Update()
    {
        _paramValue = Mathf.Lerp(pCon._infectionProgress / 100f, _paramValue, Time.deltaTime * lerpSpeed);
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName(_parameter, _paramValue);
    }
}