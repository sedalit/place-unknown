using UnityEngine;

public class ShootingVehicleInputControl : VehicleInputController
{
    [SerializeField] private Shooter shooter;
    [SerializeField] private Turret turret;
    [SerializeField] private Transform aim;

    protected override void Update()
    {
        base.Update();

        aim.position = shooter.Camera.transform.position + shooter.Camera.transform.forward * 30;
        turret.rotationControlY = Input.GetAxisRaw("Mouse Y");
        if (Input.GetMouseButton(0))
        {
            shooter.Shoot();
        }
    }
}
