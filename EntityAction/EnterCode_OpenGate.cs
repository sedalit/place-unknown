using UnityEngine;

public class EnterCode_OpenGate : TriggerInteraction
{
    [SerializeField] private Gate connectedGate;
    protected override void OnEndAction(GameObject owner)
    {
        connectedGate.PlayAnimationClip();
        base.OnEndAction(owner);
    }
}
