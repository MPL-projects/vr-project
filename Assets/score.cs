using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Nécessaire pour manipuler du texte UI

public class Score : MonoBehaviour
{
    private int scoreValue = 0; // Le score actuel
    private Text scoreText; // Référence au composant Text de l'UI

    // Start est appelé avant le premier frame
    void Start()
    {
        // Trouve automatiquement l'objet avec le nom "ScoreText"
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();

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
