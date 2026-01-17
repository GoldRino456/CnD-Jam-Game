using UnityEngine;

public class PotionItem : MonoBehaviour, IItem, ITrackable
{
    public string Name { get; } = "Potion Item";

    public Vector3 GetWorldLocation()
    {
        return transform.position;
    }

    public void OnPickup()
    {
        throw new System.NotImplementedException();
    }

    public void OnThrown(Vector2 initialVelocity, bool isThrownLeft)
    {
        return;
    }

    public void OnUse(GameObject user)
    {
        throw new System.NotImplementedException();
    }
}
