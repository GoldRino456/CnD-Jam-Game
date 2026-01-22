using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private FMODUnity.EventReference uiClick;
    private bool _isSettingsOpen;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        pauseScreen.SetActive(isPaused);
        settingsMenu.SetActive(false);
        Time.timeScale = isPaused ? 0 : 1;

        FMODUnity.RuntimeManager.PauseAllEvents(isPaused);
    }
    public void Settings()
    {
        _isSettingsOpen = !_isSettingsOpen;

        settingsMenu.SetActive(_isSettingsOpen);
        pauseScreen.SetActive(!_isSettingsOpen);
    }
    public void Menu()
    {
        //FMODUnity.RuntimeManager.PlayOneShot(uiClick, transform.position);
        SceneManager.LoadScene("MainMenu");
    }
}