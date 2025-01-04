using UnityEngine;

public class LevelFactory : MonoBehaviour
{
    public GameObject planetPrefab;
    public GameObject blackholePrefab;
    public GameObject playerPrefab;
    private GameObject levelRoot;

    public void CreateLevel(int level, int players)
    {
        if (levelRoot != null)
        {
            Destroy(levelRoot);
        }

        levelRoot = new GameObject("LevelRoot");

        CreatePlanets(level);
        CreatePlayers(players);
        CreateBlackholes(level);

        Debug.Log($"Nivel {level} creado con {players} jugadores.");
    }

    private void CreatePlanets(int level)
    {
        if (level >= 1)
        {
            AddPlanet(new Vector2(0, 0), Planet.PlanetType.Bounce);
        }
        if (level >= 2)
        {
            AddPlanet(new Vector2(3, 0), Planet.PlanetType.Gravity);
        }
        if (level == 3)
        {
            AddPlanet(new Vector2(-3, 0), Planet.PlanetType.Death);
        }
    }

    private void AddPlanet(Vector2 position, Planet.PlanetType type)
    {
        GameObject planet = Instantiate(planetPrefab, position, Quaternion.identity, levelRoot.transform);
        Planet planetComponent = planet.GetComponent<Planet>();
        planetComponent.planetType = type;

        // Asignar sprite del planeta y ruido desde el SpriteManager
        planetComponent.planetSprite = SpriteManager.Instance.GetPlanetSprite(type);
        planetComponent.noiseSprites = SpriteManager.Instance.GetNoiseSprites(type);
    }

    private void CreateBlackholes(int level)
    {
        if (level >= 2)
        {
            AddBlackHole(new Vector2(-2, 2), 1);
        }
        if (level == 3)
        {
            AddBlackHole(new Vector2(0, 3), 2);
        }
    }

    private void AddBlackHole(Vector2 position, int blackholeNumber)
    {
        GameObject blackhole = Instantiate(blackholePrefab, position, Quaternion.identity, levelRoot.transform);
        Blackhole blackholeComponent = blackhole.GetComponent<Blackhole>();
        blackholeComponent.blackholeNumber = blackholeNumber;

        // Asignar sprite del agujero negro desde el SpriteManager
        blackhole.GetComponent<SpriteRenderer>().sprite = SpriteManager.Instance.GetBlackholeSprite(blackholeNumber);
    }

    private void CreatePlayers(int players)
    {
        for (int i = 0; i < players; i++)
        {
            GameObject player = Instantiate(playerPrefab, new Vector2(-5 + i * 2, 0), Quaternion.identity, levelRoot.transform);
            Player playerComponent = player.GetComponent<Player>();

            if (i == 1 && GameManager.Instance.isSecondPlayerAI)
            {
                playerComponent.playerNumber = 3;
                player.GetComponent<SpriteRenderer>().sprite = SpriteManager.Instance.GetPlayerSprite(3);
            }
            else
            {
                playerComponent.playerNumber = i + 1;
                player.GetComponent<SpriteRenderer>().sprite = SpriteManager.Instance.GetPlayerSprite(i + 1);
            }

            Debug.Log($"Jugador {playerComponent.playerNumber} creado en posición {player.transform.position} - {(i == 1 && GameManager.Instance.isSecondPlayerAI ? "IA" : "Humano")}");
        }
    }


}
