using UnityEngine;
using UnityEngine.UI;

public class UIPanel : MonoBehaviour
{
    [SerializeField] private Image panelDisplay;

    public void Show() => panelDisplay.gameObject.SetActive(true);

    public void Hide() => panelDisplay.gameObject.SetActive(false);
}
