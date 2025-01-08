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
    private Button exitButton;
    private Button backButton = null;

    public int selectedLevel = 0;
    public int numberOfPlayers = 0;
    public bool isSecondPlayerAI = false;
    public bool isPlaying = false;
    public bool endGame = false;

    public Dictionary<int, int> lifePlayers = new Dictionary<int, int> { { 1, 100 }, { 2, 100 } };
    public Dictionary<int, int> pointsPlayers = new Dictionary<int, int> { { 1, 0 }, { 2, 0 } };
    public List<GameObject> playerLifeBars = new List<GameObject>();
    public List<GameObject> playerFillBars = new List<GameObject>();
    
    private SpriteManager spriteManager;
    private LevelFactory levelFactory;

    private void Start()
    {
        playButton = GameObject.Find("PlayButton").GetComponent<Button>();
        //exitButton = GameObject.Find("ExitButton").GetComponent<Button>();
       
        if (playButton != null)
        {
            playButton.onClick.AddListener(StartGame);
            //exitButton.onClick.AddListener(EndGame);
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
            backButton.onClick.AddListener(EndGame);  // Para volver al menú
            Debug.Log("Botón BackButton asignado.");
        }
    }

    private void AssignPlayExitButtons()
    {
        playButton = GameObject.Find("PlayButton")?.GetComponent<Button>();
        exitButton = GameObject.Find("QuitButton")?.GetComponent<Button>();
        if (playButton != null)
        {
            playButton.onClick.RemoveAllListeners();
            playButton.onClick.AddListener(StartGame);
            exitButton.onClick.AddListener(QuitGame);
        }
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
        Debug.Log("Cargando el menú...");
        SceneManager.LoadScene("MainMenu");
        ResetSelections();
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
            Debug.LogError("No se han seleccionado el nivel o el número de jugadores. No se puede iniciar el juego.");
        }
    }

    public void CheckGameOver()
    {
        bool allPlayersDead = false;
        if (numberOfPlayers == 1)
        {
            lifePlayers[2] = 0;
        }


        if (lifePlayers[1] == 0 && lifePlayers[2] == 0)
        {
            allPlayersDead = true;
        }
        
        if (allPlayersDead)
        {
            Debug.Log("Todos los jugadores han muerto. Fin del juego.");
            SceneManager.LoadScene("GameOver");
            
        }
    }

    public void EndGame()
    {
        endGame = true;
        isPlaying = false;
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
            Debug.LogError("El LevelFactory no está inicializado o faltan configuraciones.");
        }
    }

    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }

    public void SetLevel(int level)
    {
        selectedLevel = level;
        Debug.Log($"Nivel seleccionado: {level}");
    }

    public void SetPlayers(int players)
    {
        numberOfPlayers = players;
        Debug.Log($"Número de jugadores seleccionado: {players}");
    }

    public Dictionary<int, int> GetPoints()
    { return pointsPlayers; }

    public Dictionary<int, int> GetLife()
    { return lifePlayers; }

    public int GetNumberPlayers()
    { return numberOfPlayers; }

    private void ResetSelections()
    {
        selectedLevel = 0;
        numberOfPlayers = 0;
        isPlaying = false;
        endGame = false;
        lifePlayers[1]=100; lifePlayers[2]=100;
        pointsPlayers[1] = 0; pointsPlayers[2]=0;
        Debug.Log("Selecciones reiniciadas.");
    }
    
    public void TakeLife(int player, string reason)
    {
        int hurt = 0;

        // Determinar el daño según la causa
        switch (reason)
        {
            case "EnemyBullet":
                hurt = 10;
                break;
            case "PlanetBounce":
                hurt = 2;
                break;
            case "PlanetGravity":
                hurt = 3;
                break;
            case "PlanetDeath":
                lifePlayers[player] = 0;
                break;
            case "Meteorite":
                hurt = 5;
                break;
            default:
                Debug.LogWarning("Causa de daño desconocida: " + reason);
                break;
        }

        if (lifePlayers.ContainsKey(player))
        {
            lifePlayers[player] -= hurt;
            if (lifePlayers[player] < 0)
            {
                lifePlayers[player] = 0; // Asegurarse de que la vida no sea negativa
            }
            
            Debug.Log($"Jugador {player} recibió {hurt} de daño por {reason}. Vida restante: {lifePlayers[player]}");
                        
            if (lifePlayers[player] == 0)
            {
                NotifyPlayerDeath(player);
            }
        }
    }

    public void AddPoints(int player, string objective)
    {
        int points = 0;

        // Determinar los puntos según el objetivo destruido
        switch (objective)
        {
            case "NaveEnemigaTipo1":
                points = 50;
                break;
            case "NaveEnemigaTipo2":
                points = 100;
                break;
            case "Meteorite":
                points = 10;
                break;
            case "EnemyBullet":
                points = 1;
                break;
            default:
                Debug.LogWarning("Objetivo desconocido: " + objective);
                break;
        }

        if (pointsPlayers.ContainsKey(player))
        {
            pointsPlayers[player] += points;
            Debug.Log($"Jugador {player} ganó {points} puntos por destruir {objective}. Puntos totales: {pointsPlayers[player]}");
        }
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

    // Método auxiliar para buscar el objeto del jugador basado en su número
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
