using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    public GameObject meteoritePrefab;
    public GameObject enemyNoobPrefab;
    public GameObject enemyShootPrefab;
    public GameObject powerUpPrefab;

    public float meteoriteFrequency = 1.0f;
    public float noobEnemyFrequency = 3.0f;
    public float shootEnemyFrequency = 5.0f;
    public float powerUpFrequency = 10f;

    private bool spawnMeteorites = false;
    private bool spawnNoobEnemies = false;
    private bool spawnShootEnemies = false;
    private bool spawnPowerUps = false;

    public void ConfigureSpawner(float meteoriteFreq, float noobFreq, float shootFreq, float powerFreq, bool spawnMeteor, bool spawnNoob, bool spawnShoot, bool power)
    {
        // Configurar frecuencias y estados de spawn
        meteoriteFrequency = meteoriteFreq;
        noobEnemyFrequency = noobFreq;
        shootEnemyFrequency = shootFreq;
        powerUpFrequency = powerFreq;

        spawnMeteorites = spawnMeteor;
        spawnNoobEnemies = spawnNoob;
        spawnShootEnemies = spawnShoot;
        spawnPowerUps = power;

        StartSpawning();
    }

    private void StartSpawning()
    {
        // Iniciar spawners según configuración
        if (spawnMeteorites)
        {
            InvokeRepeating(nameof(SpawnMeteorite), 0f, meteoriteFrequency);
        }

        if (spawnNoobEnemies)
        {
            InvokeRepeating(nameof(SpawnNoobEnemy), 0f, noobEnemyFrequency);
        }

        if (spawnShootEnemies)
        {
            InvokeRepeating(nameof(SpawnShootEnemy), 0f, shootEnemyFrequency);
        }

        if (spawnPowerUps)
        {
            InvokeRepeating(nameof(SpawnPowerUps), 10f, powerUpFrequency);
        }
    }

    private void SpawnMeteorite()
    {
        if (meteoritePrefab == null) { return; }

        Instantiate(meteoritePrefab);
        meteoritePrefab.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
    }

    private void SpawnNoobEnemy()
    {
        if (enemyNoobPrefab == null) { return; }

        Instantiate(enemyNoobPrefab);
        enemyNoobPrefab.transform.localScale = new Vector3(0.6f, 0.6f, 1f);

    }

    private void SpawnShootEnemy()
    {
        if (enemyShootPrefab == null) { return; }

        Instantiate(enemyShootPrefab);
        enemyShootPrefab.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
    }

    private void SpawnPowerUps()
    {
        if (powerUpPrefab == null) { return; }

        Vector2 randomPosition = new Vector2(Random.Range(-5f, 5f), Random.Range(-5f, 5f));
        GameObject powerUpInstance = Instantiate(powerUpPrefab, new Vector3(randomPosition.x, randomPosition.y, 0), Quaternion.identity);
        powerUpInstance.transform.localScale = new Vector3(1.8f, 1.8f, 1f);

        powerUp powerUpScript = powerUpInstance.GetComponent<powerUp>();
        if (powerUpScript != null)
        {
            powerUp.PowerUpType randomType = (Random.value > 0.5f) ? powerUp.PowerUpType.Shield : powerUp.PowerUpType.Points;
            powerUpScript.powerUpType = randomType;

            SpriteManager spriteManager = SpriteManager.Instance;
            SpriteRenderer spriteRenderer = powerUpInstance.GetComponent<SpriteRenderer>();
            
            if (randomType == powerUp.PowerUpType.Shield)
            {
                powerUpInstance.GetComponent<SpriteRenderer>().sprite = spriteManager.lifePowerUp;
            }
            else if (randomType == powerUp.PowerUpType.Points)
            {
                powerUpInstance.GetComponent<SpriteRenderer>().sprite = spriteManager.pointsPowerUp;
            }
            
            CircleCollider2D collider = powerUpInstance.GetComponent<CircleCollider2D>();
            if (collider == null)
            {
                collider = powerUpInstance.AddComponent<CircleCollider2D>();
            }
            collider.isTrigger = true;
            collider.radius = spriteRenderer.bounds.extents.x;
        }
    }

    public void StopSpawning()
    {
        CancelInvoke(nameof(SpawnMeteorite));
        CancelInvoke(nameof(SpawnNoobEnemy));
        CancelInvoke(nameof(SpawnShootEnemy));
        CancelInvoke(nameof(SpawnPowerUps));
    }
}
