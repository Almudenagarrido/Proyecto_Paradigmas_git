using UnityEngine;
using TMPro;

public class MenuController : MonoBehaviour
{
    public TMP_Dropdown levelDropdown;
    public TMP_Dropdown playersDropdown;
    public GameObject popUpInstr;

    private void Start()
    {
        // Suscribirse a cambios en los dropdowns
        levelDropdown.onValueChanged.AddListener(delegate { DropdownValueChangedLevels(); });
        playersDropdown.onValueChanged.AddListener(delegate { DropdownValueChangedPlayers(); });

        // Inicializar dropdowns con valores predeterminados
        InitializeDropdowns();
    }

    private void InitializeDropdowns()
    {
        // Si el GameManager tiene valores previos, configúralos en los dropdowns
        if (GameManager.Instance.selectedLevel != 0)
        {
            levelDropdown.value = GameManager.Instance.selectedLevel;
        }

        if (GameManager.Instance.numberOfPlayers != 0)
        {
            playersDropdown.value = GameManager.Instance.numberOfPlayers;
        }
    }

    private void DropdownValueChangedLevels()
    {
        // Guardar selección de nivel en el GameManager
        int selectedLevel = levelDropdown.value;
        GameManager.Instance.SetLevel(selectedLevel); // Usar el método SetLevel del GameManager
        Debug.Log("Nivel seleccionado: " + selectedLevel);
    }

    private void DropdownValueChangedPlayers()
    {
        // Guardar selección de jugadores en el GameManager
        int playerOption = playersDropdown.value;
        GameManager.Instance.SetPlayers(playerOption);
        if (playerOption == 3) { GameManager.Instance.isSecondPlayerAI = true; }
        Debug.Log("Jugadores seleccionados: " + playerOption);
    }

    public void ShowInstructions()
    {
        // Mostrar las instrucciones
        popUpInstr.SetActive(true);
    }

    public void CloseInstructions()
    {
        // Cerrar las instrucciones
        popUpInstr.SetActive(false);
    }

    public void OnPlayButtonPressed()
    {
        // Validar que tanto nivel como número de jugadores estén seleccionados
        if (GameManager.Instance.selectedLevel != 0 && GameManager.Instance.numberOfPlayers != 0)
        {
            GameManager.Instance.StartGame(); // Llamar al método StartGame del GameManager
        }
        else
        {
            Debug.LogWarning("Por favor, selecciona un nivel y el número de jugadores antes de iniciar el juego.");
        }
    }

    public void OnQuitButtonPressed()
    {
        // Llamar al método QuitGame del GameManager
        GameManager.Instance.QuitGame();
    }
}
