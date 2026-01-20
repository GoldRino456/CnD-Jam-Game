using UnityEngine;

public class HolyWaterImpact : MonoBehaviour
{
    void Awake()
    {
        Destroy(gameObject, 1f);
    }
}