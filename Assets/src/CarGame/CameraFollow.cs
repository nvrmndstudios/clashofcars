using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;           // Missile or player object
    public Vector3 offset = new Vector3(0, 20f, -10f); // Height and distance behind
    public float followSpeed = 5f;     // Smooth follow speed

    void LateUpdate()
    {
        if (!target) return;

        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
        //transform.LookAt(target); // Optional: make camera look at the missile
    }
}