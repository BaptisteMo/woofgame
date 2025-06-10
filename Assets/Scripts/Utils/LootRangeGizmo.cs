using UnityEngine;

public class LootRangeGizmo : MonoBehaviour
{
    [Header("Gizmo Settings")]
    public float lootRadius = 2f; // rayon autour du joueur pour looter
    public Color gizmoColor = new Color(1f, 0.85f, 0f, 0.3f); // jaune translucide

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawSphere(transform.position, lootRadius);
    }
}