using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    public static SpriteManager Instance;

    public Sprite bounceSprite;
    public Sprite gravitySprite;
    public Sprite deathSprite;

    public Sprite[] noiseBounceSprites;
    public Sprite[] noiseGravitySprites;
    public Sprite[] noiseDeathSprites;

    public Sprite player1Sprite;
    public Sprite player2Sprite;
    public Sprite playerAISprite;

    public Sprite meteoriteSprite;

    public Sprite enemyShipSprite;

    public Sprite blackholeSprite1;
    public Sprite blackholeSprite2;

    public Sprite whiteSmokeSprite;
    public Sprite yellowSmokeSprite;
    public Sprite redSmokeSprite;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Sprite GetPlanetSprite(Planet.PlanetType type)
    {
        switch (type)
        {
            case Planet.PlanetType.Bounce:
                return bounceSprite;
            case Planet.PlanetType.Gravity:
                return gravitySprite;
            case Planet.PlanetType.Death:
                return deathSprite;
            default:
                Debug.LogWarning($"Tipo de planeta no reconocido: {type}");
                return null;
        }
    }

    public Sprite[] GetNoiseSprites(Planet.PlanetType type)
    {
        switch (type)
        {
            case Planet.PlanetType.Bounce:
                return noiseBounceSprites;
            case Planet.PlanetType.Gravity:
                return noiseGravitySprites;
            case Planet.PlanetType.Death:
                return noiseDeathSprites;
            default:
                Debug.LogWarning($"Tipo de ruido para planeta no reconocido: {type}");
                return null;
        }
    }

    public Sprite GetBlackholeSprite(int type)
    {
        switch (type)
        {
            case 1:
                return blackholeSprite1;
            case 2:
                return blackholeSprite2;
            default:
                Debug.LogWarning($"Tipo de agujero negro no reconocido: {type}");
                return null;
        }
    }

    public Sprite[] GetSmokes()
    {
        return new Sprite[]
        {
            whiteSmokeSprite,
            yellowSmokeSprite,
            redSmokeSprite
        };
    }

    public Sprite GetPlayerSprite(int playerNumber)
    {
        switch (playerNumber)
        {
            case 1:
                return player1Sprite;
            case 2:
                return player2Sprite;
            case 3:
                return playerAISprite;
            default:
                Debug.LogWarning($"Número de jugador no reconocido: {playerNumber}");
                return null;
        }
    }
    
    public Sprite GetEnemySprite()
    {
        return enemyShipSprite;
    }
    
    public Sprite GetMeteoriteSprite()
    {
        return meteoriteSprite;
    }
}
