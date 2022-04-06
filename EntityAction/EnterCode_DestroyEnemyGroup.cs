using UnityEngine;

public class EnterCode_DestroyEnemyGroup : TriggerInteraction
{
    [SerializeField] private Transform enemyGroup;
    protected override void OnEndAction(GameObject owner)
    {
        base.OnEndAction(owner);
        foreach (var drone in enemyGroup.GetComponentsInChildren<Drone>())
        {
            drone.Deactivate();
        }
    }
}
