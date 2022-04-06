using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StealthKillProperties : ActionInteractProperties
{
    [SerializeField] private Destructible target;
    public Destructible Target => target;
}
public class Action_StealthKill : ActionInteract
{
    [SerializeField] private ColliderViewpoints playerViewponts;
    [SerializeField] private CharacterMovement playerCharacterMovement;
    [SerializeField] private float timeToKill = 0.6f;
    [SerializeField] private Transform visualModel;
    [SerializeField] private int damage;

    private void Start() => OnEndAction.AddListener(OnActionEnd);

    private void OnDestroy() => OnEndAction.RemoveListener(OnActionEnd);

    private void OnActionEnd() => StartCoroutine(ApplyDamage());

    private IEnumerator ApplyDamage()
    {
        yield return new WaitForSeconds(timeToKill);
        StealthKillProperties properties = Properties as StealthKillProperties;
        if (playerViewponts.IsVisible == false)
        {
            properties.Target.ApplyDamage(properties.Target.CurrentHitPoints, null);
        }
        else
        {
            properties.Target.ApplyDamage(damage, owner.GetComponent<Destructible>());
        }

    }
}
