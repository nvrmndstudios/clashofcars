using UnityEngine;

public class EnemyCarFollow : MonoBehaviour
{
    public Transform target;

    [Header("Missile Movement")]
    public float speed = 8f;                   // How fast the missile moves
    public float turnSpeed = 1.5f;             // How tightly it can curve (LOW = slower)

    [Header("Lifetime")]
    public float lifetime = 6f;                // Missile explodes if it doesn't hit in time

    private float lifeTimer = 0f;
    private Vector3 currentDirection;

    void Start()
    {
        // Missile flies straight initially
        currentDirection = transform.forward;
    }

    void Update()
    {
        if (!target) return;

        // Lifetime tracking
        lifeTimer += Time.deltaTime;
        if (lifeTimer >= lifetime)
        {
            Destroy(gameObject); // Self-destruct
            return;
        }

        // Desired direction to player (XZ only)
        Vector3 toTarget = target.position - transform.position;
        toTarget.y = 0;

        if (toTarget.magnitude > 0.1f)
        {
            Vector3 desiredDirection = toTarget.normalized;

            // Smooth turning — prevents sharp instant changes
            currentDirection = Vector3.Lerp(currentDirection, desiredDirection, Time.deltaTime * turnSpeed);

            // Rotate toward current direction
            Quaternion targetRotation = Quaternion.LookRotation(currentDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
        }

        // Move forward constantly
        transform.position += transform.forward * (speed * Time.deltaTime);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void OnDie(bool canScore)
    {
        if(canScore)
           GameManager.Instance.OnEnemyKill(gameObject);

        if (gameObject.TryGetComponent(out BlastEffect blastEffect))
        {
            blastEffect.Blast();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Collides with another enemy car
        if (other.TryGetComponent<EnemyCarFollow>(out var enemy) && other.gameObject != gameObject)
        {
            // Prevent double-processing: only one object handles the collision
            if (GetInstanceID() < other.GetInstanceID())
            {
                enemy.OnDie(true);
                OnDie(true);
                Destroy(other.gameObject);
                Destroy(gameObject);
            }
            return;
        }

        if (other.TryGetComponent<Barrel>(out var barrel))
        {
            OnDie(true);
            barrel.Blast();
        }

        // Collides with player
        if (other.TryGetComponent<CarController>(out var carController))
        {
            OnDie(false);
            carController.OnDie();
        }
    }
}