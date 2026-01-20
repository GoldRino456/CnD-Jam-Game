using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [SerializeField] [Range(0f, 1f)] private float parallaxFactor = 0.5f;
    [SerializeField] private bool parallaxVertical = true;

    private Transform cam;
    private Vector3 lastCamPos;

    void Start()
    {
        cam = Camera.main.transform;
        lastCamPos = cam.position;
    }

    void LateUpdate()
    {
        Vector3 deltaCamPos = cam.position - lastCamPos;

        float parallaxX = deltaCamPos.x * parallaxFactor;
        float parallaxY = parallaxVertical ? deltaCamPos.y * parallaxFactor : deltaCamPos.y;

        transform.position += new Vector3(parallaxX, parallaxY, 0);

        lastCamPos = cam.position;
    }
}
