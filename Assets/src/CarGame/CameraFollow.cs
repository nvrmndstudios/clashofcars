using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;           // Missile or player object
    public Vector3 offset = new Vector3(0, 20f, -10f); // Height and distance behind
    public float followSpeed = 5f;     // Smooth follow speed

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (!target) return;

        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, 1f / followSpeed);
    }
}