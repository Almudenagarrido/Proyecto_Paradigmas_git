using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text pointsText; 
    public Image lifePlayer1;
    public Image lifePlayer2;
    public Image backgroundPlayer2;
    private GameManager gameManager;


    void Start()
    {
        // Intentar obtener la referencia al GameManager
        gameManager = GameManager.Instance;

        // Si no estamos en el menú, activamos el texto de puntos
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            pointsText.gameObject.SetActive(true);  // Aseguramos que se muestre el texto
        }
        else
        {
            pointsText.gameObject.SetActive(false);  // Ocultamos el texto en el menú
        }
    }

    void Update()
    {
        // Solo actualizamos los puntos si estamos jugando
        if (gameManager.isPlaying)
        {
            UpdatePoints();
            UpdateLife();
        }
    }

    void UpdatePoints()
    {
        int numPlayers = gameManager.GetNumberPlayers();
        if (numPlayers == 1)
        {
            int player1Points = gameManager.GetPoints()[1];
            pointsText.text = "Score: " + player1Points;
        }
        if (numPlayers == 2)
        {
            // Suponemos que tenemos dos jugadores. Si tienes más, tendrás que modificar esto.
            int player1Points = gameManager.GetPoints()[1];
            int player2Points = gameManager.GetPoints()[2];

            // Actualizamos el texto con los puntos de ambos jugadores
            pointsText.text = "Player 2 - " + player1Points + " | " + player2Points + " - Player 1";
        }
    }

    void UpdateLife()
    {
        int numPlayers = gameManager.GetNumberPlayers();

        if (numPlayers == 1)
        {
            int player1Life = gameManager.GetLife()[1];
            lifePlayer1.fillAmount = (float)player1Life / 100f;
            lifePlayer2.enabled = false;
            backgroundPlayer2.enabled = false;
        }

        if (numPlayers == 2)
        {
            //lifePlayer2.enabled = true;
            //backgroundPlayer2.enabled = true;

            // Suponemos que tenemos dos jugadores. Si tienes más, tendrás que modificar esto.
            int player1Life = gameManager.GetLife()[1];
            lifePlayer1.fillAmount = (float)player1Life / 100f;

            int player2Life = gameManager.GetLife()[2];
            lifePlayer2.fillAmount = (float)player2Life / 100f;



        }
    }


}
