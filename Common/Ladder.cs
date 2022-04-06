using UnityEngine;

public class Ladder : MonoBehaviour
{
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;

    public void Use(GameObject target)
    {
        if (target.transform.position.x == startPoint.position.x || target.transform.position.z == startPoint.position.z ||
            Vector3.Distance(target.transform.position, startPoint.position) < Vector3.Distance(target.transform.position, endPoint.position))
        {
            target.transform.position = endPoint.position;
        }
        else
        {
            target.transform.position = startPoint.position;
        }
    }
}
