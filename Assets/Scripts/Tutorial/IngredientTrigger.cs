using UnityEngine;

public class IngredientTrigger : MonoBehaviour
{
    [SerializeField] TutorialManager tutorialManager;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            tutorialManager.NearTheIngredient();
            Destroy(this.gameObject);
        }
    }
}
