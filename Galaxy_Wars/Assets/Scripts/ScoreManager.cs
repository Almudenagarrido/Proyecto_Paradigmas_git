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
        gameManager = GameManager.Instance;

        // Si no estamos en el menú, activamos el texto de puntos
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            pointsText.gameObject.SetActive(true);
        }
        else
        {
            pointsText.gameObject.SetActive(false);
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
        int totalPoints = gameManager.GetPoints();
        pointsText.text = "Score: " + totalPoints;
    }

    void UpdateLife()
    {
        int numPlayers = gameManager.numberOfPlayers;

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
