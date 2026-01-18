using System;
using UnityEngine;

public class Altar : MonoBehaviour, ITrackable
{
    public int TrackableId { get; set; }

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
            if(GameManager.Instance.CheckWinCondition())
            {
                Debug.Log("All collected and brought to altar.");
            }
            else
            {
                Debug.Log("Missing some stuff.");
            }
        }
    }
}
