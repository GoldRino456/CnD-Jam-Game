using System;
using UnityEngine;

public class FactionMember : MonoBehaviour
{
    [SerializeField] private Faction _currentFaction = Faction.Neutral;
    public Faction CurrentFaction { get => _currentFaction; set => _currentFaction = value; }
}

[Serializable]
public enum Faction
{
    PlagueDoctors = 0,
    Neutral = 1,
    Infected = 2
}
