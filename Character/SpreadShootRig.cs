using UnityEngine;

public class SpreadShootRig : MonoBehaviour
{
    [SerializeField] private CharacterMovement targetCharacter;
    [SerializeField] private UnityEngine.Animations.Rigging.Rig rig;
    [SerializeField] private float weightLerpRate;

    private float targetWeight;

    private void Update()
    {
        rig.weight = Mathf.MoveTowards(rig.weight, targetWeight, weightLerpRate * Time.deltaTime);
        if (rig.weight == 1) targetWeight = 0;
    }

    public void Spread() => targetWeight = 1;

}
