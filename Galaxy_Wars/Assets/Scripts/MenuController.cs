using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuController : MonoBehaviour
{
    public Dropdown levelDropdown;
    public Dropdown playersDropdown;
    public int levelNumber;
    public int playerOption; //1: single player, 2: multiplayer, 3: multilayer ia (hay que hacerlo)
    //public Toggle aiToggle;
    //public Button startButton;

    private void Start()
    {
        //startButton.onClick.AddListener(OnStartGame);
        levelDropdown.onValueChanged.AddListener(delegate { 
            DropdownValueChangedLevels(levelDropdown); 
        });
    }
    
    void DropdownValueChangedLevels(Dropdown levelsDropdown)
    {
        switch (levelsDropdown.value)
        {
            case 0:
                levelNumber = 1;
                break;
            case 1:
                levelNumber = 2;
                break;
            case 2:
                levelNumber = 3;
                break;
            default:
                levelNumber = 1;
                break;
        }
        Debug.Log("Valor seleccionado: " + levelNumber);
    }

    private void Update()
    {

    }

    private void PlayButton(string sceneName)
    {
        //SceneManager.LoadScene(sceneName);
    }

    private void OnStartGame()
    {
        //int selectedLevel = levelDropdown.value + 1; // Asumiendo 1, 2, 3.
        //int numberOfPlayers = playersDropdown.value + 1; // 1 o 2 jugadores.
        //bool useAI = aiToggle.isOn;

        //GameManager.Instance.StartGame(selectedLevel, numberOfPlayers, useAI);
    }
}
    

