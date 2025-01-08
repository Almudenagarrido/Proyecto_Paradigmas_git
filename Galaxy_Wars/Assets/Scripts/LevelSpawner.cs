using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    public GameObject meteoritePrefab;
    public GameObject enemyNoobPrefab;
    public GameObject enemyShootPrefab;

    public float meteoriteFrequency = 1.0f;
    public float noobEnemyFrequency = 3.0f;
    public float shootEnemyFrequency = 5.0f;

    private bool spawnMeteorites = false;
    private bool spawnNoobEnemies = false;
    private bool spawnShootEnemies = false;

    public void ConfigureSpawner(float meteoriteFreq, float noobFreq, float shootFreq, bool spawnMeteor, bool spawnNoob, bool spawnShoot)
    {
        // Configurar frecuencias y estados de spawn
        meteoriteFrequency = meteoriteFreq;
        noobEnemyFrequency = noobFreq;
        shootEnemyFrequency = shootFreq;

        spawnMeteorites = spawnMeteor;
        spawnNoobEnemies = spawnNoob;
        spawnShootEnemies = spawnShoot;

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
    }

    private void SpawnMeteorite()
    {
        if (meteoritePrefab == null) return;

        Instantiate(meteoritePrefab);
    }

    private void SpawnNoobEnemy()
    {
        if (enemyNoobPrefab == null) return;

        Instantiate(enemyNoobPrefab);
    }

    private void SpawnShootEnemy()
    {
        if (enemyShootPrefab == null) return;

        Instantiate(enemyShootPrefab);
    }

    public void StopSpawning()
    {
        CancelInvoke(nameof(SpawnMeteorite));
        CancelInvoke(nameof(SpawnNoobEnemy));
        CancelInvoke(nameof(SpawnShootEnemy));
    }
}
