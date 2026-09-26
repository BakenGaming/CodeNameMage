using UnityEngine;

public interface IEnemyMoveable
{
    public Rigidbody2D enemyRB {get; set;}
    public bool isFacingRight {get; set;}
    public void MoveEnemy(Vector2 velocity);
    public void CheckForLeftOrRightFacing(Vector2 velocity);
}
