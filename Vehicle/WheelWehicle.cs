using UnityEngine;

[System.Serializable]
public class WheelAxis
{
    [SerializeField] private WheelCollider leftWheelCollider;
    [SerializeField] private WheelCollider rightWheelCollider;
    [SerializeField] private Transform leftWheelMesh;
    [SerializeField] private Transform rightWheelMesh;
    [SerializeField] private bool isMotor;
    [SerializeField] private bool isSteering;
    public bool IsMotor => isMotor;
    public bool IsSteering => isSteering;

    public void SetTorque(float torque)
    {
        if (isMotor == false) return;
        leftWheelCollider.motorTorque = torque;
        rightWheelCollider.motorTorque = torque;
    }
    public void BrakeTorque(float brake)
    {
        leftWheelCollider.brakeTorque = brake;
        rightWheelCollider.brakeTorque = brake;
    }
    public void SetStreering(float steering)
    {
        if (isSteering == false) return;
        leftWheelCollider.steerAngle = steering;
        rightWheelCollider.steerAngle = steering;
    }
    public void UpdateMeshTransform()
    {
        UpdateWheelTransform(ref leftWheelCollider, ref leftWheelMesh);
        UpdateWheelTransform(ref rightWheelCollider, ref rightWheelMesh);
    }
    private void UpdateWheelTransform(ref WheelCollider wheelCollider, ref Transform wheelTransform)
    {
        Vector3 position;
        Quaternion rotation;
        wheelCollider.GetWorldPose(out position, out rotation);
        wheelTransform.position = position;
        wheelTransform.rotation = rotation;
    }
}
[RequireComponent(typeof(Rigidbody))]
public class WheelWehicle : Vehicle
{
    [SerializeField] private WheelAxis[] wheelAxes;
    [SerializeField] private float maxMotorTorque;
    [SerializeField] private float maxSteerAngle;
    [SerializeField] private float breakTorque;

    public override float LinearVelocity => rigidbody.velocity.magnitude;

    private Rigidbody rigidbody;


    protected override void Start()
    {
        base.Start();
        rigidbody = GetComponent<Rigidbody>();
        Freeze();
    }

    private void FixedUpdate()
    {
        float targetMotor = maxMotorTorque * TargetInputControl.z;
        float targetBrake = breakTorque * TargetInputControl.y;
        float steering = maxSteerAngle * TargetInputControl.x;
        for (int i = 0; i < wheelAxes.Length; i++)
        {
            if (targetBrake == 0 && LinearVelocity < maxLinearVelocity)
            {
                wheelAxes[i].BrakeTorque(0);
                wheelAxes[i].SetTorque(targetMotor);
            }
            if (LinearVelocity > maxLinearVelocity)
            {
                wheelAxes[i].BrakeTorque(targetBrake * 0.2f);
            }
            else
            {
                wheelAxes[i].BrakeTorque(targetBrake);
            }

            wheelAxes[i].SetStreering(steering);
            wheelAxes[i].UpdateMeshTransform();
        }
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        Freeze();
    }

    public override void Freeze() => rigidbody.constraints = RigidbodyConstraints.FreezeAll;

    public override void Unfreeze() => rigidbody.constraints = RigidbodyConstraints.None;
}
