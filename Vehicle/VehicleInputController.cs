using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleInputController : MonoBehaviour
{
    [SerializeField] private Vehicle vehicle;
    [SerializeField] private Vector3 cameraOffset;

    private ThirdPersonCamera camera;

    protected virtual void Start()
    {
        if (camera != null)
        {
            camera.IsRotateTarget = false;
        }
    }

    protected virtual void Update()
    {
        vehicle.SetTargetControl(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Jump"), Input.GetAxis("Vertical")));
        camera.RotationControl = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }

    public void AssignCamera(ThirdPersonCamera newCamera)
    {
        camera = newCamera;
        camera.IsRotateTarget = false;
        camera.SetTargetOffset(cameraOffset);
        camera.SetTarget(vehicle.transform);
    }
}
