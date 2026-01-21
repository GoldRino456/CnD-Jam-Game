using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    private FMOD.Studio.Bus masterVolume;
    [SerializeField] private AnimationCurve volumeScaling;
    void Start()
    {
        masterVolume = FMODUnity.RuntimeManager.GetBus("bus:/");
    }
    public void SetVolumeBus()
    {
        masterVolume.setVolume(volumeScaling.Evaluate(volumeSlider.value));
        Debug.Log(volumeScaling.Evaluate(volumeSlider.value));
    }
}
