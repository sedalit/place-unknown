using System.Collections.Generic;
using UnityEngine;

public class CharacterInputController : MonoBehaviour
{
    [SerializeField] private CharacterMovement targetCharacterMovement;
    [SerializeField] private ThirdPersonCamera camera;
    [SerializeField] private Shooter targetShooter;
    [SerializeField] private Vector3 aimingOffset;
    [SerializeField] private SpreadShootRig spreadShootRig;
    [SerializeField] private Vector3 defaultOffset;
    [SerializeField] private GameObject dialogPanel;
    [SerializeField] private UIQuestInfo questInfo;

    private bool isDisabled = false;

    private void Start()
    {
        Cursor.visible = false;

        GameStateController.GameStateChanged += OnGameStateChanged;
    }

    private void OnDestroy()
    {
        GameStateController.GameStateChanged -= OnGameStateChanged;
    }

    private void Update()
    {
        if (isDisabled) return;
        UpdateCursorState();

        if (dialogPanel.activeInHierarchy)
        {
            targetCharacterMovement.targetDirectionControl = Vector3.Lerp(targetCharacterMovement.targetDirectionControl, Vector3.zero, 1f);
            return;
        }

        targetCharacterMovement.targetDirectionControl = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        camera.RotationControl = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        if (targetCharacterMovement.targetDirectionControl != Vector3.zero || targetCharacterMovement.IsAiming) camera.IsRotateTarget = true;
        else camera.IsRotateTarget = false;

        if (Input.GetButtonDown("Jump")) targetCharacterMovement.Jump();
        if (Input.GetKey(KeyCode.Tab)) targetCharacterMovement.UseJetPack();
        else if (targetCharacterMovement.IsJet) targetCharacterMovement.UnUseJetPack();
        if (Input.GetMouseButtonDown(1))
        {
            targetCharacterMovement.Aiming();
            camera.SetTargetOffset(aimingOffset);
        }
        if (Input.GetMouseButtonUp(1))
        {
            targetCharacterMovement.UnAiming();
            camera.SetDefaultOffset();
        }
        if (Input.GetMouseButton(0) && targetCharacterMovement.IsAiming)
        {
            targetShooter.Shoot();
            spreadShootRig.Spread();
        }
        if (Input.GetKeyDown(KeyCode.LeftControl)) targetCharacterMovement.Crouch();
        if (Input.GetKeyUp(KeyCode.LeftControl)) targetCharacterMovement.UnCrouch();
        if (Input.GetKeyDown(KeyCode.LeftShift)) targetCharacterMovement.Sprint();
        if (Input.GetKeyUp(KeyCode.LeftShift)) targetCharacterMovement.UnSprint();
        if (Input.GetKey(KeyCode.Q)) questInfo.ShowPanel();
        if (Input.GetKeyUp(KeyCode.Q)) questInfo.HidePanel();
    }
    public void AssignCamera(ThirdPersonCamera newCamera)
    {
        camera = newCamera;
        camera.IsRotateTarget = false;
        camera.SetTargetOffset(defaultOffset);
        camera.SetTarget(targetCharacterMovement.transform);
    }

    private void UpdateCursorState()
    {
        Cursor.visible = dialogPanel.activeInHierarchy;
        camera.enabled = !dialogPanel.activeInHierarchy;
        Cursor.lockState = Cursor.visible ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private void OnGameStateChanged(GameState state)
    {
        if (state == GameState.Pause)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            isDisabled = true;
        }

        else isDisabled = false;
    }
}
