using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerCustom : MonoBehaviour
{
    private string currentScene;

    private void Start()
    {
        // Inicializar la escena actual con la escena activa al inicio
        currentScene = SceneManager.GetActiveScene().name;
    }

    public void LoadScene(string sceneName)
    {
        // Evitar recargar la misma escena
        if (currentScene == sceneName)
        {
            Debug.Log($"La escena {sceneName} ya está activa.");
            return;
        }

        // Cargar la nueva escena y actualizar la escena actual
        Debug.Log($"Cargando escena: {sceneName}");
        SceneManager.LoadScene(sceneName);
        currentScene = sceneName;
    }
}

