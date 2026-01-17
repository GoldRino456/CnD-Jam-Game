using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);
    private float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;

    [SerializeField] private Transform target;
    [SerializeField] private bool enableAutoSearchForPlayerObject = true;

    private void Start()
    {
        if(enableAutoSearchForPlayerObject)
        {
            var player = GameObject.FindGameObjectWithTag("Player");

            if(player != null)
            {
                target = player.transform;
            }
        }
    }

    private void Update()
    {
        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
