using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Altar : MonoBehaviour, ITrackable
{
    [SerializeField] float transitionTime = 2.5f;
    [SerializeField] bool isThisTutorial;
    public int TrackableId { get; set; }
    [SerializeField] GameObject LoseScreenTransition;
    public event Action<int> OnDestroyCalled;
    public bool IsAltar { get; } = true;

    public GameObject GetGameObjectRef()
    {
        return gameObject;
    }

    public Vector3 GetWorldLocation()
    {
        return transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && collision.isTrigger == false)
        {
            //Check GameManager for Win Con
            if(GameManager.Instance.CheckWinCondition() && !isThisTutorial)
            {
                Instantiate(LoseScreenTransition, new Vector2(0, 0), Quaternion.identity);
                StartCoroutine(WinTransitionDelay("WinScreen"));
            }
            else if(GameManager.Instance.CheckWinCondition() && isThisTutorial)
            {
                Instantiate(LoseScreenTransition, new Vector2(0, 0), Quaternion.identity);
                StartCoroutine(WinTransitionDelay("Level"));
            }
            else
            {
                Debug.Log("Missing some stuff.");
            }
        }
    }

    private IEnumerator WinTransitionDelay(string sceneName)
    {
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneName);
    }
}
