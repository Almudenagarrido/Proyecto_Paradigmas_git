using UnityEngine;

public class LevelFactory : MonoBehaviour
{
    public GameObject planetPrefab;
    public GameObject blackholePrefab;
    public GameObject playerPrefab;
    private GameObject levelRoot;

    public void CreateLevel(int level, int players)
    {
        // Reiniciar el nivel actual
        if (levelRoot != null)
        {
            Destroy(levelRoot);
        }

        levelRoot = new GameObject("LevelRoot");

        // Crear elementos del nivel
        CreatePlanets(level);
        CreatePlayers(players);
        CreateBlackholes(level);

        Debug.Log($"Nivel {level} creado con {players} jugadores.");
    }

    private void CreatePlanets(int level)
    {
        if (level >= 1)
        {
            AddPlanet(new Vector2(-7.02f, 0.19f), Planet.PlanetType.Bounce, 0.14f);
        }
        if (level >= 2)
        {
            AddPlanet(new Vector2(6.02f, 0.99f), Planet.PlanetType.Gravity, 0.13f);
        }
        if (level == 3)
        {
            AddPlanet(new Vector2(1.93f, -3.2f), Planet.PlanetType.Death, 0.12f);
        }
    }

    private void AddPlanet(Vector2 position, Planet.PlanetType type, float scale)
    {
        GameObject planet = Instantiate(planetPrefab, position, Quaternion.identity, levelRoot.transform);

        Planet planetComponent = planet.GetComponent<Planet>();
        planetComponent.planetType = type;

        Sprite planetSprite = SpriteManager.Instance.GetPlanetSprite(type);
        planetComponent.planetSprite = planetSprite;
        planet.GetComponent<SpriteRenderer>().sprite = planetSprite;

        planet.transform.localScale = new Vector3(scale, scale, 1.0f);

        CircleCollider2D collider = planet.GetComponent<CircleCollider2D>();
        if (collider != null)
        {
            collider.radius = planet.GetComponent<SpriteRenderer>().bounds.extents.x / planet.transform.localScale.x;
        }
    }

    private void CreateBlackholes(int level)
    {
        if (level == 3)
        {
            AddBlackHole(new Vector2(-3.4f, 3.6f), 1, 1.2f);
            AddBlackHole(new Vector2(8.8f, -2.3f), 2, 1.2f);
        }
    }

    private void AddBlackHole(Vector2 position, int blackholeNumber, float scale)
    {
        GameObject blackhole = Instantiate(blackholePrefab, position, Quaternion.identity, levelRoot.transform);

        WormholeController blackholeComponent = blackhole.GetComponent<WormholeController>();
        blackholeComponent.blackholeNumber = blackholeNumber;

        Sprite blackholeSprite = SpriteManager.Instance.GetBlackholeSprite(blackholeNumber);
        blackhole.GetComponent<SpriteRenderer>().sprite = blackholeSprite;

        blackhole.transform.localScale = new Vector3(scale, scale, 1.0f);

        CircleCollider2D collider = blackhole.GetComponent<CircleCollider2D>();
        if (collider != null)
        {
            collider.radius = blackhole.GetComponent<SpriteRenderer>().bounds.extents.x / blackhole.transform.localScale.x;
        }
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
