using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [HideInInspector] public bool IsRotateTarget;
    [HideInInspector] public Vector2 RotationControl;

    [SerializeField] private Transform target;
    [SerializeField] private float distaneToTarget;
    [SerializeField] private float minDistance;
    [SerializeField] private float distanceLerpRate;
    [SerializeField] private float distanceOffsetFromCollision;
    [SerializeField] private float sensitiveMouse;

    [Header("Rotation Limit")]
    [SerializeField] private float maxLimitY;
    [SerializeField] private float minLimitY;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float changeOffsetRate;
    [SerializeField] private float rotateTargetLerpRate;

    private float deltaRotationX;
    private float deltaRotationY;
    private float currentDistance;
    private Vector3 targetOffset;
    private Vector3 defaultOffset;

    private void Start()
    {
        defaultOffset = offset;
        targetOffset = offset;
        transform.SetParent(null);
    }
    private void Update()
    {
        deltaRotationX += RotationControl.x * sensitiveMouse;
        deltaRotationY += RotationControl.y * -sensitiveMouse;
        deltaRotationY = ClampAngle(deltaRotationY, minLimitY, maxLimitY);

        offset = Vector3.MoveTowards(offset, targetOffset, Time.deltaTime * changeOffsetRate);

        Quaternion finalRotation = Quaternion.Euler(deltaRotationY, deltaRotationX, 0);
        Vector3 finalPosition = target.position - (finalRotation * Vector3.forward * distaneToTarget);
        finalPosition = AddLocalOffset(finalPosition);

        float targetDistance = distaneToTarget;
        RaycastHit hit;
        if (Physics.Linecast(target.position + new Vector3(0, offset.y, 0), finalPosition, out hit))
        {
            float distanceToHit = Vector3.Distance(target.position + new Vector3(0, offset.y, 0), hit.point);
            if (hit.transform != target)
            {
                if (distanceToHit < distaneToTarget)
                {
                    targetDistance = distanceToHit - distanceOffsetFromCollision;
                }
            }
        }

        currentDistance = Mathf.MoveTowards(currentDistance, targetDistance, Time.deltaTime * distanceLerpRate);
        currentDistance = Mathf.Clamp(currentDistance, minDistance, distaneToTarget);

        finalPosition = target.position - (finalRotation * Vector3.forward * currentDistance);
        finalPosition = AddLocalOffset(finalPosition);

        transform.rotation = finalRotation;
        transform.position = finalPosition;
        transform.position = AddLocalOffset(transform.position);
        
        if (IsRotateTarget)
        {
            Quaternion targetRotation = Quaternion.Euler(transform.rotation.x, transform.eulerAngles.y, transform.eulerAngles.z);
            target.rotation = Quaternion.RotateTowards(target.rotation, targetRotation, rotateTargetLerpRate);
        }
    }
    public void SetTargetOffset(Vector3 offset)
    {
        targetOffset = offset;
        this.offset = offset;
    }
    public void SetDefaultOffset()
    {
        targetOffset = defaultOffset;
    }
    public void SetTarget(Transform target)
    {
        this.target = target;
    }
    private Vector3 AddLocalOffset(Vector3 position)
    {
        Vector3 result = position;
        result += new Vector3(0, offset.y, 0);
        result += transform.right * offset.x;
        result += transform.forward * offset.z;
        return result;
    }
    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}
