using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private float lerpSpeed;
    [SerializeField] private PlayerController pCon;
    [SerializeField] private FMODUnity.EventReference _musicEvent;
    [SerializeField] private FMODUnity.EventReference _titleEvent;
    [FMODUnity.ParamRef]
    [FormerlySerializedAs("parameter")]
    public string _parameter;
    private float _paramValue;
    private FMOD.Studio.EventInstance _musicInstance;
    private void Start()
    {

        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            _musicInstance = FMODUnity.RuntimeManager.CreateInstance(_titleEvent);
            _musicInstance.start();
        }
        else if (SceneManager.GetActiveScene().name == "Level")
        {
            _musicInstance = FMODUnity.RuntimeManager.CreateInstance(_musicEvent);
            _musicInstance.start();
        }
    }
    public void StopMusic()
    {
        _musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Level")
        {
            if(pCon != null)
            {
                _paramValue = Mathf.Lerp(pCon._infectionProgress / 100f, _paramValue, Time.deltaTime * lerpSpeed);
                FMODUnity.RuntimeManager.StudioSystem.setParameterByName(_parameter, _paramValue);
            }
            else
            {
                pCon = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            }

                
        }
    }
}