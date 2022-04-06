using UnityEngine;

public class PatrolPath : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private PatrolPathNode[] allNodes;

    public int Id => id;

    private void Start() => UpdatePathNode();

    public PatrolPathNode GetRandomNode()
    {
        return allNodes[Random.Range(0, allNodes.Length)];
    }

    public PatrolPathNode GetNextNode(ref int index)
    {
        index = Mathf.Clamp(index, 0, allNodes.Length - 1);
        index++;
        if (index >= allNodes.Length)
        {
            index = 0;
        }
        return allNodes[index];
    }

    [ContextMenu("Update Path Node")]
    private void UpdatePathNode()
    {
        allNodes = new PatrolPathNode[transform.childCount];

        for (int i = 0; i < allNodes.Length; i++)
        {
            allNodes[i] = transform.GetChild(i).GetComponent<PatrolPathNode>();
        }
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (allNodes == null) return;
        Gizmos.color = Color.red;
        for (int i = 0; i < allNodes.Length - 1; i++)
        {
            Gizmos.DrawLine(allNodes[i].transform.position + new Vector3(0, 0.5f, 0), allNodes[i + 1].transform.position + new Vector3(0, 0.5f, 0));
        }
        Gizmos.DrawLine(allNodes[0].transform.position + new Vector3(0, 0.5f, 0), allNodes[allNodes.Length - 1].transform.position + new Vector3(0, 0.5f, 0));
    }
#endif
}
