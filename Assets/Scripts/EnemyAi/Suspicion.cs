using System.Collections;
using UnityEngine;
[CreateAssetMenu(fileName ="Suspicion Settings", menuName = "Objects")]
public class Suspicion : ScriptableObject
{
    public int noticedPlayer;
    public float suspicion = 0f;
    public float suspicionThreshold = 2f;
    [SerializeField] private float decayFactor = 2f;
    public Coroutine decayRoutine;
    public IEnumerator DecayRoutine()
    {
        while (suspicion > 0f)
        {
            suspicion -= Time.deltaTime / decayFactor;
            yield return null;
        }

        suspicion = 0f;
        decayRoutine = null;
    }
    public void IncreaseSuspicion()
    {
        suspicion += Time.deltaTime / noticedPlayer;
    }
}