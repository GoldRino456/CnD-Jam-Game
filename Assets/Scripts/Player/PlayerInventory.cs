using System;
using System.Collections;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private GameObject holyWaterPrefab;
    [SerializeField] private Transform throwSpawnPoint;
    [SerializeField] private int startingHolyWater = 5;
    [SerializeField] private float itemUseCooldown = 1f;

    [SerializeField] private PlayerPickup _pickUpLogic;
    [SerializeField] private float humanPickupRadius = 0.5f;

    private int currentHolyWater;

    private Rigidbody2D playerRb;
    private bool isThrowRequested;
    private bool isUseRequested;
    private bool isFacingLeft = true;
    private bool isDirectionChange = false;
    private float itemUseTimer = 0f;

    public Action<int> OnHolyWaterCountChanged;
    public Action OnThrow;
    public Action OnUse;
    private void Awake()
    {
        currentHolyWater = startingHolyWater;
        playerRb = GetComponent<Rigidbody2D>();
    }

    private IEnumerator Start()
    {
        _pickUpLogic.OnHolyWaterPickup += GainHolyWater;

        yield return new WaitForEndOfFrame(); //Ensures that this will invoke event once anything else has had time to subscribe
        OnHolyWaterCountChanged?.Invoke(currentHolyWater);
    }

    public void Update()
    {
        ProcessItemTimer();
        CheckForInventoryInput();
        ProcessThrow();
        ProcessUse();
    }

    private void ProcessItemTimer()
    {
        itemUseTimer -= Time.deltaTime;

        if (itemUseTimer < 0f)
        {
            itemUseTimer = 0f;
        }
    }

    private void CheckForInventoryInput()
    {
        isThrowRequested = InputManager.isThrowPressed;
        isUseRequested = InputManager.isUsePressed;
        var moveX = InputManager.moveDirection.x;
        
        if(moveX != 0)
        {
            bool isMovingLeft = moveX < 0; //True if less than zero, otherwise moving right

            if(isMovingLeft != isFacingLeft)
            {
                isFacingLeft = isMovingLeft;
                isDirectionChange = true;
            }
        }
    }

    private void ProcessThrow()
    {
        if(isDirectionChange)
        {
            throwSpawnPoint.localPosition = new Vector2(-throwSpawnPoint.localPosition.x, throwSpawnPoint.localPosition.y);
            isDirectionChange = false;
        }

        if (isThrowRequested && itemUseTimer <= 0 && currentHolyWater > 0)
        {
            var newHolyWater = Instantiate(holyWaterPrefab, throwSpawnPoint.position, Quaternion.identity);
            newHolyWater.GetComponent<HolyWater>().OnThrown(playerRb.linearVelocity, isFacingLeft);
            itemUseTimer = itemUseCooldown;
            currentHolyWater--;
            OnHolyWaterCountChanged?.Invoke(currentHolyWater);
            OnThrow?.Invoke();
        }
    }

    private void ProcessUse()
    {
        if (isUseRequested && itemUseTimer <= 0 && currentHolyWater > 0)
        {
            var newHolyWater = Instantiate(holyWaterPrefab, throwSpawnPoint.position, Quaternion.identity);
            newHolyWater.GetComponent<HolyWater>().OnUse(gameObject);
            Destroy(newHolyWater);
            itemUseTimer = itemUseCooldown;
            currentHolyWater--;
            OnHolyWaterCountChanged?.Invoke(currentHolyWater);
            OnUse?.Invoke();
        }
    }

    private void GainHolyWater(int amount)
    {
        currentHolyWater += amount;
        OnHolyWaterCountChanged?.Invoke(currentHolyWater);
    }
}
