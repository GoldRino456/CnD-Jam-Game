using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] GameObject FirstDialogueManager;
    [SerializeField] GameObject SecondDialogueManager;
    [SerializeField] GameObject Player;
    [SerializeField] GameObject FirstDialogBox;
    [SerializeField] GameObject SecondDialogBox;
    [SerializeField] GameObject BlackScreen;
    [SerializeField] GameObject TrailBox;
    [SerializeField] GameObject ThirdDialogBox;
    [SerializeField] GameObject IngredientBox;
    [SerializeField] GameObject ThirdDialogueManager;
    void Awake()
    {
        Player.GetComponent<CircleCollider2D>().enabled = false;
        Time.timeScale = 0f;
    }

    public void EndOfFirstDialogue()
    {
        BlackScreen.SetActive(false);
        FirstDialogBox.SetActive(false);
        FirstDialogueManager.SetActive(false);
        Time.timeScale = 1f;
    }

    public void NearTheTrail()
    {
        TrailBox.SetActive(true);
        SecondDialogueManager.SetActive(true);
        SecondDialogBox.SetActive(true);
        BlackScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    public void EndOfSecondDialogue()
    {
        TrailBox.SetActive(false);
        SecondDialogueManager.SetActive(false);
        SecondDialogBox.SetActive(false);
        BlackScreen.SetActive(false);
        Time.timeScale = 1f;
    }

    public void NearTheIngredient()
    {
        ThirdDialogueManager.SetActive(true);
        ThirdDialogBox.SetActive(true);
        IngredientBox.SetActive(true);
        BlackScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    public void EndOfThirdDialogue()
    {
        ThirdDialogueManager.SetActive(false);
        ThirdDialogBox.SetActive(false);
        IngredientBox.SetActive(false);
        BlackScreen.SetActive(false);
        Time.timeScale = 1f;
    }




}
