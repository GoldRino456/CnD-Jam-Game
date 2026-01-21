using UnityEngine;

public class TrailTrigger : MonoBehaviour
{
    [SerializeField] TutorialManager tutorialManager;

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            tutorialManager.NearTheTrail();
            Destroy(this.gameObject);
        }
    }
}
