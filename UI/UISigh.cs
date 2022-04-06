using UnityEngine;
using UnityEngine.UI;

public class UISigh : MonoBehaviour
{
    [SerializeField] private CharacterMovement characterMovement;
    [SerializeField] private CharacterVehicleHandler characterVehicleHandler;
    [SerializeField] private Image imageSigh;
    [SerializeField] private Sprite sighOnVehicle;
    [SerializeField] private Sprite sighOnFoot;

    public Image ImageSigh => imageSigh;

    private void OnEnable() => characterVehicleHandler.VehicleChanged += OnVehicleChanged;

    private void OnDisable() => characterVehicleHandler.VehicleChanged -= OnVehicleChanged;

    private void Update()
    {
        if (characterMovement == null) characterMovement = Player.Instance.transform.GetComponent<CharacterMovement>();
        if (characterVehicleHandler == null) characterVehicleHandler = Player.Instance.transform.GetComponent<CharacterVehicleHandler>();
        imageSigh.enabled = characterMovement.IsAiming || characterVehicleHandler.CurrentVehicle != null && characterVehicleHandler.CurrentVehicle.VehicleType == VehicleType.Shooter;
    }

    private void OnVehicleChanged(Vehicle vehicle)
    {
        if (vehicle == null)
        {
            imageSigh.sprite = sighOnFoot;
        }
        else if (vehicle.VehicleType == VehicleType.Shooter)
        {
            imageSigh.sprite = sighOnVehicle;
        }
    }
}
