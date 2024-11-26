using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Nécessaire pour manipuler du texte UI
using TMPro;

public class Score : MonoBehaviour
{
    private bool isScored;
    private GameObject corbeille;
    private GameObject cylinder;
    private GameObject ball;
    private int scoreValue = 0; // Le score actuel
    private TMP_Text scoreText; // Référence au composant Text de l'UI

    // Start est appelé avant le premier frame
    void Start()
    {
        // Trouver "ScoreText" sous "Canvas > Panel"
        GameObject scoreTextObject = GameObject.Find("Canvas/Panel/ScoreText");
        if (scoreTextObject == null)
        {
            Debug.LogError("ScoreText introuvable dans la hiérarchie !");
            return;
        }

        // Trouver la composante de texte dans le game object
        scoreText = scoreTextObject.GetComponent<TMP_Text>();
        if (scoreText == null)
        {
            Debug.LogError("Composant TMP_Text manquant sur ScoreText !");
            return;
        }


        corbeille = GameObject.Find("Corbeille");
        if (corbeille == null){ 
            Debug.LogError("Corbeille non trouvée dans la scène");
            return;
        }


        cylinder = GameObject.Find("Cylinder");
        if (cylinder == null){ 
            Debug.LogError("Cylindre non trouvée dans la scène");
            return;
        }

        ball = GameObject.Find("Ball");
        if (ball == null){
            Debug.LogError("Balle non trouvée dans la scène");
            return;
        }

        UpdateScore();
    }

    // Méthode pour mettre à jour le texte du score
    void UpdateScore()
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

            // + 1
            scoreValue++;
            scoreText.text = scoreValue.ToString();
        }
    }
}
