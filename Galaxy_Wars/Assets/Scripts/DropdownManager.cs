using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownManager : MonoBehaviour
{
    private Dropdown levels;
    private Dropdown players;
    private int levelNumber;
    private int playerNumber;
    public string dropdownName;

    // Start is called before the first frame update
    void Start()
    {
        levels.onValueChanged.AddListener(delegate {
            DropdownValueChanged(levels);
        });

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void DropdownValueChanged(Dropdown levels)
    {
        if (dropdownName == "Level")
        {
            switch (levels.value)
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
        else if (dropdownName == "Player")
        {
            switch (players.value)
            {
                case 0:
                    playerNumber = 1;
                    break;
                case 1:
                    playerNumber = 2;
                    break;
                case 2:
                    playerNumber = 3;
                    break;
                default:
                    levelNumber = 1;
                    break;
            }
            Debug.Log("Valor seleccionado: " + playerNumber);
        }

    }
}
