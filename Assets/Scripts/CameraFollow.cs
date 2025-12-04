using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform iconTarget;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    private Vector3 velocity = Vector3.zero;
    [SerializeField] [Range(0.1f, 1f)] private float smoothSpeed = 0.125f;

    private void Update()
    {
        Vector3 desiredPosition = target.position + offset;
        desiredPosition.z = -10;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);

        if (iconTarget != null)
        {
            Vector3 iconDesiredPosition = iconTarget.position + offset;
            iconDesiredPosition.y -= 0.5f;
            iconDesiredPosition.z = -10;
            iconTarget.position = Vector3.SmoothDamp(iconTarget.position, iconDesiredPosition, ref velocity, smoothSpeed);
            // Optionally, you can have a separate smoothing for the icon if needed
        }
    }
}
