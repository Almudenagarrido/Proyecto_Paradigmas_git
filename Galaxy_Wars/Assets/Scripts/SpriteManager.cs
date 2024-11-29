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

    public Sprite meteoriteSprite;

    public Sprite enemyShipSprite;

    public Sprite bulletSprite;

    public Sprite blackholeSprite1;
    public Sprite blackholeSprite2;

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

    // Métodos para obtener sprites según el tipo de objeto
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
                return null;
        }
    }

    public Sprite GetNoiseSprite(int index)
    {
        if (index < 0 || index >= noiseBounceSprites.Length) return null;
        return noiseBounceSprites[index];
    }

    public Sprite GetBlackholeSprite(int type)
    {
        if (type == 1) return blackholeSprite1;
        if (type == 2) return blackholeSprite2;
        return null;
    }

    public Sprite GetPlayerSprite(int playerNumber)
    {
        return playerNumber == 1 ? player1Sprite : player2Sprite;
    }

    public Sprite GetBulletSprite(bool isPlayerBullet)
    {
        return isPlayerBullet ? bulletSprite : bulletSprite;
    }
}
