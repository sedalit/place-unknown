using UnityEngine;

public class SpaceMarine : Destructible
{
    [SerializeField] private CharacterMovement characterMovement;
    [SerializeField] private float fallHeightToGetDamage;
    [SerializeField] private int damageFall;

    protected override void Start()
    {
        base.Start();
        characterMovement.Land += OnLand;
    }

    protected override void OnDeath()
    {
        characterMovement.Land -= OnLand;
        EventOnDeath?.Invoke();
    }

    private void OnLand(Vector3 velocity)
    {
        if (Mathf.Abs(velocity.y) < fallHeightToGetDamage) return;
        ApplyDamage(Mathf.Abs((int)velocity.y) * damageFall, this);
    }
}
