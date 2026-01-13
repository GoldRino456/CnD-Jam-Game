using UnityEngine;

public class AggroCheck : MonoBehaviour
{
    GameObject Player;
    Enemy enemy;
    void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        enemy = GetComponent<Enemy>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == Player)
        {
            enemy.SetAgroStatus(true);
            
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject == Player)
        {
            enemy.SetAgroStatus(false);
        }
    }
}
