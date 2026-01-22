using System;
using System.Collections;
using System.Data.Common;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private GameObject holyWaterPrefab;
    [SerializeField] private Transform throwSpawnPoint;
    [SerializeField] private int startingHolyWater = 5;
    [SerializeField] private float itemUseCooldown = 1f;

    [SerializeField] private PlayerPickup _pickUpLogic;

    [SerializeField] private float aimResolution;
    [SerializeField] private float aimMaxTime;
    [SerializeField] private float aimLineSpeed;
    [SerializeField] private float holyWaterDamping;
    [SerializeField] private LineRenderer aimLine;
    [SerializeField] private Animator anim;
    [SerializeField] private LayerMask collisionLayers;
    private bool _aiming;
    private int currentHolyWater;

    private Rigidbody2D playerRb;
    //private bool isThrowRequested;
    private bool isUseRequested;
    private bool isFacingLeft = true;
    private float itemUseTimer = 0f;
    private Coroutine aimArcCoroutine = null;

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

        InputManager.onThrowPress += ProcessAim;
        InputManager.onThrowRelease += ProcessThrow;

        anim.SetBool("facingLeft", true);
    }

    public void Update()
    {
        ProcessItemTimer();
        CheckForInventoryInput();
        //ProcessThrow();
        if(!GameManager.Instance.isPaused)
        {
            ProcessUse();
        }

        if (!_aiming && aimLine.enabled)
        {
            for (int i = 0; i < Mathf.RoundToInt(aimMaxTime / aimResolution); i++)
            {
                aimLine.SetPosition(i, Vector3.zero);
            }

            aimLine.enabled = false;
        }
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
        //isThrowRequested = InputManager.isThrowPressed;
        isUseRequested = InputManager.isUsePressed;
        var moveX = InputManager.moveDirection.x;

        if (moveX != 0)
        {
            bool isMovingLeft = moveX < 0; //True if less than zero, otherwise moving right

            if (isMovingLeft != isFacingLeft)
            {
                isFacingLeft = isMovingLeft;
                anim.SetBool("facingLeft", isFacingLeft);
            }

            throwSpawnPoint.localPosition = new Vector3((isFacingLeft ? -1 : 1) * 0.1f, throwSpawnPoint.localPosition.y, 0);
        }
    }

    /*private void ProcessThrow()
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
    }*/

    private void ProcessAim()
    {
        if (itemUseTimer > 0 || currentHolyWater <= 0) return;

        _aiming = true;
        aimLine.enabled = true;

        if(GameManager.Instance.isPaused) return;
        aimArcCoroutine = StartCoroutine(ShowAimArc());
    }
    private IEnumerator ShowAimArc()
    {
        Vector3 lastPlayerPos = transform.position;

        yield return DrawArc(false);

        while (_aiming)
        {
            yield return null;

            if (lastPlayerPos != transform.position)
            {
                yield return DrawArc(true);
            }
        }
    }
    private IEnumerator DrawArc(bool redraw)
    {   
        
            
        Vector2 velocity = new Vector2(playerRb.linearVelocity.x, 0) + (Vector2)(throwSpawnPoint.up * 5f + throwSpawnPoint.right * 5f * (isFacingLeft ? -1 : 1));
        Vector2 prevPos = throwSpawnPoint.position;

        for (float i = 0; i < aimMaxTime; i += aimResolution)
        {
            for (int j = Mathf.RoundToInt(i / aimResolution); j < Mathf.RoundToInt(aimMaxTime / aimResolution); j++)
            {
                aimLine.SetPosition(j, prevPos);
            }

            velocity += Physics2D.gravity * aimResolution;
            velocity /= 1 + holyWaterDamping * aimResolution;

            prevPos += velocity * aimResolution;

            if (Physics2D.OverlapCircle(prevPos, 0.01f, collisionLayers)) yield break;

            if (!redraw) yield return new WaitForSeconds(aimLineSpeed);
        }
    
        }
    private void ProcessThrow()
    {
        if (itemUseTimer > 0 || currentHolyWater <= 0 || !_aiming || GameManager.Instance.isPaused) return;

        _aiming = false;
        if(aimArcCoroutine != null)
        {
            print("LN 183 - PlayerInventory.cs StopCoroutine: coroutine is null? - " + aimArcCoroutine == null);
            StopCoroutine(aimArcCoroutine);
            aimArcCoroutine = null;
        }

        anim.SetBool("throwing", true);
    }
    public void Throw()
    {   
        if(!GameManager.Instance.isPaused)
        {
        var newHolyWater = Instantiate(holyWaterPrefab, throwSpawnPoint.position, Quaternion.identity);
        newHolyWater.GetComponent<HolyWater>().OnThrown(playerRb.linearVelocity, isFacingLeft, true);
        itemUseTimer = itemUseCooldown;
        currentHolyWater--;
        OnHolyWaterCountChanged?.Invoke(currentHolyWater);
        }
    }
    public void EndThrow()
    {
        anim.SetBool("throwing", false);
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
