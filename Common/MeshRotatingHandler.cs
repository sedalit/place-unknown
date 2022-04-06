using UnityEngine;

public class MeshRotatingHandler : MonoBehaviour
{
    [SerializeField] private CharacterMovement characterMovement;

    private void Update()
    {
        if (characterMovement.targetDirectionControl.normalized != Vector3.zero)
        {
            transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
    }
}
