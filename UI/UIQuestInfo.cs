using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIQuestInfo : MonoBehaviour
{
    [SerializeField] private QuestCollector questCollector;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Text questName;
    [SerializeField] private Text questDescription;

    private void Start()
    {
        questCollector.QuestReceived += OnQuestReceived;
        questCollector.QuestCompleted += OnQuestCompleted;
    }

    private void OnDestroy()
    {
        questCollector.QuestReceived += OnQuestReceived;
        questCollector.QuestCompleted += OnQuestCompleted;
    }

    public void ShowPanel() => canvasGroup.DOFade(1f, 0.5f);

    public void HidePanel() => canvasGroup.DOFade(0f, 0.5f);

    private void OnQuestReceived(Quest quest)
    {
        questName.text = quest.Properties.Task;
        questDescription.text = quest.Properties.Description;
    }

    private void OnQuestCompleted(Quest quest)
    {
        questName.text = "";
        questDescription.text = "";
    }
}
