using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public static event Action OnBallCollisionWithCylinder;

    public static event Action OnBallCollisionWithSol;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Cylindre"))
        {
            OnBallCollisionWithCylinder?.Invoke(); // Déclenche l'événement
        }
        if (other.gameObject.CompareTag("Sol"))
        {
            OnBallCollisionWithSol?.Invoke(); // Déclenche l'événement

        }
    }
}
