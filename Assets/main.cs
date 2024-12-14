using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;
using Oculus.Interaction;
using System.Collections;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System;


public class GameController : MonoBehaviour
{
    public bool end = false;
    public bool isGameRunning = false; // Indique si le jeu est en cours
    public int scoreValue = 0; // Le score actuel
    public int throwCount = 0; // Compteur de lancers
    public int maxThrows = 10; // Nombre maximum de lancers

    private GameObject corbeille;
    private GameObject cylinder;
    private GameObject ball;

    public GameObject imageObject;  // Référence à l'objet Image dans l'UI
    private Image imageComponent;   // Composant Image de l'objet

    Sprite checkSprite;  // Sprite pour l'état "marqué"
    Sprite crossSprite;  // Sprite pour l'état "non marqué"
    Sprite emptySprite;  // Sprite pour l'état "vide"

    private Vector3 initialBallPosition; // Position initiale de la balle
    private float timeOutOfInitialPosition = 0f; // Temps écoulé depuis que la balle a quitté sa position initiale
    public float resetTime = 10f; // Temps maximum avant réinitialisation
    public GameObject sol; // Plan de référence
    public float maxDistance = 5f; // Distance maximale autorisée depuis le plan


    private bool isScored = false;
    private bool isGrab = false;

    private int test_throw = 0;
    private int test_score = 0;

    // Variables pour gérer le client TCP
    private TcpClient client;
    private NetworkStream stream;
    private const string serverIp = "192.168.0.167"; 
    private const int serverPort = 5000; // Port du serveur
    private Thread clientThread;

    // Variable pour stocker la distance du premier rebond
    private double dist = 0;
    private float x_ball;
    private float z_ball;
    private bool first_rebound = true;
    private float x_cylindre;
    private float z_cylindre;

    Collider ballCollider;

    void Start()
    {
        // Initialisation des références
        end = false;
        isGameRunning = false; 

        corbeille = GameObject.Find("Corbeille");
        cylinder = GameObject.Find("Corbeille/Cylinder");
        ball = GameObject.Find("Ball");
        sol = GameObject.Find("Sol");

        ballCollider = ball.GetComponent<Collider>();

        // Charger les sprites à partir des ressources
        checkSprite = Resources.Load<Sprite>("Images/check");  
        crossSprite = Resources.Load<Sprite>("Images/croix");
        emptySprite = Resources.Load<Sprite>("Images/vide");

        if (corbeille == null || cylinder == null || ball == null)
        {
            Debug.LogError("Un ou plusieurs objets nécessaires sont manquants !");
            return;
        }

        initialBallPosition = ball.transform.position;

        //Point Cylindre 
        x_cylindre = cylinder.transform.position.x;
        z_cylindre = cylinder.transform.position.z;

        

    }


    void Update()
    {
        if (!isGameRunning || throwCount > maxThrows)
        {
            return;
        }


        if (throwCount > maxThrows)
        {
            Debug.Log("Nombre maximum de lancers atteint !");
            return; // Arrête les interactions si le nombre maximum de lancers est atteint
        }

        // Mettre à jour l'état de grab
        IsGrab();

        // Vérifier la collision et la réinitialisation
        CheckBallReset();

    }

    public void StartGame()
    {
        // Nettoyer la connexion existante
        if (stream != null)
        {
            stream.Close();
            stream = null;
        }

        if (client != null)
        {
            client.Close();
            client = null;
        }

        // Réinitialise les variables du jeu
        isGameRunning = true;
        end = false;
        throwCount = 0;
        scoreValue = 0;
        test_throw = 0;
        test_score = 0;

        ResetImage();

        // Connexion au serveur
        ConnectToServer();

        // Réinitialiser la position de la balle
        if (ball != null)
        {
            ball.transform.position = initialBallPosition;

            Rigidbody rb = ball.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }

        Debug.Log("Le jeu a commencé !");
    }


    void OnEnable()
    {
        Ball.OnBallCollisionWithSol += HandleBallCollisionWithSol;
        Ball.OnBallCollisionWithCylinder += HandleBallCollisionWithCylinder;
        
    }

    void OnDisable()
    {
        Ball.OnBallCollisionWithSol -= HandleBallCollisionWithSol;
        Ball.OnBallCollisionWithCylinder -= HandleBallCollisionWithCylinder;
        
    }

    void HandleBallCollisionWithCylinder()
    {
        Debug.Log("Collision détectée via événement !");
        if (isGrab && !isScored)
        {
            isScored = true;
            StartCoroutine(ChangeColorAfterDelay());
        }
        dist = 0.0;
    }

    void HandleBallCollisionWithSol()
    {
        if(first_rebound)
        {
            //Point Ball
            x_ball = ball.transform.position.x;
            z_ball = ball.transform.position.z;

            dist = Math.Sqrt(
                    Math.Pow(x_ball - x_cylindre, 2) +
                    Math.Pow(z_ball - z_cylindre, 2)
                            );
            
            first_rebound = false;
        }
        
 
    }



