using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject LoseScreenTransition;
    [SerializeField] GameObject PauseScreen;
    public bool isPaused = false;
    public int HowManyIngredientsArePickedUp = 0;
    public int IngredientsNeededForWin = 3;
    public static GameManager Instance;
    [SerializeField] private FMODUnity.EventReference uiClick;
    [SerializeField] private MusicManager musMan;
    [FMODUnity.ParamRef]
    [FormerlySerializedAs("parameter")]
    public string loseParam;
    [FMODUnity.ParamRef]
    [FormerlySerializedAs("parameter")]
    public string winParama;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

        if(PauseScreen != null)
        {
            PauseScreen.SetActive(false);
        }
    }


    public void PlayButton()
    {
        FMODUnity.RuntimeManager.PlayOneShot(uiClick, transform.position);
        musMan.StopMusic();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitButton()
    {
        FMODUnity.RuntimeManager.PlayOneShot(uiClick, transform.position);
        Debug.Log("Quit Game Request Received...");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;

#else
            Application.Quit();
#endif
    }

    public void PickedUpOneIngredient()
    {
        HowManyIngredientsArePickedUp++;
    }

    public bool CheckWinCondition()
    {
        if (HowManyIngredientsArePickedUp == IngredientsNeededForWin)
        {
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName(winParama, 1);
            return true;
        }

        return false;
    }

    public void CheckLoseCondition()
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName(loseParam, 1);
        Instantiate(LoseScreenTransition, new Vector2(0, 0), Quaternion.identity);
        StartCoroutine(LostTransitionDelay());
    }

    private IEnumerator LostTransitionDelay()
    {
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene("LoseScreen");
    }


    public void BackToMainMenu()
    {
        FMODUnity.RuntimeManager.PlayOneShot(uiClick, transform.position);
        SceneManager.LoadScene("MainMenu");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused)
        {
            TogglePause();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isPaused)
        {
            Resumegame();
        }

        if (!isPaused)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    void TogglePause()
    {
        PauseScreen.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

    }
    public void Resumegame()
    {
        PauseScreen.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        FMODUnity.RuntimeManager.PlayOneShot(uiClick, transform.position);
    }
}
