using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VehicleType
{
    Base,
    Hover,
    Shooter
}

public abstract class Vehicle : Destructible
{
    [SerializeField] private VehicleType vehicleType;
    [SerializeField] protected float maxLinearVelocity;
    [SerializeField] protected GameObject[] lights;
    [SerializeField] private ParticleSystem explosion;
    [SerializeField] private bool isLongExit;

    public VehicleType VehicleType => vehicleType;
    public bool IsLongExit => isLongExit;

    public virtual float LinearVelocity => 0;

    public float NormalizedLinearVelocity
    {
        get
        {
            if (Mathf.Approximately(0, LinearVelocity)) return 0;
            else return Mathf.Clamp01(LinearVelocity / maxLinearVelocity);
        }
    }

    protected Vector3 TargetInputControl;

    public void SetTargetControl(Vector3 control) => TargetInputControl = control.normalized;

    public void SetEngine(bool active)
    {
        engineSFX.enabled = active;
        if (active == true)
        {
            engineSFX.PlayOneShot(engineStartSFX);
        }
        foreach (var light in lights)
        {
            light.SetActive(active);
        }
    }

    [Header("SFX")]
    [SerializeField] private AudioSource engineSFX;
    [SerializeField] private AudioClip engineStartSFX;
    [SerializeField] private AudioClip engineWorkingSFX;
    [SerializeField] private float engineSFXModifier;

    private void UpdateEngineSFX()
    {
        if (engineSFX != null)
        {
            if (engineSFX.clip != engineWorkingSFX) engineSFX.clip = engineWorkingSFX;
            engineSFX.pitch = 1.0f + engineSFXModifier * NormalizedLinearVelocity;
            engineSFX.volume = 0.5f + NormalizedLinearVelocity;
        }
    }

    protected override void OnDeath()
    {
        explosion.gameObject.SetActive(true);
        SoundManager.Instanse.PlayOneShot(SoundManager.Instanse.CarExplosion);
        GetComponent<BoxCollider>().enabled = false;
    }

    protected virtual void Update() => UpdateEngineSFX();

    public abstract void Freeze();
    public abstract void Unfreeze();
}
