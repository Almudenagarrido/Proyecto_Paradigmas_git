using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    private List<string> Scenes = new List<string>;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   
    void LoadScene(string nameScene)
    {

    }

    bool SceneExists(string nameScene)
    {
        if (nameScene in Scenes)
        {
            return true;
        }
        else
        {
            Scenes.add(nameScene);
        }

    }
}
