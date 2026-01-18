using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
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
        Debug.Log("You lose");
    }
}
