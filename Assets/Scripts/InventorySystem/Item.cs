using UnityEngine;

public class Item : MonoBehaviour, ICollectable
{
    public delegate void ItemDelegateData(ItemData itemData);
    public static event ItemDelegateData OnItemCollected;
    protected ItemData itemData;
    public virtual void Initialize(ItemData data)
    {
        itemData = data;
    }
    public virtual void Collect()
    {
        OnItemCollected?.Invoke(itemData);
    }

    public virtual void SetTarget(Vector3 targetPosition)
    {
        
    }
}
