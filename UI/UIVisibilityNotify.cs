using UnityEngine;
using UnityEngine.UI;

public class UIVisibilityNotify : MonoBehaviour
{
    [SerializeField] private GameObject notify;

    public void Show() => notify.SetActive(true);

    public void Hide() => notify.SetActive(false);

}
