using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int? selectedLevel = null;
    public int? numberOfPlayers = null;
    public bool isSecondPlayerAI = false;
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
        ResetSelections();
        SceneManager.LoadScene("MainMenu");
    }

    public void StartGame()
    {
        if (selectedLevel.HasValue && numberOfPlayers.HasValue)
        {
            isPlaying = true;
            Debug.Log($"Iniciando juego con nivel {selectedLevel.Value} y {numberOfPlayers.Value} jugadores...");
            SceneManager.LoadScene("BasicSceneLevels");
            Invoke(nameof(CreateLevel), 0.1f);
        }
        else
        {
            Debug.LogError("No se han seleccionado el nivel o el número de jugadores. No se puede iniciar el juego.");
        }
    }

    private void CreateLevel()
    {
        if (levelFactory != null && selectedLevel.HasValue && numberOfPlayers.HasValue)
        {
            levelFactory.CreateLevel(selectedLevel.Value, numberOfPlayers.Value);
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

    private void ResetSelections()
    {
        selectedLevel = null;
        numberOfPlayers = null;
        isPlaying = false;
        endGame = false;
        Debug.Log("Selecciones reiniciadas.");
    }
}
