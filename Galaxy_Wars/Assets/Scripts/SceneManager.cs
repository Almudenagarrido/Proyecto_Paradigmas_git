using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerCustom : MonoBehaviour
{
    private string currentScene = "";

    public void LoadScene(string sceneName)
    {
        if (currentScene == sceneName)
        {
            Debug.Log($"La escena {sceneName} ya est� activa.");
            return;
        }

        Debug.Log($"Cargando escena: {sceneName}");
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        currentScene = sceneName;
    }
}
