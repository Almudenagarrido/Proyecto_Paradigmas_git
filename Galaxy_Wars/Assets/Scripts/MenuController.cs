using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private Dropdown levelDropdown;
    private Dropdown playersDropdown;
    public int levelNumber; //1: level 1, 2: level 2, 3: level 3
    public int playerOption; //1: single player, 2: multiplayer, 3: multilayer ia (hay que hacerlo)
    public GameObject popUpInstr;
    
    //public Toggle aiToggle;
    //public Button startButton;

    private void Start()
    {
        //startButton.onClick.AddListener(OnStartGame);
        levelDropdown.onValueChanged.AddListener(delegate { 
            DropdownValueChangedLevels(levelDropdown); 
        });
    }

    private void Update()
    {

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
    
    public void ShowIntructions()
    {
        popUpInstr.SetActive(true);
    }

    public void CloseIntructions()
    {
        popUpInstr.SetActive(false);
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
    

