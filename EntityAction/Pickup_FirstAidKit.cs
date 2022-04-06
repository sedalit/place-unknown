using UnityEngine;

public class Pickup_FirstAidKit : TriggerInteraction
{
    [SerializeField] private int restoreAmount;

    protected override void OnEndAction(GameObject owner)
    {
        base.OnEndAction(owner);

        Destructible des = owner.transform.root.GetComponent<Destructible>();

        if (des != null)
        {
            des.Heal(restoreAmount);
            Destroy(gameObject);
        }
    }
}
