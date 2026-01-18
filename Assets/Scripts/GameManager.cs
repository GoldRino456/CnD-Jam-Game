using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    private int HowManyIngredientsArePickedUp = 0;
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

    public void CheckWinCondition()
    {
        if (HowManyIngredientsArePickedUp == 3)
        {
            Debug.Log("You won");
        }
    }

    public void CheckLoseCondition()
    {
        Debug.Log("You lose");
    }
}
