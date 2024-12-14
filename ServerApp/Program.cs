using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ServerApp
{
    public class Server
    {
        private static TcpListener tcpListener = null!; // Déclaration de l'écouteur de TCP
        private static bool isRunning = true;
        private static int serverPort = 5000; // Port sur lequel écouter
        private static string filePath = "messages.txt"; // Chemin du fichier texte
        private static StreamWriter writer;
        

        public static void Main(string[] args)
        {
            StartServer();
        }

        private static void StartServer()
        {
            try
            {
                // Réinitialise le fichier à chaque lancement du serveur
                ResetServerFile();

                // Créer ou ouvrir le fichier de messages pour l'écriture
                writer = new StreamWriter(filePath, append: true);
                Console.WriteLine($"Serveur démarré sur le port {serverPort}...");

                tcpListener = new TcpListener(IPAddress.Any, serverPort); // Écoute sur toutes les interfaces réseau
                tcpListener.Start();

                while (isRunning)
                {
                    // Attente d'une connexion du client
                    TcpClient client = tcpListener.AcceptTcpClient();
                    Console.WriteLine("Client connecté !");
                    NetworkStream stream = client.GetStream();

                    // Lecture des données envoyées par le client
                    byte[] buffer = new byte[1024];
                    int bytesRead;

                    while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                        Console.WriteLine($"Message reçu : {message}");

                        // Écrire le message dans le fichier texte
                        writer.WriteLine($"[{DateTime.Now}] {message}");  // Ajoute un timestamp avant chaque message
                        writer.Flush();  // Force l'écriture immédiate
                    }

                    // Ferme la connexion client
                    client.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur serveur : " + ex.Message);
            }
            finally
            {
                // Fermer le fichier en cas de sortie du serveur
                writer?.Close();
            }
        }

        // Réinitialise le fichier avant chaque démarrage du serveur
        private static void ResetServerFile()
        {
            // Supprimer le fichier s'il existe déjà
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public static void StopServer()
        {
            isRunning = false;
            tcpListener.Stop();
            Console.WriteLine("Serveur arrêté.");
        }
    }
}
