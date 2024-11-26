using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Nécessaire pour manipuler du texte UI

public class Score : MonoBehaviour
{
    private bool isScored;
    private GameObject corbeille;
    private GameObject cylinder;
    private GameObject ball;
    private int scoreValue = 0; // Le score actuel
    private Text scoreText; // Référence au composant Text de l'UI

    // Start est appelé avant le premier frame
    void Start()
    {
        // Trouve automatiquement l'objet avec le nom "ScoreText"
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();

        corbeille = GameObject.Find("Corbeille");
        cylinder = GameObject.Find("Cylinder");
        ball = GameObject.Find("[BuildingBlock] Grabbable sphere");

        if (corbeille == null){ Debug.LogError("Corbeille non trouvée dans la scène"); }
        if (cylinder == null){ Debug.LogError("Cylindre non trouvée dans la scène"); }
        if (ball == null){ Debug.LogError("Balle non trouvée dans la scène"); }
        if (scoreText == null)
        {
            Debug.LogError("Aucun objet nommé 'ScoreText' trouvé ou le composant Text n'est pas attaché !");
        }
        else
        {
            UpdateScoreText(); // Initialise l'affichage du score
        }
    }

    // Méthode pour mettre à jour le texte du score
    void UpdateScoreText()
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
        scoreText.text = scoreValue.ToString();
    }

    // Méthode appelée lorsqu'une balle entre en collision avec le cylindre
    private void OnCollisionEnter(Collision collision)
    {
        // Vérifie si l'objet en collision a le tag "Ball"
        if (collision.gameObject.CompareTag("Ball")) // Assurez-vous que vos balles ont le tag "Ball"
        {
            scoreValue++; // Augmente le score
            UpdateScoreText(); // Met à jour le texte du score
        }
    }
}
