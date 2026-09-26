using UnityEngine;
public class PlayerMovement_topdown : PlayerMovement
{
    Vector2 moveInput;
    private bool facingRight=true;
    public override void Initialize()
    {
        base.Initialize();
        input.OnMoveEvent += OnMove;
    }
    public override void OnMove(Vector2 _moveInput)
    {
        moveInput = _moveInput;
    }
    void FixedUpdate()
    {
        Vector2 moveSpeed = moveInput.normalized;
        RB.linearVelocity = new Vector2(moveInput.x * stats.SPEED, 
            moveInput.y *.5f * stats.SPEED);
        FlipPlayer();
    }
    private void FlipPlayer()
    {
        if (moveInput.x < 0 && facingRight)
        {
            RB.transform.localScale = new Vector3(-1f, 1f, 1f);
            facingRight = false;
        }
        else if (moveInput.x > 0 && !facingRight)
        {
            RB.transform.localScale = Vector3.one;
            facingRight = true;
        }
    } 

}
