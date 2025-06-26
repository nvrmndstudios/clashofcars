using System.Collections;
using UnityEngine;

public class BlastEffect : MonoBehaviour
{
    [Header("Explosion Settings")]
    public float explosionForce = 700f;
    public float explosionRadius = 5f;
    public float upwardsModifier = 0.4f;

    [Header("Visuals")]
    public GameObject blastParticle;

    [Header("Other Settings")]
    public bool addColliderIfMissing = true;

    [Header("Blast Chain Settings")]
    public bool searchNearbyBlasts = false;
    public float blastChainRadius = 8f;

    private bool hasBlasted = false;

    public void SetValuesManually(GameObject blastPart,  float exploForce = 10f, float exploRad = 1f, float upwardModifier = 0.1f, bool searchForNear = false, bool addColIfMssng = true)
    { 
        blastParticle = blastPart; 
        explosionForce = exploForce; 
        explosionRadius = exploRad; 
        upwardsModifier = upwardModifier; 
        searchNearbyBlasts = searchForNear; 
        addColliderIfMissing = addColIfMssng;
    }

    public void Blast()
    {
        if (hasBlasted) return;
        hasBlasted = true;

        Vector3 blastPosition = transform.position;

        if (blastParticle)
            Instantiate(blastParticle, blastPosition, Quaternion.identity);

        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();

        foreach (var renderer in meshRenderers)
        {
            GameObject part = renderer.gameObject;

            // Add Rigidbody if missing
            Rigidbody rb = part.GetComponent<Rigidbody>();
            if (rb == null)
                rb = part.AddComponent<Rigidbody>();

            // Add Collider if missing
            if (addColliderIfMissing && part.GetComponent<Collider>() == null)
            {
                MeshCollider collider = part.AddComponent<MeshCollider>();
                collider.convex = true;
            }

            // Detach
            part.transform.SetParent(null);

            // Apply force
            rb.AddExplosionForce(explosionForce, blastPosition, explosionRadius, upwardsModifier, ForceMode.Impulse);

            // Add sink logic
            rb.gameObject.AddComponent<SinkingPart>().StartSinkProcess();
        }

        // Chain Blast Logic
        if (searchNearbyBlasts)
        {
            Collider[] hits = Physics.OverlapSphere(blastPosition, blastChainRadius);
            foreach (var hit in hits)
            {
                IBlastable nearby = hit.GetComponent<IBlastable>();
                if (nearby != null && !nearby.IsBlasted)
                {
                    nearby.Blast();
                }
            }
        }
        // Destroy shell object
        Destroy(gameObject);
    }

}