using System;
using UnityEngine;

[CreateAssetMenu(menuName ="Player Movement Stats")]
public class PlayerMovementStats : ScriptableObject
{
    #region Variables and Setup

    [Header("Movement Variables")]
    [Range (0f,100f)] public float moveSpeed = 12.5f;

    [Header("Ground Detection")]
    public float groundDetectionRayLength = .02f;
    public float headDetectionRayLength = .02f;
    [Range (0f,1f)] public float headWidth = .75f;

    [Header("Jump")]
    public float jumpHeight = 6.5f;
    [Range (1f, 1.1f)] public float jumpHeightCompensationFactor = 1.054f;
    public float timeTillJumpApex = .35f;
    [Range (0.01f, 5f)] public float gravityOnReleaseMultiplier = 2f;
    public float maxFallSpeed = 26f;
    [Range (0f, 1f)] public float timeForUpwardsCancel = .027f;
    public int numberOfJumpsAllowed = 1;
    
    [Header("Jump Cut")]
    [Range (0f, 1f)] public float apexThreshold = .97f;
    [Range (0f, 1f)] public float apexHangTime = .75f;

    [Header("Jump Buffer")]
    [Range (0f, 1f)] public float jumpBufferLength;
    
    [Header("Jump Coyote Time")]
    [Range (0f, 1f)] public float jumpCoyoteTime;

    [Header("Debug")]
    public bool debugShowIsGroundedBox;
    public bool debugShowHeadBumpBox;

    [Header("Jump Visualation Tool")]
    public bool showJumpArc = false;
    public bool stopOnCollision = true;
    public bool drawRight = true;
    [Range (5,100)] public int arcResolution = 20;
    [Range (0,500)] public int visualizationSteps = 90;

    public float Gravity {get; private set;}
    public float InitialJumpVelocity {get; private set;}
    public float AdjustedJumpHeight {get; private set;}
    #endregion
    
    #region Jump Calculations
    private void OnValidate()
    {
        CalculateValues();        
    }
    private void CalculateValues()
    {
        AdjustedJumpHeight = jumpHeight * jumpHeightCompensationFactor;
        Gravity = -(2f * AdjustedJumpHeight) / Mathf.Pow(timeTillJumpApex, 2f);
        InitialJumpVelocity = Mathf.Abs(Gravity) * timeTillJumpApex;
    }
    #endregion 
}
