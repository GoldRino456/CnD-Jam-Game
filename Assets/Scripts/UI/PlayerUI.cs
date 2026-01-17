using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private GameObject _holyWaterUIContainer;
    [SerializeField] private GameObject _holyWaterIconPrefab;
    [SerializeField] private RectTransform _infectionBarProgress;

    private List<GameObject> _holyWaterUIIcons;
    private float _infectionBarMaxWidth;

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

        var playerInventory = playerObj.GetComponent<PlayerInventory>();
        var playerController = playerObj.GetComponent<PlayerController>();

        //Player Event Subscriptions
        playerInventory.OnHolyWaterCountChanged += PlayerInventory_OnHolyWaterCountChanged;
        playerController.OnInfectionProgressChanged += PlayerController_OnInfectionProgressChanged;
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
}
