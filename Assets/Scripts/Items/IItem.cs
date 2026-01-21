using UnityEngine;

public interface IItem
{
    public ItemType Type { get; }
    public bool OnPickup();
    public void OnThrown(Vector2 initialVelocity, bool isThrownLeft, bool isThrownByPlayer);
    public void OnUse(GameObject user);
}

public enum ItemType
{
    HolyWater = 0,
    PotionItem = 1
}
