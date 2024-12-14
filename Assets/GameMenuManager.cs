using UnityEngine;
using UnityEngine.UI; // Pour le bouton et le Dropdown
using TMPro; // Pour TMP_Dropdown
using Oculus.Interaction;
using Oculus.Interaction.Input;
using System.Text;

public class GameMenuManager : MonoBehaviour
{
    public GameObject menuCanva; // Drag & Drop votre menu ici dans l'inspecteur
    public Button closeButton; // Drag & Drop le bouton croix ici dans l'inspecteur
    public Button startButton; // Drag & Drop le bouton start ici dans l'inspecteur

    public TMP_Dropdown typeModeDropdown; // Drag & Drop le Dropdown ici dans l'inspecteur
    public GameObject avatarObject; // L'objet représentant l'avatar

    public GameObject menuFinCanva; // Drag & Drop votre menu ici dans l'inspecteur
    public Button finButton; // Drag & Drop le bouton croix ici dans l'inspecteur

    // Référence au script GameController
    public GameController gameController; // Référence au script GameController

    // private TMP_Text scoreText; 
    public TMP_Text scoreText;




    private bool boolFin = false;

    void Start()
    {
        // Si gameController n'est pas assigné dans l'inspecteur, essaye de le trouver automatiquement
        if (gameController == null)
        {
            gameController = FindObjectOfType<GameController>();
            if (gameController == null)
            {
                Debug.LogError("Le script GameController n'a pas été trouvé dans la scène !");
            }
        }

        // Vérifie si menuCanva est assigné
        if (menuCanva == null)
        {
            menuCanva = GameObject.Find("MenuCanva");

            if (menuCanva == null)
            {
                Debug.LogError("MenuCanva introuvable !");
                return;
            }
        }

        // Active le menu au démarrage
        menuCanva.SetActive(true);
        Debug.Log("MenuCanva activé.");

        //Deactive le menu de fin au démarrage
        menuFinCanva.SetActive(false);
        Debug.Log("MenuCanva activé.");

    }

   public void StartGame()
    {

        boolFin = false;
        if (menuCanva != null)
        {
            menuCanva.SetActive(false); // Cache le menu
            Debug.Log("MenuCanva fermé.");
        }

        

        if (gameController != null)
        {
            gameController.StartGame(); // Lance le jeu depuis GameController
            Debug.Log("Le jeu a été lancé !");
        }
        else
        {
            Debug.LogError("GameController est null !");
        }
    }

    // Méthode pour cacher le menu
    public void CloseMenu()
    {
        if (menuCanva != null)
        {
            menuCanva.SetActive(false);
            Debug.Log("MenuCanva fermé.");
        }
        else
        {
            Debug.LogError("MenuCanva est null !");
        }
    }

    

    // Méthode pour gérer la visibilité de l'avatar
    public void SetTypeMode(int index)
    {
        if (avatarObject != null)
        {
            if (index == 1) // Mode Avatar visible
            {
                avatarObject.SetActive(true);
                Debug.Log("Avatar visible.");
            }
            else if (index == 2) // Mode Avatar caché
            {
                avatarObject.SetActive(false);
                Debug.Log("Avatar caché.");
            }
            else
            {
                Debug.LogWarning("Option non reconnue dans le Dropdown.");
            }
        }
        else
        {
            Debug.LogError("AvatarObject est null !");
        }
    }

    // Méthode pour ouvrir le menu de fin
    public void OpenFinMenu()
    {
        if (menuFinCanva != null && gameController != null && gameController.end)
        {

            
            menuFinCanva.SetActive(true);
            Debug.Log("Menu de fin affiché.");
            boolFin = true;

            Debug.Log("Score: " + gameController.scoreValue);
            scoreText.text = "Score: " + gameController.scoreValue;
        }
        else
        {
            if (menuFinCanva == null)
                Debug.LogError("menuFinCanva est null !");
            if (gameController == null)
                Debug.LogError("gameController est null !");
        }
    }

    void Update()
    {
        if(!boolFin)
        {
            OpenFinMenu();
        }
    }

 

    public void MenuFin()
    {
        if (menuFinCanva != null)
        {
            //Deactive le menu de fin au démarrage
            menuFinCanva.SetActive(false);
            Debug.Log("menuFinCanva activé.");
        }
        else
        {
            Debug.LogError("menuFinCanva est null !");
        }

        if (menuCanva != null)
        {
            menuCanva.SetActive(true);
        }
        else
        {
            Debug.LogError("MenuCanva est null !");
        }
    }



}
