using UnityEngine;

public class VehicleUseTrigger : TriggerInteraction
{
    [SerializeField] private ActionUseVehicleProperties useProperties;

    protected override void InitActionProperties() => action.SetProperties(useProperties);

}
