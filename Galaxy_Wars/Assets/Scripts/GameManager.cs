using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int selectedLevel;
    public int numberOfPlayers;
    public bool isSecondPlayerAI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
        public void StartGame(int level, int players, bool useAI)
    {
        selectedLevel = level;
        numberOfPlayers = players;
        isSecondPlayerAI = useAI;

        UnityEngine.SceneManagement.SceneManager.LoadScene("BasicScene");
    }
}
