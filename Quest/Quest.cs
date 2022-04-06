using UnityEngine;
using UnityEngine.Events;

public class Quest : MonoBehaviour
{
    public UnityAction Completed;

    [SerializeField] private Quest nextQuest;
    [SerializeField] private QuestProperties questProperties;
    [SerializeField] private Transform reachPoint;

    public Quest NextQuest => nextQuest;
    public QuestProperties Properties => questProperties;
    public Transform ReachPoint => reachPoint;

    private void Update() => UpdateCompleteCondition();

    protected virtual void UpdateCompleteCondition() {}
}
