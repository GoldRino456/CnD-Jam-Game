using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private float parallaxFactor;
    [SerializeField] private SpriteRenderer[] _bgSections;
    [SerializeField] private Camera _mainCam;

    private void LateUpdate()
    {
        transform.position = new Vector3(-_mainCam.transform.position.x * parallaxFactor, -_mainCam.transform.position.y * parallaxFactor, 0);
    }

}
