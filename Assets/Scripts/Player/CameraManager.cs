using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private float _smoothSpeed = 0.125f;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private Transform _target;

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, _target.position + _offset, _smoothSpeed);
    }
}
