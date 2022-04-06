using UnityEngine;
using UnityEngine.Events;

public class CharacterMovement : MonoBehaviour
{
    public UnityAction<Vector3> Land;

    public bool UpdatePosition;
    public Vector3 targetDirectionControl;
    public Vector3 directionControl;

    [SerializeField] private CharacterController characterController;
    [SerializeField] private JetPack jetPack;

    [Header("Movement")]
    [SerializeField] private float rifleRunSpeed;

    [SerializeField] private float rifleSprintSpeed;

    [SerializeField] private float aimingWalkSpeed;

    [SerializeField] private float aimingRunSpeed;

    [SerializeField] private float crouchSpeed;

    [SerializeField] private float jumpSpeed;

    [SerializeField] private float accelerationRate;

    [Header("State")]
    [SerializeField] private float crouchHeight;

    private bool isAiming;
    private bool isJump;
    private bool isCrouch;
    private bool isSprint;
    private bool isClimb;
    private float distanceToGround;
    private bool isJet;

    public bool IsAiming => isAiming;
    public bool IsJump => isJump;
    public bool IsCrouch => isCrouch;
    public bool IsSprint => isSprint;
    public bool IsClimb => isClimb;
    public float DistanceToGround => distanceToGround;
    public bool IsGrounded => distanceToGround <= 0.33f;
    public float CurrentSpeed => GetCurrentSpeedByState();
    public bool IsJet => isJet;

    private float baseCharacterHeight;
    private float baseCharacterHeightOffset;

    private Vector3 movementDirection;

    private float clickTime;
    private Vector3 flyDirection;

    private void Start()
    {
        baseCharacterHeight = characterController.height;
        baseCharacterHeightOffset = characterController.center.y;
        distanceToGround = 0f;
    }
    private void Update()
    {
        Move();
        UpdateDistanceToGround();
    }

    public void Jump()
    {
        if (isJump) return;
        if (IsGrounded == false) return;
        if (isCrouch) return;
        if (isAiming) return;
        isJump = true;
    }

    public void UseJetPack()
    {
        if (jetPack.CurrentFuel <= 5f)
        {
            UnUseJetPack();
            return;
        }
        isJet = true;
        jetPack.Jet();
    }

    public void UnUseJetPack()
    {
        isJet = false;
        jetPack.Stop();
        directionControl = Vector3.zero;
    }

    public void Crouch()
    {
        if (IsGrounded == false) return;
        if (isSprint) return;
        isCrouch = true;
        characterController.height = crouchHeight;
        characterController.center = new Vector3(0, characterController.center.y / 2, 0);
    }

    public void Climb()
    {
        isClimb = true;
    }

    public void UnCrouch()
    {
        isCrouch = false;
        characterController.height = baseCharacterHeight;
        characterController.center = new Vector3(0, baseCharacterHeightOffset, 0);
    }

    public void Sprint()
    {
        if (IsGrounded == false) return;
        if (isCrouch) return;
        isSprint = true;
    }
    public void UnSprint()
    {
        isSprint = false;
    }
    public void Aiming()
    {
        if (IsGrounded == false) return;
        isAiming = true;
    }
    public void UnAiming()
    {
        isAiming = false;
    }
    public float GetCurrentSpeedByState()
    {
        if (isCrouch)
        {
            return crouchSpeed;
        }
        if (isAiming)
        {
            if (isSprint) return aimingRunSpeed;
            else return aimingWalkSpeed;
        }
        if (isAiming != true)
        {
            if (isSprint) return rifleSprintSpeed;
            else return rifleRunSpeed;
        }
        return rifleRunSpeed;
    }

    private void Move()
    {
        directionControl = Vector3.MoveTowards(directionControl, targetDirectionControl, Time.deltaTime * accelerationRate);
        if (characterController.isGrounded == true)
        {
            movementDirection = directionControl * GetCurrentSpeedByState();

            if (isJump == true)
            {
                movementDirection.y = jumpSpeed;
                isJump = false;
            }
            movementDirection = transform.TransformDirection(movementDirection);
        }

        if (!isJet) movementDirection += Physics.gravity * Time.deltaTime;

        if (UpdatePosition) characterController.Move(movementDirection * Time.deltaTime);

        if (characterController.isGrounded && Mathf.Abs(movementDirection.y) > 2 && isJet == false)
        {
            Land?.Invoke(movementDirection);
        }
    }

    private void UpdateDistanceToGround()
    {
        if (isJet || UpdatePosition == false) return;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            distanceToGround = Vector3.Distance(transform.position, hit.point) - 1f;
        }
    }
}