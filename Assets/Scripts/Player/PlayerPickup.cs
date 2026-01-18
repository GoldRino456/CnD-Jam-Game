using System;
using System.Linq;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    private CircleCollider2D _playerPickupCollider;

    public Action<int> OnHolyWaterPickup;
    public Action<Sprite> OnPotionItemPickup;

    private void Awake()
    {
        _playerPickupCollider = GetComponent<CircleCollider2D>();
    }

    public void SetPickUpRadius(float radius)
    {
        _playerPickupCollider.radius = radius;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var itemList = collision.GetComponents<MonoBehaviour>().OfType<IItem>().ToList();

        if (itemList.Count <= 0) { return; }

        foreach (var item in itemList)
        {
            bool validPickup = item.OnPickup();

            if(!validPickup) {  continue; } //Skip items that cannot be picked up

            switch(item.Type)
            {
                case ItemType.HolyWater:
                    HolyWater hwItem = (HolyWater)item;
                    OnHolyWaterPickup?.Invoke(hwItem.PickupAmount);
                    Destroy(hwItem.gameObject);
                    break;

                case ItemType.PotionItem:
                    PotionItem pItem = (PotionItem)item;
                    OnPotionItemPickup?.Invoke(pItem.PotionSprite);
                    Destroy(pItem.gameObject);
                    break;

                default:
                    Debug.LogWarning("Item type not defined int PlayerPickup.cs: " +  item.Type.ToString());
                    break;
            }
        }
    }
}
