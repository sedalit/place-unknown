using UnityEngine;
using DG.Tweening;

public class JetPack : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private CharacterMovement characterMovement;
    [SerializeField] private float hoverForce;
    [SerializeField] private float maxFuel;
    [SerializeField] private float fuelRegen;
    [SerializeField] private GameObject[] flameThrows;

    private bool isJet;
    private float currentFuel;
    private AudioSource audioSource;

    public float MaxFuel => maxFuel;
    public float CurrentFuel => currentFuel;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        currentFuel = maxFuel;
    }

    private void Update()
    {
        if (isJet)
        {
            if (audioSource.isPlaying == false) audioSource.Play();
        }
        if (currentFuel != maxFuel && isJet == false)
        {
            currentFuel += fuelRegen * Time.deltaTime;
            currentFuel = Mathf.Clamp(currentFuel, 0, maxFuel);
        }
    }

    public void Jet()
    {
        if (currentFuel > 0)
        {
            isJet = true;
            characterMovement.directionControl = new Vector3(0, hoverForce, hoverForce / 2);
            currentFuel -= 5 * Time.deltaTime;
            SetFlameThrows(true);
        }
        else
        {
            Stop();
        }
    }

    public void Stop()
    {
        isJet = false;
        SetFlameThrows(false);
        audioSource.Stop();
    }

    private void SetFlameThrows(bool enabled)
    {
        foreach (var flame in flameThrows)
        {
            if (flame.activeInHierarchy != enabled) flame.SetActive(enabled);
        }
    }
}
