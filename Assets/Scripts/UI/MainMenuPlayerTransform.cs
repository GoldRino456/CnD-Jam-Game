using System.Collections;
using UnityEngine;

public class MainMenuPlayerTransform : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] GameObject transformationParticles;

    void Start()
    {
        StartCoroutine(PlayerTransform());
    }

    private IEnumerator PlayerTransform()
    {
        while (true)
        {
            animator.SetTrigger("Human");
            yield return new WaitForSeconds(5f);

            SpawnEffect();
            animator.SetTrigger("HumanWolf");
            yield return new WaitForSeconds(5f);

            SpawnEffect();
            animator.SetTrigger("Wolf");
            yield return new WaitForSeconds(5f);

            SpawnEffect();
        }


    }

    private void SpawnEffect()
    {
        GameObject effect = Instantiate(transformationParticles, transform.position, transform.rotation);
        Destroy(effect, 2.1f);
        
    }
}
