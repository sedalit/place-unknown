using UnityEngine;
using UnityEngine.UI;

public class UIEnemyStatusIndicator : MonoBehaviour
{
    [SerializeField] private Text indicatorText;
    [SerializeField] private AIAlienSoldier currentAI;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        GetComponent<Canvas>().worldCamera = mainCamera;
        currentAI.BehaviourChanged += OnBehaivourChanged;
    }

    private void OnDisable() => currentAI.BehaviourChanged -= OnBehaivourChanged;

    private void Update()
    {
        if (mainCamera == null) mainCamera = Camera.main;
        transform.LookAt(mainCamera.transform);
    }

    private void OnBehaivourChanged(AIBehaivour behaivour)
    {
        switch (behaivour)
        {
            case AIBehaivour.None:
                break;
            case AIBehaivour.Idle:
                indicatorText.text = "Стоит";
                break;
            case AIBehaivour.PatrolRandom:
                indicatorText.text = "Патрулирует";
                break;
            case AIBehaivour.CirclePatrol:
                indicatorText.text = "Патрулирует";
                break;
            case AIBehaivour.PursuitTarget:
                indicatorText.text = "Преследует цель";
                break;
            case AIBehaivour.SeekTarget:
                indicatorText.text = "Потерял цель";
                break;
            case AIBehaivour.FindLostTarget:
                indicatorText.text = "Ищет цель";
                break;
        }
    }
}
