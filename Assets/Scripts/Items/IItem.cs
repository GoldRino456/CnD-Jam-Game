using UnityEngine;

public interface IItem
{
    public string Name { get; }
    public void OnPickup();
    public void OnThrown(Vector2 initialVelocity);
    public void OnUse(GameObject user);
}
