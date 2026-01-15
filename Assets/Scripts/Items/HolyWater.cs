using UnityEngine;

[RequireComponent (typeof(CircleCollider2D))]
public class HolyWater : MonoBehaviour, IItem
{
    public string Name { get; } = "Holy Water";

    [SerializeField] private CircleCollider2D _projectileCollider;
    [SerializeField] private Rigidbody2D _projectileRb;
    [SerializeField] private CircleCollider2D _aoeRadius; //Should be Trigger
    [SerializeField] private int infectionCureAmount = 35;
    [SerializeField] private FMODUnity.EventReference _splashSFX;
    public void OnPickup()
    {
        
    }

    public void OnThrown(Vector2 initialVelocity)
    {
        _projectileRb.linearVelocity = initialVelocity;
        _projectileCollider.enabled = true;
    }

    public void OnCollisionEnter2D(Collision2D collision) //Collision of Holy Water
    {
        FMODUnity.RuntimeManager.PlayOneShot(_splashSFX, transform.position);
        _projectileRb.linearVelocity = Vector2.zero;
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
}
