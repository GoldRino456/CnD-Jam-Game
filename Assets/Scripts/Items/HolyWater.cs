using UnityEngine;

public class HolyWater : MonoBehaviour, IItem
{
    public string Name { get; } = "Holy Water";

    [SerializeField] private CircleCollider2D _projectileCollider;
    [SerializeField] private Rigidbody2D _projectileRb;
    [SerializeField] private CircleCollider2D _aoeRadius; //Should be Trigger
    [SerializeField] private int infectionCureAmount = 35;
    [SerializeField] private FMODUnity.EventReference _splashSFX;

    [Header("Throw Settings")]
    [SerializeField] private float throwForce = 5f;

    private bool isThrown = false;

    private void FixedUpdate()
    {
        if(isThrown)
        {
            float angleRad = Mathf.Atan2(_projectileRb.linearVelocity.y, _projectileRb.linearVelocity.x);
            float angleDeg = (180 / Mathf.PI) * angleRad - 90;

            transform.rotation = Quaternion.Euler(0, 0, angleDeg);
        }
    }

    public void OnPickup()
    {
        
    }

    public void OnThrown(Vector2 initialVelocity, bool isThrownLeft)
    {
        _projectileRb.linearVelocity = new Vector2(initialVelocity.x, 0);
        _projectileCollider.enabled = true;
        ApplyThrowForce(isThrownLeft);
    }

    public void OnCollisionEnter2D(Collision2D collision) //Collision of Holy Water
    {
        isThrown = false;
        FMODUnity.RuntimeManager.PlayOneShot(_splashSFX, transform.position);
        _projectileRb.linearVelocity = Vector2.zero;
        Destroy(gameObject);
    }

    public void OnTriggerEnter2D(Collider2D collision) //AOE Effect Range
    {
        
    }

    public void OnUse(GameObject user)
    {
        //Trigger any vfx/sfx

        if(user.CompareTag("player"))
        {
            var controller = user.GetComponent<PlayerController>();
            controller.ChangeInfection(infectionCureAmount);
        }
    }

    private void ApplyThrowForce(bool isThrownLeft)
    {
        int throwDirection = isThrownLeft ? -1 : 1;
        var impulseForce = transform.up * throwForce + transform.right * throwForce * throwDirection;
        _projectileRb.AddForce(impulseForce, ForceMode2D.Impulse);
        isThrown = true;
    }
}