    // Vérifie la distance et réinitialise la balle si nécessaire
    void CheckBallReset()
    {
        if (sol != null)
        {
            float distanceFromPlane = Vector3.Distance(ball.transform.position, sol.transform.position);

            if (distanceFromPlane > maxDistance) // Réinitialise immédiatement si la balle dépasse la distance maximale
            {
                ResetBallPosition();
                return;
            }
        }
        if (isScored)
        {
            ResetBallPosition();
            return;
        }

        // Réinitialiser si nécessaire
        if (ball.transform.position != initialBallPosition)
        {
            timeOutOfInitialPosition += Time.deltaTime;

            if (timeOutOfInitialPosition >= resetTime && ball.transform.position.y < 0.1)
            {
                ResetBallPosition();
            }
        }
    }

    // Réinitialise la balle et les compteurs
    void ResetBallPosition()
    {
        ball.transform.position = initialBallPosition;
        timeOutOfInitialPosition = 0f;

        Rigidbody rb = ball.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        if (throwCount <= maxThrows)
        {
            if(isGrab)
            {
                throwCount++;
                UpdateImage();
                SendDataToServer(); // Envoie les données après un lancer

            }

            // Réinitialiser l'état de grab si nécessaire
            isScored = false;
            isGrab = false;
            first_rebound = true;
            
            if(throwCount == maxThrows)
            {
                // isRunning = false;
                if (stream != null) stream.Close();
                if (client != null) client.Close();
                end = true;
            }
        }
    }

    void UpdateImage(){

        // Trouver l'objet Image dynamique dans le Canvas et assigner le sprite en fonction de isScored
        imageObject = GameObject.Find("ScoreCanvas/Image" + throwCount.ToString());
        imageComponent = imageObject.GetComponent<Image>();

        if (imageObject != null)
        {
            // Mettre à jour le sprite en fonction de l'état isScored
            if(isScored)
            {
                // test_score = true;
                Debug.Log("OUIIIIIIIIIIIIIIIIIIIIIIIII");
                scoreValue++;
                imageComponent.sprite = checkSprite;
            }
            else{
                // test_score = false;
                Debug.Log("CROSSSSSSSSSSSSSSSSSSSSSSSss");
                imageComponent.sprite = crossSprite;
            }
        }
        
    }

    void ResetImage(){

        // Trouver l'objet Image dynamique dans le Canvas et assigner le sprite en fonction de isScored
        for(int i = 1 ; i < (maxThrows+1); i++)
        {
            imageObject = GameObject.Find("ScoreCanvas/Image" + i.ToString());
            imageComponent = imageObject.GetComponent<Image>();
            imageComponent.sprite = emptySprite;
        }
        
    }

    void IsGrab()
    {
        if(ball.transform.position.y > 0.9) // Si la balle est au dessu de 50 cm alors elle est grab
        {
            isGrab = true;  
        }
    }

    private void ConnectToServer()
    {
       
        if (client != null && client.Connected)
        {
            Debug.Log("Client déjà connecté.");
            return;
        }

        client = new TcpClient(serverIp, serverPort); // Connexion au serveur
        stream = client.GetStream();
        Debug.Log("Connexion au serveur réussie.");
    
    }


    private void SendDataToServer()
    {
        
        if (stream == null || !client.Connected)
        {
            Debug.LogError("Aucune connexion active. Les données ne peuvent pas être envoyées.");
            return;
        }

        if (throwCount <= maxThrows && throwCount == (test_throw + 1))
        {
            string message = $"Essais: {throwCount}, Restant: {maxThrows - throwCount}, Score: {scoreValue}, " +
                                $"Resultat: {(test_score < scoreValue ? "Reussi" : "Echoue")}, " +
                                $"Distance du rebond: {dist}";
            byte[] data = Encoding.ASCII.GetBytes(message);
            stream.Write(data, 0, data.Length);

            Debug.Log("Message envoyé : " + message);

            test_throw++;
            if (test_score < scoreValue)
            {
                test_score++;
            }
        }
        else
        {
            Debug.Log("Conditions non remplies pour envoyer les données.");
        }
        
        
    }

    IEnumerator ChangeColorAfterDelay()
    {
        corbeille.GetComponent<MeshRenderer>().material.color = new Color(0f, 1f, 0f, 1f); // Change en vert
        yield return new WaitForSeconds(3f); // Attend 3 secondes
        corbeille.GetComponent<MeshRenderer>().material.color = new Color(1f, 0.5f, 0f, 1f); // Retourne à orange
    }

    void OnApplicationQuit()
    {
        if (stream != null)
        {
            stream.Close();
            stream = null;
        }

        if (client != null)
        {
            client.Close();
            client = null;
        }

        Debug.Log("Application quittée. Connexion fermée.");
    }


}
