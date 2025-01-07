using System.Collections.Generic;
using UnityEngine;

public class LevelFactory : MonoBehaviour
{
    public GameObject planetPrefab;
    public GameObject wormholePrefab;
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
        CreateWormholes(level);

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
            AddWormhole(new Vector2(-3.4f, 3.6f), 1, 1.2f);
            AddWormhole(new Vector2(8.8f, -2.3f), 2, 1.2f);
        }
    }

    private void AddWormhole(Vector2 position, int wormholeNumber, float scale)
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
        }
    }

    private void CreatePlayers(int players)
    {
        for (int i = 0; i < players; i++)
        {
            Vector2 position = Vector2.zero;

            // Asignar posiciones dependiendo del n�mero de jugadores
            if (players == 1)
            {
                position = new Vector2(0, 0); // Un jugador empieza en el centro
            }
            else if (players == 2)
            {
                if (i == 0)
                    position = new Vector2(2.5f, 0); // Jugador 1 a la izquierda
                else if (i == 1)
                    position = new Vector2(-2.5f, 0); // Jugador 2 a la derecha
            }

            // Crear el jugador en la posici�n asignada
            GameObject player = Instantiate(playerPrefab, position, Quaternion.identity, levelRoot.transform);
            Player playerComponent = player.GetComponent<Player>();
            player.GetComponent<Player>().smokeSprites = SpriteManager.Instance.GetSmokes();
            player.transform.localScale = new Vector3(0.8f, 0.8f, 1.0f);

            // Configurar si el jugador es IA o humano
            if (i == 1 && GameManager.Instance.isSecondPlayerAI)
            {
                playerComponent.playerNumber = 3; // IA es el jugador 3
                player.GetComponent<SpriteRenderer>().sprite = SpriteManager.Instance.GetPlayerSprite(3);
            }
            else
            {
                playerComponent.playerNumber = i + 1; // Jugador humano
                player.GetComponent<SpriteRenderer>().sprite = SpriteManager.Instance.GetPlayerSprite(i + 1);
            }

            Debug.Log($"Jugador {playerComponent.playerNumber} creado en posici�n {player.transform.position} - {(i == 1 && GameManager.Instance.isSecondPlayerAI ? "IA" : "Humano")}");
        }
    }

}
