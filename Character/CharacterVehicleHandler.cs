using UnityEngine;
using UnityEngine.Events;

public class CharacterVehicleHandler : MonoBehaviour
{
    public event UnityAction<Vehicle> VehicleChanged;

    private Vehicle currentVehicle;

    public Vehicle CurrentVehicle => currentVehicle;

    public void SetVehicle(Vehicle vehicle)
    {
        currentVehicle = vehicle;
        VehicleChanged?.Invoke(vehicle);
    }
}
