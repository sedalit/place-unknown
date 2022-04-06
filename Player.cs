using UnityEngine;

public class Player : SingletoneBase<Player>
{
    [SerializeField] private UIVisibilityNotify visibilityNotifyUI;

    private int pursuersNumber;

    public void StartPursuet()
    {
        pursuersNumber++;
        visibilityNotifyUI.Show();
    }

    public void StopPursuet()
    {
        pursuersNumber--;
        if (pursuersNumber == 0) visibilityNotifyUI.Hide();
    }
}
