using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] private Weapon currentWeapon;
    [SerializeField] private Camera camera;
    [SerializeField] private Transform aim;

    public Camera Camera => camera;
    public Weapon Weapon => currentWeapon;

    public void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(aim.position, aim.forward, out hit, 10000))
        {
            currentWeapon.FirePointLookAt(hit.point);
        }
        currentWeapon.Fire();
    }
}
