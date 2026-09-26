using UnityEngine;

public class EnemyAggroCheck : MonoBehaviour
{
    void Awake()
    {
        GetComponentInParent<EnemyHandler>().SetAggroStatus(true);
    }
}
