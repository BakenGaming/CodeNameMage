using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IPlayerMovementHandler
{
    #region Variables
    public static InputReader input;
    public static PlayerStatsSO stats;
    public static Rigidbody2D RB;
    

    public virtual void Initialize()
    {
        input = GetComponent<PlayerHandler>().Input;
        stats = GetComponent<PlayerHandler>().Stats;
        RB = GetComponent<Rigidbody2D>();

    }
    public virtual void OnDisable()
    {
        input.OnMoveEvent -= OnMove;
    }
    #endregion
    public virtual void OnMove(Vector2 _moveInput)
    {

    }

    internal void OnMove()
    {
        throw new NotImplementedException();
    }
}
