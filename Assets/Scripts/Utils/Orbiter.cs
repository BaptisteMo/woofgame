using UnityEngine;

public class Orbiter : MonoBehaviour
{
    public Transform centerObject;   // L'objet autour duquel orbiter
    public float orbitRadius = 40f;   // Rayon de l'orbite
    public float orbitSpeed = 30f;   // Degrés par seconde

    private float angle = 0f;

    void Update()
    {
        if (centerObject == null) return;

        angle += orbitSpeed * Time.deltaTime;
        float radians = angle * Mathf.Deg2Rad;

        // Calcule la position en cercle dans le plan horizontal (XZ)
        float x = Mathf.Cos(radians) * orbitRadius;
        float z = Mathf.Sin(radians) * orbitRadius;

        // On garde la hauteur Y actuelle
        Vector3 offset = new Vector3(x, 0f, z);
        transform.position = centerObject.position + offset;
    }
}