using UnityEngine;

public class EnemyAttackCheck : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == GameManager.i.playerGO)
        {
            GetComponentInParent<EnemyHandler>().SetAggroStatus(false);
            GetComponentInParent<EnemyHandler>().SetAttacking(true);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject == GameManager.i.playerGO)
        {
            GetComponentInParent<EnemyHandler>().SetAggroStatus(true);
            GetComponentInParent<EnemyHandler>().SetAttacking(false);
        }
    }
}
