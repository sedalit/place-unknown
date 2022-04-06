using UnityEngine;
using UnityEngine.UI;

public class UIWeaponEnergy : MonoBehaviour
{
    [SerializeField] private Weapon targetWeapon;
    [SerializeField] private CharacterMovement characterMovement;
    [SerializeField] private Slider slider;
    [SerializeField] private Image[] images;

    private void Start() => slider.maxValue = targetWeapon.MaxEnergy;

    private void Update()
    {
        if (targetWeapon == null) targetWeapon = Player.Instance.transform.GetComponent<Shooter>().Weapon;
        if (characterMovement == null) characterMovement = Player.Instance.transform.GetComponent<CharacterMovement>();
        slider.value = targetWeapon.CurrentEnergy;
        SetActiveImages(characterMovement.IsAiming);
    }

    private void SetActiveImages(bool active)
    {
        for (int i = 0; i < images.Length; i++)
        {
            images[i].enabled = active;
        }
    }
}
