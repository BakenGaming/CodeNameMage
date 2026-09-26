using System.Collections;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    void OnEnable()
    {
        StartCoroutine(DestroyThis());
    }
    IEnumerator DestroyThis()
    {
        yield return new WaitForSeconds(1f);
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}
