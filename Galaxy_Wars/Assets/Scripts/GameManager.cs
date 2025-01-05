using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private MenuController menuController;
    public int selectedLevel;
    public int numberOfPlayers;
    public bool isSecondPlayerAI;
    public bool isPlaying = false;
    public bool endGame = false;

    private SpriteManager spriteManager;
    private LevelFactory levelFactory;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeManagers();
            LoadMenu();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (isPlaying && SceneManager.GetActiveScene().name != "BasicSceneLevels")
        {
            StartGame();
        }
        else if (!isPlaying && SceneManager.GetActiveScene().name != "MainMenu")
        {
            LoadMenu();
        }

        if (endGame)
        {
            QuitGame();
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

    private void LoadMenu()
    {
        Debug.Log("Cargando el menú...");
        SceneManager.LoadScene("MainMenu");
    }

    public void returnLevel()
    {
        menuController = GetComponent<MenuController>();
        selectedLevel = menuController.GetLevel();
    }

    public void returnPlayers()
    {
        menuController = GetComponent<MenuController>();
        numberOfPlayers = menuController.GetPlayers();
    }

    public void StartGame()
    {
        returnLevel();
        returnPlayers();
        Debug.Log($"Iniciando juego con nivel {selectedLevel} y {numberOfPlayers} jugadores...");
        SceneManager.LoadScene("BasicLevelScene");  // Escena Base
        Invoke(nameof(CreateLevel), 0.1f); // Crear nivel dinamicamente
    }

    private void CreateLevel()
    {
        if (levelFactory != null)
        {
            levelFactory.CreateLevel(selectedLevel, numberOfPlayers);
        }
        else
        {
            Debug.LogError("El LevelFactory no está inicializado.");
        }
    }

    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}
