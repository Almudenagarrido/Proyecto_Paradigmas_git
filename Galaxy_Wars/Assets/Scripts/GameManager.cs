using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private Button playButton;
    private Button quitButton;
    private Button backButton = null;
    private TextMeshProUGUI score;

    public int selectedLevel = 0;
    public int numberOfPlayers = 0;
    public bool isSecondPlayerAI = false;
    public bool isPlaying = false;
    public bool endGame = false;
    private bool gameOverTriggered = false;

    public Dictionary<int, int> lifePlayers = new Dictionary<int, int> { { 1, 100 }, { 2, 100 } }; 
    public int totalPoints = 0;
    public List<GameObject> playerLifeBars = new List<GameObject>();
    public List<GameObject> playerFillBars = new List<GameObject>(); 
    private Dictionary<int, bool> shieldStatus = new Dictionary<int, bool> { { 1, false }, { 2, false } };

    private SpriteManager spriteManager;
    private LevelFactory levelFactory;

    private void Start()
    {
        playButton = GameObject.Find("PlayButton").GetComponent<Button>();
        quitButton = GameObject.Find("QuitButton").GetComponent<Button>();
       
        if (playButton != null)
        {
            playButton.onClick.AddListener(StartGame);
        }

        if (playButton != null && quitButton != null)
        {
            quitButton.onClick.AddListener(QuitGame);
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "GameOver")
        {
            CheckGameOver();
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeManagers();
            LoadMenu();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameOver")  // Si la escena cargada es GameOver
        {
            AssignBackButton();
            UpdateGameOverScore();
        }
        if (scene.name == "MainMenu")
        {
            AssignPlayExitButtons();
        }
    }

    private void AssignBackButton()
    {
        backButton = GameObject.Find("BackButton").GetComponent<Button>();
        Debug.Log(backButton);
        if (backButton != null)
        {
            backButton.onClick.RemoveAllListeners();  // Quitar los que haya y solo usar ese
            backButton.onClick.AddListener(EndGame);  // Para volver al men�
            Debug.Log("Bot�n BackButton asignado.");
        }
    }

    private void AssignPlayExitButtons()
    {
        playButton = GameObject.Find("PlayButton")?.GetComponent<Button>();
        quitButton = GameObject.Find("QuitButton")?.GetComponent<Button>();
        if (playButton != null)
        {
            playButton.onClick.RemoveAllListeners();
            playButton.onClick.AddListener(StartGame);
            quitButton.onClick.AddListener(QuitGame);
        }
    }

    private void UpdateGameOverScore()
    {
        if (score == null)
        {
            score = GameObject.Find("ScoreText")?.GetComponent<TextMeshProUGUI>();
        }

        score.text = $"Total Score: {totalPoints}";
    }

    private void InitializeManagers()
    {
        GameObject spriteManagerObject = GameObject.Find("SpriteManager");
        if (spriteManagerObject == null)
        {
            spriteManagerObject = new GameObject("SpriteManager");
            spriteManager = spriteManagerObject.AddComponent<SpriteManager>();
        }
        else
        {
            spriteManager = spriteManagerObject.GetComponent<SpriteManager>();
        }

        GameObject levelFactoryObject = GameObject.Find("LevelFactory");
        if (levelFactoryObject == null)
        {
            levelFactoryObject = new GameObject("LevelFactory");
            levelFactory = levelFactoryObject.AddComponent<LevelFactory>();
        }
        else
        {
            levelFactory = levelFactoryObject.GetComponent<LevelFactory>();
        }
    }

    public void LoadMenu()
    {
        Debug.Log("Cargando el men�...");
        ResetSelections();
        SceneManager.LoadScene("MainMenu");
    }

    public void StartGame()
    {
        if (selectedLevel != 0 && numberOfPlayers != 0)
        {
            isPlaying = true;
            Debug.Log($"Iniciando juego con nivel {selectedLevel} y {numberOfPlayers} jugadores...");
            
            SceneManager.LoadScene("BasicSceneLevels");
            Invoke(nameof(CreateLevel), 0.1f);
        }
        else
        {
            Debug.LogError("No se han seleccionado el nivel o el n�mero de jugadores. No se puede iniciar el juego.");
        }
    }

    public void CheckGameOver()
    {
        if (gameOverTriggered) return;

        bool allPlayersDead = false;

        if (numberOfPlayers == 1)
        {
            lifePlayers[2] = 0; // Si hay un jugador, establece la vida del segundo en 0.
        }

        if (lifePlayers[1] == 0 && lifePlayers[2] == 0)
        {
            allPlayersDead = true;
        }

        if (allPlayersDead)
        {
            Debug.Log("Todos los jugadores han muerto. Fin del juego.");
            gameOverTriggered = true;
            StartCoroutine(WaitChargeGameOver());

        }
    }
    
    private IEnumerator WaitChargeGameOver()
    {
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene("GameOver");
    }
    
    private void EndGame()
    {
        endGame = true;
        isPlaying = false;
        gameOverTriggered = false;
        LoadMenu();
    }

    private void CreateLevel()
    {
        if (levelFactory != null && selectedLevel != 0 && numberOfPlayers != 0)
        {
            levelFactory.CreateLevel(selectedLevel, numberOfPlayers);
        }
        else
        {
            Debug.LogError("El LevelFactory no est� inicializado o faltan configuraciones.");
        }
    }

    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }

    public int GetPoints()
        { return totalPoints; }
    
    public void SetLevel(int level)
    {
        selectedLevel = level;
        Debug.Log($"Nivel seleccionado: {level}");
    }

    public void SetPlayers(int players)
    {
        numberOfPlayers = players;
        Debug.Log($"N�mero de jugadores seleccionado: {players}");
    }

    public Dictionary<int, int> GetLife()
    { return lifePlayers; }

    private void ResetSelections()
    {
        selectedLevel = 0;
        numberOfPlayers = 0;
        isPlaying = false;
        endGame = false;
        lifePlayers[1]=100; lifePlayers[2]=100;
        totalPoints = 0;
        Debug.Log("Selecciones reiniciadas.");
    }

    public void ActivateShield(int playerNumber, float duration)
    {
        if (shieldStatus.ContainsKey(playerNumber))
        {
            shieldStatus[playerNumber] = true;
            Debug.Log($"Jugador {playerNumber} tiene el escudo activado durante {duration} segundos.");
            StartCoroutine(ShieldTimer(playerNumber, duration));
        }
    }

    private IEnumerator ShieldTimer(int playerNumber, float duration)
    {
        yield return new WaitForSeconds(duration);
        shieldStatus[playerNumber] = false;
        Debug.Log($"Escudo desactivado para el jugador {playerNumber}.");
    }

    public void TakeLife(int player, string reason)
    {
        if (shieldStatus.ContainsKey(player) && shieldStatus[player])
        {
            Debug.Log($"Jugador {player} est� protegido. No se le quita vida.");
            return;
        }

        int hurt = 0;

        // Determinar el da�o seg�n la causa
        switch (reason)
        {
            case "EnemyBullet":
                hurt = 3;
                break;
            case "EnemyShoot":
                hurt = 20;
                break;
            case "EnemyNoob":
                hurt = 15;
                break;
            case "PlanetBounce":
                hurt = 3;
                break;
            case "PlanetGravity":
                hurt = 5;
                break;
            case "PlanetDeath":
                lifePlayers[player] = 0;
                break;
            case "Meteorite":
                hurt = 10;
                break;
            default:
                Debug.LogWarning("Causa de da�o desconocida: " + reason);
                break;
        }

        if (lifePlayers.ContainsKey(player))
        {
            lifePlayers[player] -= hurt;
            if (lifePlayers[player] < 0)
            {
                lifePlayers[player] = 0; // Asegurarse de que la vida no sea negativa
            }
            
            Debug.Log($"Jugador {player} recibi� {hurt} de da�o por {reason}. Vida restante: {lifePlayers[player]}");
                        
            if (lifePlayers[player] == 0)
            {
                NotifyPlayerDeath(player);
            }
        }
    }

    public void AddPoints(int player, string objective)
    {
        int points = 0;

        // Determinar los puntos seg�n el objetivo destruido
        switch (objective)
        {
            case "EnemyShoot":
                points = 100;
                break;
            case "EnemyNoob":
                points = 30;
                break;
            case "Meteorite":
                points = 10;
                break;
            case "PowerUp":
                points = 50;
                break;
            default:
                Debug.LogWarning("Objetivo desconocido: " + objective);
                break;
        }

        // Sumar puntos al puntaje total
        totalPoints += points;
        Debug.Log($"Jugador {player} gan� {points} puntos por destruir {objective}. Puntos totales: {totalPoints}");
    }

    private void NotifyPlayerDeath(int player)
    {
        Debug.Log($"Jugador {player} ha muerto.");
        Player playerObject = FindPlayerByNumber(player); // Buscar al objeto del jugador
        if (playerObject != null)
        {
            playerObject.TriggerDeath();
        }
    }

    private Player FindPlayerByNumber(int playerNumber)
    {
        Player[] players = FindObjectsOfType<Player>();
        foreach (Player player in players)
        {
            if (player.playerNumber == playerNumber)
            {
                return player;
            }
        }
        return null;
    }
}
