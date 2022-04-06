using UnityEngine;

public enum WeaponMode
{
    Primary,
    Secondary
}

[CreateAssetMenu]
public sealed class WeaponProperties : ScriptableObject
{
    [SerializeField] private WeaponMode mode;
    public WeaponMode Mode => mode;

    [SerializeField] private Projectile projectilePrefab;
    public Projectile ProjectilePrefab => projectilePrefab;

    [SerializeField] private float rateOfFire;
    public float RateOfFire => rateOfFire;

    [SerializeField] private float spreadRange;
    public float SpreadRange => spreadRange;

    [SerializeField] private float spreadDistanceFactor = 0.1f;
    public float SpreadDistanceFactor => spreadDistanceFactor;

    [SerializeField] private int energyUsage;
    public int EnergyUsage => energyUsage;

    [SerializeField] private int energyAmountToFire;
    public int EnergyAmountToFire => energyAmountToFire;

    [SerializeField] private int energyRegenPerSecond;
    public int EnergyRegenPerSecond => energyRegenPerSecond;

    [SerializeField] private AudioClip launchSFX;
    public AudioClip LaunchSFX => launchSFX;
    

} 

