using UnityEngine;

public class AggroCheck : MonoBehaviour
{
    GameObject Player;
    Enemy enemy;

    float agroDistance = 4f;
    void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        enemy = GetComponent<Enemy>();
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
            enemy.SetAgroStatus(true);
        }

        else
        {
            enemy.SetAgroStatus(false);
        }
    }

    private void AttackCheck()
    {
        if(enemy.RaycastAttackSweep() == true)
        {
            enemy.SetAttackStatus(true);
        }

        else if(enemy.RaycastAttackSweep() == false)
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
