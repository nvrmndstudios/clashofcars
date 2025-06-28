using System;
using UnityEngine;

public class CarController : MonoBehaviour, IBlastable
{
    [Header("Movement Settings")]
    public float forwardSpeed = 10f;            // Constant forward speed
    public float turnSpeed = 5f;                // How fast the object rotates
    public float steerResponsiveness = 2f;      // How fast direction updates (higher = snappier)

    private Vector3 targetDirection;            // The desired direction to steer toward
    private Camera mainCamera;

    void Start()
    {
        Application.targetFrameRate = 30;
        mainCamera = Camera.main;
        targetDirection = transform.forward;    // Start with initial forward direction
    }

    void Update()
    {
        HandleInput();
        MoveForward();
    }

    void HandleInput()
    {
        Vector3 inputWorldPos = Vector3.zero;
        bool hasInput = false;

#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetMouseButton(0))
        {
            inputWorldPos = Input.mousePosition;
            hasInput = true;
        }
#else
        if (Input.touchCount > 0)
        {
            inputWorldPos = Input.GetTouch(0).position;
            hasInput = true;
        }
#endif

        if (hasInput)
        {
            Ray ray = mainCamera.ScreenPointToRay(inputWorldPos);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 dir = (hit.point - transform.position);
                dir.y = 0; // lock to XZ plane

                // Smoothly update target direction (delayed response)
                targetDirection = Vector3.Lerp(targetDirection, dir.normalized, Time.deltaTime * steerResponsiveness);
            }
        }
    }

    void MoveForward()
    {
        // Smoothly rotate toward the target direction
        if (targetDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }

        // Move forward along the current forward vector
        transform.position += transform.forward * (forwardSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Car Controller -  OnTriggerEnter ");
        
        if (other.gameObject.TryGetComponent(out Wall wall))
        {
            OnDie();
        }
        if (other.gameObject.TryGetComponent(out Barrel barrel))
        {
            OnDie();
            barrel.Blast();
        }
    }

    public void OnDie()
    {
        OnBlast();
        GameManager.Instance.OnPlayerDie();
    }

    public bool IsBlasted { get; private set; }
    public void Blast()
    {
        IsBlasted = true;
    }

    public void OnBlast()
    {
        if (TryGetComponent(out BlastEffect blastEffect))
        {
            blastEffect.Blast();
        }
    }
}