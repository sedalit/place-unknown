using UnityEngine;

[System.Serializable]
public class CharacterAnimatorParametersName
{
    public string NormalizeMovementX;
    public string NormalizeMovementZ;
    public string Sprint;
    public string Aiming;
    public string Crouch;
    public string Ground;
    public string Jump;
    public string DistanceToGround;
    public string GroundSpeed;
    public string Climb;
    public string JetPack;
}
[System.Serializable]
public class AnimationCrossFadeParameters
{
    public string name;
    public float duration;
}
public class CharacterAnimationState : MonoBehaviour
{
    private const float INPUT_CONTROL_LERP_RATE = 10f;

    [SerializeField] private CharacterController targetCharacterController;
    [SerializeField] private CharacterMovement targetCharacterMove;
    [SerializeField] private Animator targetAnimator;
    [SerializeField] [Space(5)] private CharacterAnimatorParametersName animatorParameterName;

    [Header("Fades")]
    [SerializeField] [Space(5)] private AnimationCrossFadeParameters fallFade;
    [SerializeField] private float minDistanceToGroundToBeFalled;
    [SerializeField] private AnimationCrossFadeParameters jumpIdleFade;
    [SerializeField] private AnimationCrossFadeParameters jumpMoveFade;

    private Vector3 inputControl;

    private void Update()
    {
        Vector3 movementSpeed = transform.InverseTransformDirection(targetCharacterController.velocity);
        inputControl = Vector3.MoveTowards(inputControl, targetCharacterMove.targetDirectionControl, Time.deltaTime * INPUT_CONTROL_LERP_RATE);

        targetAnimator.SetFloat(animatorParameterName.NormalizeMovementX, inputControl.x);
        targetAnimator.SetFloat(animatorParameterName.NormalizeMovementZ, inputControl.z);

        Vector3 groundSpeed = targetCharacterController.velocity;
        groundSpeed.y = 0;

        targetAnimator.SetFloat(animatorParameterName.GroundSpeed, groundSpeed.magnitude);
        targetAnimator.SetFloat(animatorParameterName.Jump, targetCharacterController.velocity.y);
        targetAnimator.SetBool(animatorParameterName.Sprint, targetCharacterMove.IsSprint);
        targetAnimator.SetBool(animatorParameterName.Aiming, targetCharacterMove.IsAiming);
        targetAnimator.SetBool(animatorParameterName.Crouch, targetCharacterMove.IsCrouch);
        targetAnimator.SetBool(animatorParameterName.Ground, targetCharacterMove.IsGrounded);
        targetAnimator.SetBool(animatorParameterName.JetPack, targetCharacterMove.IsJet);

        if (targetCharacterMove.IsJump && targetCharacterMove.IsJet == false)
        {
            if (groundSpeed.magnitude <= 0.01f)
            {
                CrossFade(jumpIdleFade);
            }
            if (groundSpeed.magnitude >= 0.01f)
            {
                CrossFade(jumpMoveFade);
            }
        }

        if (targetCharacterMove.IsGrounded != true)
        {
            targetAnimator.SetFloat(animatorParameterName.Jump, targetCharacterController.velocity.y);
            if (movementSpeed.y > 0 && targetCharacterMove.DistanceToGround > minDistanceToGroundToBeFalled)
            {
                CrossFade(fallFade);
            }
        }

        targetAnimator.SetFloat(animatorParameterName.DistanceToGround, targetCharacterMove.DistanceToGround);
    }

    private void CrossFade(AnimationCrossFadeParameters parameters)
    {
        targetAnimator.CrossFade(parameters.name, parameters.duration);
    }
}
