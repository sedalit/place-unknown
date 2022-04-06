using UnityEngine;

public class ConverterCameraRotation : MonoBehaviour
{
    [SerializeField] private Transform camera;
    [SerializeField] private Transform cameraLook;
    [SerializeField] private Vector3 lookOffset;

    [SerializeField] private float topAngleLimit;
    [SerializeField] private float bottomAngleLimit;

    void Update()
    {
        Vector3 angle = new Vector3(0, 0, 0);

        angle.z = camera.eulerAngles.x;

        if (angle.z >= topAngleLimit || angle.z <= bottomAngleLimit)
        {
            transform.LookAt(cameraLook.position + lookOffset);

            angle.x = transform.eulerAngles.x;
            angle.y = transform.eulerAngles.y;

            transform.eulerAngles = angle;
        }
    }
    
}
