using UnityEngine;

public class LevelFactory : MonoBehaviour
{
    public GameObject planetPrefab;
    public GameObject blackholePrefab;
    public GameObject playerPrefab;

    private GameObject sceneRoot;

    private void Start()
    {
        sceneRoot = new GameObject("LevelRoot");

        int level = GameManager.Instance.selectedLevel;
        int players = GameManager.Instance.numberOfPlayers;

        // Crear elementos según el nivel.
        CreatePlanets(level);

        // Crear jugadores.
        CreatePlayers(players);

        Debug.Log("Level created!");
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
            //AddBlackhole(new Vector2(5, 5));
        }
    }

    private void AddPlanet(Vector2 position, Planet.PlanetType type)
    {
        GameObject planet = Instantiate(planetPrefab, position, Quaternion.identity, sceneRoot.transform);
        Planet planetComponent = planet.GetComponent<Planet>();
        planetComponent.planetType = type;

        // Asignar sprite del planeta y ruido desde el SpriteManager
        planetComponent.planetSprite = SpriteManager.Instance.GetPlanetSprite(type);
        planetComponent.noiseSprites = SpriteManager.Instance.noiseBounceSprites;
    }

    private void AddBlackhole(Vector2 position, int blackholeNumber)
    {
        GameObject blackhole = Instantiate(blackholePrefab, position, Quaternion.identity, sceneRoot.transform);
        Blackhole blackholeComponent = blackhole.GetComponent<Blackhole>();
        blackholeComponent.blackholeNumber = blackholeNumber;

        // Asignar sprite del agujero negro desde el SpriteManager
        blackhole.GetComponent<SpriteRenderer>().sprite = SpriteManager.Instance.GetBlackholeSprite(blackholeNumber);
    }

    private void CreatePlayers(int players)
    {
        for (int i = 0; i < players; i++)
        {
            GameObject player = Instantiate(playerPrefab, new Vector2(-5 + i * 2, 0), Quaternion.identity, sceneRoot.transform);
            Player playerComponent = player.GetComponent<Player>();
            playerComponent.playerNumber = i + 1;

            // Asignar sprite desde el SpriteManager
            player.GetComponent<SpriteRenderer>().sprite = SpriteManager.Instance.GetPlayerSprite(i + 1);

            // Configurar IA si es necesario
            if (i == 1 && GameManager.Instance.isSecondPlayerAI)
            {
                //player.AddComponent<PlayerAI>();
            }
        }
    }

}
