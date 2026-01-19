using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject LoseScreenTransition;
    public int HowManyIngredientsArePickedUp = 0;
    public int IngredientsNeededForWin = 3;
    public static GameManager Instance;
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitButton()
    {
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
            Debug.Log("You won");
            return true;
        }

        return false;
    }

    public void CheckLoseCondition()
    {
        Debug.Log("asd");
        Instantiate(LoseScreenTransition, new Vector2(0, 0), Quaternion.identity);
        StartCoroutine(TransitionDelay());     
    }

    private IEnumerator TransitionDelay()
    {
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene("LoseScreen");
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
