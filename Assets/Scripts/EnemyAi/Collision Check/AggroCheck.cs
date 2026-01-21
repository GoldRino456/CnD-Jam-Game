using UnityEngine;

public class AggroCheck : MonoBehaviour
{
    GameObject Player;
    Enemy enemy;
    private bool _playedAggroSound;
    float agroDistance = 4f;

    [SerializeField] private FMODUnity.EventReference patrolSound;
    [SerializeField] private FMODUnity.EventReference attackSound;
    private FMOD.Studio.EventInstance patrolInstance;
    void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        enemy = GetComponent<Enemy>();

        patrolInstance = FMODUnity.RuntimeManager.CreateInstance(patrolSound);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(patrolInstance, gameObject);
    }

    void Update()
    {
        AgroCheck();
        AttackCheck();
        //enemy.DistanceBetweenPlayerAndEnemy();
    }

    private void AgroCheck()
    {
        if (enemy.RaycastChaseSweep() == true)
        {
            if (!_playedAggroSound)
            {
                FMODUnity.RuntimeManager.PlayOneShot(attackSound, transform.position);
                _playedAggroSound = true;
            }
            enemy.SetAgroStatus(true);
            patrolInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }

        else
        {
            enemy.SetAgroStatus(false);
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(patrolInstance, gameObject);
            _playedAggroSound = false;
        }
    }

    private void AttackCheck()
    {
        if (enemy.RaycastAttackSweep() == true)
        {
            enemy.SetAttackStatus(true);
        }

        else if (enemy.RaycastAttackSweep() == false)
        {
            enemy.SetAttackStatus(false);
        }

    }



    // void OnTriggerEnter2D(Collider2D collision)
    // {
    //     if(collision.gameObject == Player)
    //     {
    //         enemy.SetAgroStatus(true);

    //     }
    // }

    // void OnTriggerExit2D(Collider2D collision)
    // {
    //     if(collision.gameObject == Player)
    //     {
    //         enemy.SetAgroStatus(false);
    //     }
    // }
}
