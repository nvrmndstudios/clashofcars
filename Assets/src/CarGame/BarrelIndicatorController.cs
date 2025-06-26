using UnityEngine;

public class BarrelIndicatorController : MonoBehaviour
{
    public RectTransform markerUI;
    private Camera worldCamera;

    private Transform target;
    private Barrel barrel;

    public void SetTarget(Barrel barrelTarget, Camera wc)
    {
        barrel = barrelTarget;
        target = barrelTarget.transform;
        worldCamera = wc;
        barrel.OnBarrelBlasted += HandleBarrelDestroyed;
    }

    private void OnDestroy()
    {
        if (barrel != null)
            barrel.OnBarrelBlasted -= HandleBarrelDestroyed;
    }

    void HandleBarrelDestroyed(GameObject b)
    {
        Destroy(markerUI.gameObject); // Remove UI
        Destroy(this.gameObject);     // Optionally destroy controller if separate
    }

    void Update()
    {
        if (!markerUI) return;
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 screenPos = worldCamera.WorldToScreenPoint(target.position);

        if (screenPos.z < 0)
        {
            markerUI.gameObject.SetActive(false);
            return;
        }

        markerUI.gameObject.SetActive(true);

        Vector2 clampedPos = screenPos;
        clampedPos.x = Mathf.Clamp(clampedPos.x, 50f, Screen.width - 50f);
        clampedPos.y = Mathf.Clamp(clampedPos.y, 50f, Screen.height - 50f);

        markerUI.position = clampedPos;
    }
}