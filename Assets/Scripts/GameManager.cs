using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject LoseScreenTransition;
    public int HowManyIngredientsArePickedUp = 0;
    public int IngredientsNeededForWin = 3;
    public static GameManager Instance;
    [SerializeField] private FMODUnity.EventReference uiClick;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); 
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }


    public void PlayButton()
    {
        FMODUnity.RuntimeManager.PlayOneShot(uiClick, transform.position);
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
            return true;
        }

        return false;
    }

    public void CheckLoseCondition()
    {
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
}
