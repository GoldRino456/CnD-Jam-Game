using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private GameObject _holyWaterUIContainer;
    [SerializeField] private GameObject _holyWaterIconPrefab;
    [SerializeField] private RectTransform _infectionBarProgress;
    [SerializeField] private Image[] potionItems;

    private List<GameObject> _holyWaterUIIcons;
    private float _infectionBarMaxWidth;
    private PlayerController playerController;
    private PlayerInventory playerInventory;

    private void Start()
    {
        var playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj == null)
        {
            Debug.LogWarning("Player object was not found by playerUI. PlayerUI will be disabled.");
            gameObject.SetActive(false);
            return;
        }

        _holyWaterUIIcons = new();
        _infectionBarMaxWidth = _infectionBarProgress.sizeDelta.x;

        playerInventory = playerObj.GetComponent<PlayerInventory>();
        playerController = playerObj.GetComponent<PlayerController>();

        //Player Event Subscriptions
        playerInventory.OnHolyWaterCountChanged += PlayerInventory_OnHolyWaterCountChanged;
        playerController.OnInfectionProgressChanged += PlayerController_OnInfectionProgressChanged;
        GameManager.Instance.OnPotionItemPickedUp += GameManager_OnPotionItemPickedUp;
    }

    private void PlayerInventory_OnHolyWaterCountChanged(int newCount)
    {
        //Clear And Clean Up List
        foreach(var item in _holyWaterUIIcons)
        {
            Destroy(item);
        }

        _holyWaterUIIcons.Clear();

        //Regenerate
        for(int i = 0; i < newCount;  i++)
        {
            var newIcon = Instantiate(_holyWaterIconPrefab, _holyWaterUIContainer.transform);
            _holyWaterUIIcons.Add(newIcon);
        }
    }

    private void PlayerController_OnInfectionProgressChanged(int newProgress)
    {
        var progressDecimal = newProgress / 100f;
        _infectionBarProgress.sizeDelta = new Vector2(
            progressDecimal * _infectionBarMaxWidth, 
            _infectionBarProgress.sizeDelta.y);
    }

    private void GameManager_OnPotionItemPickedUp(Sprite image, int index)
    {
        potionItems[index].sprite = image;
    }

    private void OnDestroy()
    {
        playerInventory.OnHolyWaterCountChanged -= PlayerInventory_OnHolyWaterCountChanged;
        playerController.OnInfectionProgressChanged -= PlayerController_OnInfectionProgressChanged;
        GameManager.Instance.OnPotionItemPickedUp -= GameManager_OnPotionItemPickedUp;
    }
}
