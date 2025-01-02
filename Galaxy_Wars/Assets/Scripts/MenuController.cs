using UnityEngine;
//using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    //public Dropdown levelDropdown;
    //public Dropdown playersDropdown;
    //public Toggle aiToggle;
    //public Button startButton;

    private void Start()
    {
        //startButton.onClick.AddListener(OnStartGame);
    }

    private void Update()
    {
        
    }

    private void OnStartGame()
    {
        //int selectedLevel = levelDropdown.value + 1; // Asumiendo 1, 2, 3.
        //int numberOfPlayers = playersDropdown.value + 1; // 1 o 2 jugadores.
        //bool useAI = aiToggle.isOn;

        //GameManager.Instance.StartGame(selectedLevel, numberOfPlayers, useAI);
    }

    public void PlayButton(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

