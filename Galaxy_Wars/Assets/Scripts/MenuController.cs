using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Dropdown levelDropdown;
    public Dropdown playersDropdown;
    public GameObject popUpInstr;

    private void Start()
    {
        // Suscribirse a cambios en los dropdowns
        levelDropdown.onValueChanged.AddListener(delegate { DropdownValueChangedLevels(); });
        playersDropdown.onValueChanged.AddListener(delegate { DropdownValueChangedPlayers(); });
    }

    private void DropdownValueChangedLevels()
    {
        // Guardar selección de nivel en el GameManager
        int selectedLevel = levelDropdown.value + 1;
        GameManager.Instance.selectedLevel = selectedLevel;
        Debug.Log("Nivel seleccionado: " + selectedLevel);
    }

    private void DropdownValueChangedPlayers()
    {
        // Guardar selección de jugadores en el GameManager
        int playerOption = playersDropdown.value + 1;
        GameManager.Instance.numberOfPlayers = playerOption;
        Debug.Log("Jugadores seleccionados: " + playerOption);
    }

    public void ShowInstructions()
    {
        popUpInstr.SetActive(true);
    }

    public void CloseInstructions()
    {
        popUpInstr.SetActive(false);
    }

    public void OnPlayButtonPressed()
    {
        // Establecer el booleano "jugando" en true y notificar al GameManager
        GameManager.Instance.isPlaying = true;
        Debug.Log("Iniciando el juego...");
    }
    public void OnQuitButtonPressed()
    {
        GameManager.Instance.endGame = true;
        Debug.Log("Saliendo del juego...");
    }
}