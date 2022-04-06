using UnityEngine;

public class Turret : Weapon
{
    public float rotationControlY;

    [SerializeField] private Transform turretBase;

    [SerializeField] private Transform turretGun;

    [SerializeField] private Transform aim;

    [SerializeField] private float rotationLerpFactor;

    [SerializeField] private Transform gunForward;

    protected Quaternion BaseTargetRotation;
    protected Quaternion BaseRotation;
    protected Quaternion GunTargetRotation;
    protected Vector3 GunRotation;

    private float deltaY;

    protected override void Update()
    {
        base.Update();

        LookOnAim();
    }

    private void LookOnAim()
    {

        BaseTargetRotation = Quaternion.LookRotation(new Vector3(aim.position.x, turretGun.position.y, aim.position.z) - turretGun.position);
        BaseRotation = Quaternion.RotateTowards(turretBase.localRotation, BaseTargetRotation, Time.deltaTime * rotationLerpFactor);
        turretBase.localRotation = BaseRotation;

        GunTargetRotation = Quaternion.LookRotation(aim.position);
        GunRotation = Quaternion.RotateTowards(turretGun.rotation, GunTargetRotation, Time.deltaTime * rotationLerpFactor).eulerAngles;
        deltaY += rotationControlY;
        turretGun.rotation = BaseRotation * Quaternion.Euler(0, -90f,  Mathf.Clamp(deltaY, -15, 30));
    }

    public void SetAim(Transform aim) => this.aim = aim;
}
