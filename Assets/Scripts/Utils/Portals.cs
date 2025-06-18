using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public string portalId;              // ID pour repérer les paires
    public bool isEntrance = true;       // Ce portail est une entrée ?
    public float requiredSpeed = 25f;    // Vitesse minimum requise
    public Color activeColor = Color.green;
    public Color inactiveColor = Color.red;

    private static Dictionary<string, Transform> exitPortals = new();

    private void Start()
    {
        if (!isEntrance)
        {
            if (!exitPortals.ContainsKey(portalId))
            {
                exitPortals.Add(portalId, transform);
            }
            else
            {
                Debug.LogWarning($"🔁 Deux portails de sortie avec le même ID '{portalId}' détectés !");
            }
        }

        UpdatePortalColor(false); // Par défaut : rouge
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isEntrance) return;

        var player = other.GetComponent<PlayerMovement>();
        if (player == null) return;

        if (player.currentSpeed >= requiredSpeed)
        {
            if (exitPortals.ContainsKey(portalId))
            {
                Transform target = exitPortals[portalId];
                player.transform.position = target.position;
                Debug.Log($"🌀 Portail {portalId} utilisé avec succès !");
            }
            else
            {
                Debug.LogWarning($"🚫 Portail de sortie non trouvé pour l'ID '{portalId}'");
            }
        }
        else
        {
            Debug.Log($"🚷 Vitesse insuffisante pour utiliser le portail {portalId} ({player.currentSpeed:F1} < {requiredSpeed})");
        }
    }

    private void Update()
    {
        if (!isEntrance) return;

        var player = FindFirstObjectByType<PlayerMovement>();
        if (player != null)
        {
            UpdatePortalColor(player.currentSpeed >= requiredSpeed);
        }
    }

    private void UpdatePortalColor(bool active)
    {
        var renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = active ? activeColor : inactiveColor;
        }
    }
}
