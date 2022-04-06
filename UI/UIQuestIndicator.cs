using UnityEngine;
using UnityEngine.UI;

public class UIQuestIndicator : MonoBehaviour
{
    [SerializeField] private QuestCollector questCollector;
    [SerializeField] private Camera camera;
    [SerializeField] private Image indicator;
    [SerializeField] private Text distanceText;
    [SerializeField] private Transform playerTransform;

    private Transform targetPosition;
    private bool hasActiveQuest;

    private void OnEnable() => questCollector.QuestReceived += OnQuestReceived;

    private void OnDisable() => questCollector.QuestReceived -= OnQuestReceived;

    private void Update()
    {
        if (camera == null) camera = Camera.main;
        Vector3 pos = camera.WorldToScreenPoint(targetPosition.position);
        if (pos.z > 0)
        {
            if (pos.x < 0) pos.x = 0;
            if (pos.x > Screen.width) pos.x = Screen.width;

            if (pos.y < 0) pos.y = 0;
            if (pos.y > Screen.height) pos.y = Screen.height;

            indicator.transform.position = pos;
        }
        distanceText.text = ((int)Vector3.Distance(playerTransform.position, targetPosition.position)).ToString() + " m.";

    }

    private void OnQuestReceived(Quest quest) => targetPosition = quest.ReachPoint;

}
