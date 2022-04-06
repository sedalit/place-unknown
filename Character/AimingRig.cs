using UnityEngine;

public class AimingRig : MonoBehaviour
{
    [SerializeField] private CharacterMovement targetCharacter;
    [SerializeField] private UnityEngine.Animations.Rigging.Rig[] rigs;
    [SerializeField] private float weightLerpRate;

    private float targetWeight;

    private void Update()
    {
        for (int i = 0; i < rigs.Length; i++)
        {
            rigs[i].weight = Mathf.MoveTowards(rigs[i].weight, targetWeight, weightLerpRate * Time.deltaTime);
        }
        if (targetCharacter.IsAiming) targetWeight = 1;
        else targetWeight = 0;
    }

}
