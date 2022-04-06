using UnityEngine;

public class UIMainMenuController : MonoBehaviour
{
    public void OnButtonStart() => SceneTransition.SwitchToScene("SampleScene");

    public void OnButtonExit() => Application.Quit();

}
