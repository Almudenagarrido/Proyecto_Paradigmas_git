using System.Collections.Generic;
using UnityEngine;

public class LevelFactory : MonoBehaviour
{
    public GameObject planetPrefab;
    public GameObject wormholePrefab;
    public GameObject playerPrefab;
    public GameObject bulletPrefab;
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
        CreateWormholes(level);
        SetupSpawner(level);

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
        Sprite[] noiseSprites = SpriteManager.Instance.GetNoiseSprites(type);
        planetComponent.noiseSprites = noiseSprites;

        planet.transform.localScale = new Vector3(scale, scale, 1.0f);

        CircleCollider2D collider = planet.GetComponent<CircleCollider2D>();
        if (collider != null)
        {
            collider.radius = planet.GetComponent<SpriteRenderer>().bounds.extents.x / planet.transform.localScale.x;
        }
    }

    private void CreateWormholes(int level)
    {
        if (level == 3)
        {
            GameObject wormhole1 = AddWormhole(new Vector2(-3.4f, 3.6f), 1, 1.2f);
            GameObject wormhole2 = AddWormhole(new Vector2(8.8f, -2.3f), 2, 1.2f);

            WormholeController comp1 = wormhole1.GetComponent<WormholeController>();
            WormholeController comp2 = wormhole2.GetComponent<WormholeController>();

            comp1.exitWormhole = comp2.transform;
            comp2.exitWormhole = comp1.transform;
        }
    }

    private GameObject AddWormhole(Vector2 position, int wormholeNumber, float scale)
    {
        GameObject wormhole = Instantiate(wormholePrefab, position, Quaternion.identity, levelRoot.transform);

        WormholeController wormholeComponent = wormhole.GetComponent<WormholeController>();
        wormholeComponent.wormholeNumber = wormholeNumber;

        Sprite blackholeSprite = SpriteManager.Instance.GetBlackholeSprite(wormholeNumber);
        wormhole.GetComponent<SpriteRenderer>().sprite = blackholeSprite;

        wormhole.transform.localScale = new Vector3(scale, scale, 1.0f);

        CircleCollider2D collider = wormhole.GetComponent<CircleCollider2D>();
        if (collider != null)
        {
            collider.radius = wormhole.GetComponent<SpriteRenderer>().bounds.extents.x / wormhole.transform.localScale.x;
            collider.isTrigger = true;
        }
        return wormhole;
    }

    private void CreatePlayers(int players)
    {
        for (int i = 0; i < (GameManager.Instance.isSecondPlayerAI ? 2 : players); i++)
        {
            Vector2 position = Vector2.zero;

            if (players == 1)
            {
                position = new Vector2(0, 0);
            }
            else if (players == 2 || GameManager.Instance.isSecondPlayerAI)
            {
                if (i == 0)
                    position = new Vector2(2.5f, 0);
                else if (i == 1)
                    position = new Vector2(-2.5f, 0);
            }

            GameObject player = Instantiate(playerPrefab, position, Quaternion.identity, levelRoot.transform);
            Player playerComponent = player.GetComponent<Player>();
            playerComponent.smokeSprites = SpriteManager.Instance.GetSmokes();
            player.transform.localScale = new Vector3(0.8f, 0.8f, 1.0f);
            playerComponent.shootingPoint = playerComponent.transform;

            playerComponent.bulletPrefab = bulletPrefab;

            if (i == 1 && GameManager.Instance.isSecondPlayerAI)
            {
                // Configuración para la IA
                playerComponent.playerNumber = 2;
                playerComponent.isAI = true;
                player.GetComponent<SpriteRenderer>().sprite = SpriteManager.Instance.GetPlayerSprite(3);
            }
            else
            {
                // Configuración para jugadores humanos
                playerComponent.playerNumber = i + 1;
                playerComponent.isAI = false;
                player.GetComponent<SpriteRenderer>().sprite = SpriteManager.Instance.GetPlayerSprite(i + 1);
            }

            Debug.Log($"Jugador {playerComponent.playerNumber} creado en posición {player.transform.position} - {(playerComponent.isAI ? "IA" : "Humano")}.");
        }
    }

    private void SetupSpawner(int level)
    {
        GameObject spawnerObject = GameObject.Find("LevelSpawner");
        if (spawnerObject == null)
        {
            Debug.LogError("No se encontró un GameObject llamado 'LevelSpawner' en la escena.");
            return;
        }

        LevelSpawner levelSpawner = spawnerObject.GetComponent<LevelSpawner>();
        if (levelSpawner == null)
        {
            Debug.LogError("El GameObject 'LevelSpawner' no tiene un componente LevelSpawner.");
            return;
        }

        if (level == 1)
        {
            levelSpawner.ConfigureSpawner(1.0f, 0f, 0f, 15f, true, false, false, true);
        }
        else if (level == 2)
        {
            levelSpawner.ConfigureSpawner(0.9f, 5f, 0f, 20f, true, true, false, true);
        }
        else if (level == 3)
        {
            levelSpawner.ConfigureSpawner(0.8f, 4f, 6f, 25f, true, true, true, true);
        }
    }
}
