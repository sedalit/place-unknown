using UnityEngine;
using UnityEngine.UI;

public class UIVisibilityIndicator : MonoBehaviour
{
    [SerializeField] private Text visibilityText;
    [SerializeField] private Text statusText;
    [SerializeField] private ColliderViewpoints targetViewpoints;

    private void Update()
    {
        visibilityText.text = targetViewpoints.IsVisible ? "Вас заметили!" : "Вас не видно";
        statusText.text = targetViewpoints.IsInvisibility ? "Включен чит невидимости" : "Чит невидимости выключен";
    }
}
