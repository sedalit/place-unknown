using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CubeArea : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] public Vector3 movementArea;

    public int Id => id;

    public Vector3 GetRandomInsideZone()
    {
        Vector3 result = transform.position;
        result.x += Random.Range(-movementArea.x / 2, movementArea.x / 2);
        result.y += Random.Range(-movementArea.y / 2, movementArea.y / 2);
        result.z += Random.Range(-movementArea.z / 2, movementArea.z / 2);
        return result;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, movementArea);
    }
#endif
}
