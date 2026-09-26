using UnityEngine;

public interface ITriggerHandler
{
    public bool isAttacking {get; set;}
    public bool isAggroed {get; set;}
    public void SetAggroStatus(bool isAggroed);
    public void SetAttacking(bool isAttacking);
}
