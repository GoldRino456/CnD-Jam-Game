using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerTracking : MonoBehaviour
{
    [SerializeField] private Transform trackingIndicator;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private bool isTrackingEnabled = true;

    private List<ITrackable> trackedItems;
    private Vector3 currentlyTrackedPosition;
    private Vector3 currentPosition;

    private void Start()
    {
        UpdatePlayerPosition();

        trackedItems = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .OfType<ITrackable>()
            .ToList();

        UpdateTrackedItems();
    }

    public void SetTrackingEnabled(bool isEnabled)
    {
        isTrackingEnabled = isEnabled;
        trackingIndicator.gameObject.SetActive(isEnabled);
    }

    private void Update()
    {
        if(isTrackingEnabled)
        {
            UpdatePlayerPosition();
            UpdateTrackedItems();
            UpdateTrackingVisual();
        }
    }

    private void UpdatePlayerPosition()
    {
        currentPosition = transform.position;
    }

    private void UpdateTrackedItems()
    {
        trackedItems.Sort((x, y) =>
        {
            return (currentPosition - x.GetWorldLocation()).sqrMagnitude
                    .CompareTo((currentPosition - y.GetWorldLocation()).sqrMagnitude);
        });

        if (trackedItems.Count > 0)
        {
            currentlyTrackedPosition = trackedItems[0].GetWorldLocation();
        }
        else
        {
            currentlyTrackedPosition = Vector3.zero;
        }
    }

    private void UpdateTrackingVisual()
    {
        Vector3 targetDirection = currentlyTrackedPosition - trackingIndicator.position;
        float rotationZ = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, rotationZ);

        trackingIndicator.rotation = Quaternion.Slerp(trackingIndicator.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
