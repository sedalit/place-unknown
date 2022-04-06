using UnityEngine;

public class StealthKillTrigger : TriggerInteraction
{
    [SerializeField] private StealthKillProperties useProperties;

    protected override void InitActionProperties() => action.SetProperties(useProperties);

}
