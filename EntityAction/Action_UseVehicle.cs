using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActionUseVehicleProperties : ActionInteractProperties
{
    [SerializeField] private Vehicle vehicle;
    [SerializeField] private VehicleInputController vehicleInput;

    public Vehicle Vehicle => vehicle;
    public VehicleInputController VehicleInput => vehicleInput;
}

public class Action_UseVehicle : ActionInteract
{
    [SerializeField] private GameObject visualModel;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private CharacterMovement characterMovement;
    [SerializeField] private ThirdPersonCamera camera;
    [SerializeField] private CharacterInputController characterInput;
    [SerializeField] private CharacterVehicleHandler characterVehicleHandler;

    private bool inVehicle;

    private void Start()
    {
        OnStartAction.AddListener(OnActionStart);
        OnEndAction.AddListener(OnActionEnd);
    }

    private void Update()
    {
        if (inVehicle)
        {
            IsCanEnd = (Properties as ActionUseVehicleProperties).Vehicle.LinearVelocity < 2f;
            IsCanStart = IsCanEnd;
        }
    }

    private void OnDestroy()
    {
        OnStartAction.RemoveListener(OnActionStart);
        OnEndAction.RemoveListener(OnActionEnd);
    }

    private void OnActionStart()
    {
        if (IsCanEnd)
        {
            OnActionEnd();
            return;
        }
        ActionUseVehicleProperties properties = Properties as ActionUseVehicleProperties;
        inVehicle = true;
        properties.Vehicle.enabled = true;
        properties.Vehicle.Unfreeze();
        properties.Vehicle.SetEngine(true);
        properties.VehicleInput.enabled = true;
        characterVehicleHandler.SetVehicle(properties.Vehicle);
        characterInput.enabled = false;
        visualModel.transform.localPosition = visualModel.transform.localPosition + new Vector3(0, -100, 0);
        characterController.enabled = false;
        characterMovement.enabled = false;
        properties.VehicleInput.AssignCamera(camera);
    }

    private void OnActionEnd()
    {
        if (IsCanEnd == false) return;
        inVehicle = false;
        ActionUseVehicleProperties properties = Properties as ActionUseVehicleProperties;
        properties.Vehicle.enabled = false;
        characterVehicleHandler.SetVehicle(null);
        properties.Vehicle.SetEngine(false);
        properties.VehicleInput.enabled = false;
        properties.Vehicle.SetTargetControl(Vector3.zero);
        if (properties.Vehicle.IsLongExit)
        {
            StartCoroutine(LongExit(properties.Vehicle.transform));
        }
        else
        {
            properties.Vehicle.Freeze();
            characterInput.enabled = true;
            visualModel.transform.localPosition = FindTargetToExit(properties.Vehicle.transform);
            characterController.enabled = true;
            characterMovement.enabled = true;
            characterInput.AssignCamera(camera);
        }
    }

    private IEnumerator LongExit(Transform vehicle)
    {
        yield return new WaitForSeconds(1f);
        vehicle.GetComponent<Vehicle>().Freeze();
        characterInput.enabled = true;
        visualModel.transform.localPosition = FindTargetToExit(vehicle);
        characterController.enabled = true;
        characterMovement.enabled = true;
        characterInput.AssignCamera(camera);
    }

    private Vector3 FindTargetToExit(Transform vehicle)
    {
        if (Physics.Raycast(vehicle.position, vehicle.right, 100f) && Physics.Raycast(vehicle.position, -vehicle.right, 100f))
        {
            return vehicle.position + vehicle.up;
        }
        else if (Physics.Raycast(vehicle.position, -vehicle.right, 100f))
        {
            return vehicle.position + vehicle.right * 2;
        }
        else if (Physics.Raycast(vehicle.position, vehicle.right, 100f))
        {
            return vehicle.position + (-vehicle.right * 2);
        }
        return vehicle.position + vehicle.right * 2;
    }
}
