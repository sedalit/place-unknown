using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponMode mode;
    [SerializeField] private WeaponProperties properties;
    [SerializeField] private ParticleSystem[] muzzleParticles;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float maxEnergy;

    public bool CanFire => refireTimer <= 0 && isEnergyRestored == false;
    public float MaxEnergy => maxEnergy;
    public float CurrentEnergy => currentEnergy;

    private float refireTimer;
    private float currentEnergy;
    private bool isEnergyRestored;
    private NoiseAudioSource audioSource;
 
    private void Start() 
    {
        audioSource = GetComponent<NoiseAudioSource>();
        currentEnergy = maxEnergy;
    }
    protected virtual void Update() 
    {
        if (refireTimer > 0)
        refireTimer -= Time.deltaTime;
        UpdateEnergy();
    }
    private void UpdateEnergy()
    {
        currentEnergy += (float)properties.EnergyRegenPerSecond * Time.deltaTime;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
        if (currentEnergy >= properties.EnergyAmountToFire)
        {
            isEnergyRestored = false;
        }
    }
    private bool TryDrawEnergy(int count)
    {
        if (count == 0) return true;
        if (currentEnergy >= count)
        {
            currentEnergy -= count;
            return true;
        }
        return false;
    }

    public void Fire()
    {
        if (properties == null) return;
        if (refireTimer > 0) return;
        if (TryDrawEnergy(properties.EnergyUsage) != true) return;
        TryDrawEnergy(properties.EnergyUsage);

        Projectile projectile = Instantiate(properties.ProjectilePrefab).GetComponent<Projectile>();
        projectile.SetParentShooter(transform.root.GetComponent<Destructible>());
        projectile.transform.position = firePoint.position;
        projectile.transform.forward = firePoint.forward;

        audioSource.Clip = properties.LaunchSFX;
        audioSource.Play();

        refireTimer = properties.RateOfFire;
        foreach (var particle in muzzleParticles)
        {
            particle.time = 0;
            particle.Play();
        }
    }
    public void FirePointLookAt(Vector3 pos)
    {
        Vector3 offset = Random.insideUnitSphere * properties.SpreadRange;
        if (properties.SpreadDistanceFactor != 0)
        {
            offset = offset * (Vector3.Distance(firePoint.position, pos) / 2) * properties.SpreadDistanceFactor;
        }
        firePoint.LookAt(pos + offset);
    }
    public void AssignLoadout(WeaponProperties props)
    {
        if(mode != props.Mode) return;
        refireTimer = 0;
        properties = props;
    }
}
