using UnityEngine;

public class Ladder_Climb : TriggerInteraction
{
    [SerializeField] private Ladder attachedLadder;

    protected override void OnEndAction(GameObject owner)
    {
        attachedLadder.Use(owner);
        base.OnEndAction(owner);
    }
}
