using UnityEngine;
using UnityEngine.Events;

public class QuestCollector : MonoBehaviour
{
    public UnityAction<Quest> QuestReceived;
    public UnityAction<Quest> QuestCompleted;
    public UnityAction<Quest> LastQuestCompleted;

    [SerializeField] private Quest startQuest;
    [SerializeField] private Quest currentQuest;

    public Quest CurrentQuest => currentQuest;

    private void Start()
    {
        if (startQuest != null)
        {
            AssignQuest(startQuest);
        }
    }

    public void AssignQuest(Quest quest)
    {
        currentQuest = quest;
        QuestReceived?.Invoke(currentQuest);
        currentQuest.Completed += OnQuestCompleted;
        SoundManager.Instanse.Play(SoundManager.Instanse.QuestStarted);
    }

    private void OnQuestCompleted()
    {
        currentQuest.Completed -= OnQuestCompleted;
        QuestCompleted?.Invoke(currentQuest);
        if (currentQuest.NextQuest != null)
        {
            AssignQuest(currentQuest.NextQuest);
        }
        else
        {
            LastQuestCompleted?.Invoke(currentQuest);
        }
    }
}
