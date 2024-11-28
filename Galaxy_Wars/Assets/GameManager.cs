using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class GameManager : MonoBehaviour
{
    SceneFactoryScreator factory
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonStartNewGame)
        {
            factory.create(level0); //-> instaciar los gameobjects especificos del nivel
            Launcher.scene(GameScene);
        }
        else if(buttonContinueGame)
        {
            factory.create(lastLevel); //-> instaciar los gameobjects especificos del nivel
            Launcher.scene(GameScene);
        }
        else if (buttonExit)
        {

        }
    }
}
