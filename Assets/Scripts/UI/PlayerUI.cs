using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private GameObject _holyWaterUIContainer;
    [SerializeField] private GameObject _holyWaterIconPrefab;
    private List<GameObject> _holyWaterUIIcons;

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
        var playerInventory = playerObj.GetComponent<PlayerInventory>();
        playerInventory.OnHolyWaterCountChanged += PlayerInventory_OnHolyWaterCountChanged;
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
}
