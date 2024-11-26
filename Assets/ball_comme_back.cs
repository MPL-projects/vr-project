using System.Collections;
using UnityEngine;

public class BallComeBack : MonoBehaviour
{
    // Position initiale de la balle
    private Vector3 initialPosition;

    // Temps depuis que la balle a quitté sa position initiale
    private float timeOutOfInitialPosition = 0f;

    // Temps maximum hors de la position initiale avant de réinitialiser (en secondes)
    public float resetTime = 10f;

    // Plan de référence (ou objet à surveiller pour détection)
    public GameObject sol;

    // Limites de distance à partir du plan
    public float maxDistance = 5f;

    void Start()
    {
        // Enregistrer la position initiale de la balle
        initialPosition = transform.position;
    }

    void Update()
    {
        // Vérifier si la balle est hors du plan
        if (sol != null)
        {
            float distanceFromPlane = Vector3.Distance(transform.position, sol.transform.position);

            // Si la balle dépasse la limite, réinitialiser immédiatement
            if (distanceFromPlane > maxDistance)
            {
                ResetPosition();
            }
        }

        // Si la balle s'est éloignée de sa position initiale
        if (transform.position != initialPosition)
        {
            timeOutOfInitialPosition += Time.deltaTime;

            // Réinitialiser si le temps hors position dépasse la limite
            if (timeOutOfInitialPosition >= resetTime)
            {
                ResetPosition();
            }
        }
        else
        {
            // Réinitialiser le compteur si la balle revient à sa position initiale
            timeOutOfInitialPosition = 0f;
        }
    }

    // Méthode pour réinitialiser la position de la balle
    private void ResetPosition()
    {
        transform.position = initialPosition;
        timeOutOfInitialPosition = 0f;

        // Optionnel : Réinitialiser la vélocité si un Rigidbody est utilisé
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
