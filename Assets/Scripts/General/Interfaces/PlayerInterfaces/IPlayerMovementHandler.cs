using UnityEngine;

public interface IPlayerMovementHandler
{
    public void Initialize(InputReader _i, PlayerMovementStats _s, Collider2D _b, Collider2D _f);
}
