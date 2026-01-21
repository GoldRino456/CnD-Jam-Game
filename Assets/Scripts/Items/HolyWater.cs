using System.Data.Common;
using UnityEngine;

public class HolyWater : MonoBehaviour, IItem
{
    public ItemType Type { get; } = ItemType.HolyWater;

    [SerializeField] private CircleCollider2D _projectileCollider;
    [SerializeField] private Rigidbody2D _projectileRb;
    [SerializeField] private int infectionCureAmount = 35;
    [SerializeField] private int pickupAmount = 2;
    [SerializeField] private FMODUnity.EventReference _pickupSFX;
    [SerializeField] private FMODUnity.EventReference _splashSFX;
    [SerializeField] private float _visualRotationSpeed = 5f;
    [SerializeField] private GameObject impactPrefab;

    [Header("Throw Settings")]
    [SerializeField] private float throwForce = 5f;
    [SerializeField] private float stunTime = 4f;

    public int PickupAmount { get => pickupAmount; }
    private bool isThrown = false;
    public bool isThrownByPlayer = false;

    private void FixedUpdate()
    {
        if(isThrown)
        {
            float angleRad = Mathf.Atan2(_projectileRb.linearVelocity.y, _projectileRb.linearVelocity.x);
            float angleDeg = (180 / Mathf.PI) * angleRad - 90;

            transform.rotation = Quaternion.Euler(0, 0, angleDeg * _visualRotationSpeed);
        }
    }

    public bool OnPickup()
    {
        if(isThrown)
        {
            return false; //Tossed, ignore pickup so player can get damaged
        }
        else
        {
            FMODUnity.RuntimeManager.PlayOneShot(_pickupSFX, transform.position);
            return true;
        }
    }

    public void OnThrown(Vector2 initialVelocity, bool isThrownLeft, bool isThrownByPlayer = false)
    {
        _projectileRb.linearVelocity = new Vector2(initialVelocity.x, 0);
        _projectileCollider.enabled = true;
        this.isThrownByPlayer = isThrownByPlayer;
        ApplyThrowForce(isThrownLeft);
    }

    public void OnCollisionEnter2D(Collision2D collision) //Collision of Holy Water
    {
        if (collision.gameObject.CompareTag("Player") && isThrownByPlayer) { return; } //Player ignores their own thrown holy water

        isThrown = false;
        FMODUnity.RuntimeManager.PlayOneShot(_splashSFX, transform.position);
        _projectileRb.linearVelocity = Vector2.zero;
        print("Holy water collision detected.");

        if(collision.gameObject.CompareTag("Player"))
        {
            print("Hit player.");
            var controller = collision.gameObject.GetComponent<PlayerController>();
            controller.ChangeInfection(-infectionCureAmount);
        }

        if(collision.gameObject.CompareTag("Enemy"))
        {
            if(collision.gameObject.TryGetComponent<EnemyTest>(out var wolf))
            {
                Debug.Log("Applying wolf stun.");
                wolf.Stun(stunTime);
            }
            else if(collision.gameObject.TryGetComponent<Enemy>(out var plagueDoctor))
            {
                Debug.Log("Applying plague doctor stun.");
                plagueDoctor.Stun(stunTime);
            }
        }

        Instantiate(impactPrefab, transform.position - Vector3.up * 0.5f, Quaternion.Euler(-90, 0, 0));
        Destroy(gameObject);
    }

    public void OnTriggerEnter2D(Collider2D collision) //AOE Effect Range
    {
        print("Ping - OnTriggerEnter2D is called on HolyWater obj.");
    }

    public void OnUse(GameObject user)
    {
        //Trigger any vfx/sfx

        if(user.CompareTag("Player"))
        {
            var controller = user.GetComponent<PlayerController>();
            controller.ChangeInfection(-infectionCureAmount);
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
