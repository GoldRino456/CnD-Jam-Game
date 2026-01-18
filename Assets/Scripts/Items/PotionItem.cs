using System;
using UnityEngine;

public class PotionItem : MonoBehaviour, IItem, ITrackable
{
    public ItemType Type { get; } = ItemType.PotionItem;
    [SerializeField] private SpriteRenderer _potionSpriteRenderer;
    [SerializeField] private int _trackableId;

    public Sprite PotionSprite { get => _potionSpriteRenderer.sprite; }
    public int TrackableId { get => _trackableId; set => _trackableId = value; }

    public event Action<int> OnDestroyCalled;

    private void Awake()
    {
        _potionSpriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnDestroy()
    {
        OnDestroyCalled?.Invoke(TrackableId);
    }

    public Vector3 GetWorldLocation()
    {
        return transform.position;
    }

    public GameObject GetGameObjectRef()
    {
        return gameObject;
    }

    public bool OnPickup()
    {
        return true;
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
