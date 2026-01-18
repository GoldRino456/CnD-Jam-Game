using System;
using UnityEngine;

public interface ITrackable
{
    public int TrackableId { get; set; }

    event Action<int> OnDestroyCalled;
    public Vector3 GetWorldLocation();
    public GameObject GetGameObjectRef();
}
