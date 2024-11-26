using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    private bool isScored;
    private GameObject corbeille;
    private GameObject cylinder;
    private GameObject ball;

    // Start is called before the first frame update
    void Start()
    {
        isScored = false;

        corbeille = GameObject.Find("Corbeille");
        cylinder = GameObject.Find("Cylinder");
        ball = GameObject.Find("[BuildingBlock] Grabbable sphere");

        if (corbeille == null){ Debug.LogError("Corbeille non trouvée dans la scène"); }
        if (cylinder == null){ Debug.LogError("Cylindre non trouvée dans la scène"); }
        if (ball == null){ Debug.LogError("Balle non trouvée dans la scène"); }
    }

    // Update is called once per frame
    void Update()
    {
        // Vérifie si la distance entre la balle et le cylindre est suffisamment petite pour indiquer un contact
        float distance = Vector3.Distance(ball.transform.position, cylinder.transform.position);
        isScored = distance < 0.5f;

        if (isScored)
        {
            // Corbeille prends une couleur au hasard
            // corbeille.GetComponent<MeshRenderer>().material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
            
            // Corbeille verte
            corbeille.GetComponent<MeshRenderer>().material.color = new Color(0f, 1f, 0f, 1f);
        }
    }
}
