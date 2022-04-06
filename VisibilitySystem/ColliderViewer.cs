using UnityEngine;

public class ColliderViewer : MonoBehaviour
{
    [SerializeField] private float viewAngle;
    [SerializeField] private float viewDistance;
    [SerializeField] private float viewHeight;
    [SerializeField] private Transform head;

    public bool IsObjectVisible(GameObject target)
    {
        if (target.TryGetComponent(out ColliderViewpoints viewPoints))
        {
            return viewPoints.IsVisibleFromPoint(head.position, head.forward, viewAngle, viewDistance);
        }
        else
        {
            return false;
        }
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.matrix = Matrix4x4.TRS(head.position, transform.rotation, Vector3.one);
        Gizmos.DrawFrustum(Vector3.zero, viewAngle, 0, viewDistance, 1);
    }
#endif
}
