using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxSpeed = 10f;
    private float acceleration = 6f;
    private float deceleration = 3f;
    public int playerNumber;
    private float currentSpeed = 0f;
    private float rotationSpeed = 300f;
    private bool isDead = false;
    private Rigidbody2D rb;
    public Sprite[] smokeSprites;

    private float topMargin = 5.3f;
    private float bottomMargin = -5.35f;
    private float leftMargin = -10.4f;
    private float rightMargin = 10.4f;

    public GameObject bulletPrefab;
    public Transform shootingPoint;
    public float bulletSpeed = 7f;
    public float timeBetweenBullets = 0.15f;
    private float lastShot = 0f;

    public bool isAI = false;
    private Transform target;
    private float randomMoveTimer = 0f;
    private float randomDirection = 0f;
    public float maxSpeedAI = 4.5f;
    private float accelerationAI = 3f;
    private float rotationSpeedAI = 150f;

    public AudioClip shootingSound;
    public AudioClip explosionSound;
    private AudioSource audioSource;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }
    
    private void Update()
    {
        if (!isDead)
        {
            if (isAI)
            {
                HandleAIMovement();
                HandleAIShooting();
            }
            else
            {
                HandleMovement();
                HandleFrames();
                HandleShooting();
            }
        }
    }
    
    private void HandleMovement()
    {
        float rotation = 0f;

        // Control de movimiento para el Jugador 1
        if (playerNumber == 1)
        {
            // Control de rotaci�n con las flechas izquierda y derecha
            if (Input.GetKey(KeyCode.RightArrow))
            {
                rotation = -rotationSpeed * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                rotation = rotationSpeed * Time.deltaTime;
            }

            // Control de movimiento hacia adelante y atr�s con las flechas arriba y abajo
            if (Input.GetKey(KeyCode.UpArrow))
            {
                currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                currentSpeed = Mathf.MoveTowards(currentSpeed, -maxSpeed / 2f, acceleration * Time.deltaTime); // Hacia atr�s, con velocidad limitada
            }
            else
            {
                // Desacelerar gradualmente cuando no hay entrada
                currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * Time.deltaTime);
            }
        }

        // Control de movimiento para el Jugador 2
        else if (playerNumber == 2)
        {
            // Control de rotaci�n con las teclas A y D
            if (Input.GetKey(KeyCode.D))
            {
                rotation = -rotationSpeed * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                rotation = rotationSpeed * Time.deltaTime;
            }

            // Control de movimiento hacia adelante y atr�s con las teclas W y S
            if (Input.GetKey(KeyCode.W))
            {
                currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                currentSpeed = Mathf.MoveTowards(currentSpeed, -maxSpeed / 2f, acceleration * Time.deltaTime); // Hacia atr�s, con velocidad limitada
            }
            else
            {
                // Desacelerar gradualmente cuando no hay entrada
                currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * Time.deltaTime);
            }
        }

        // Aplicar la rotaci�n y movimiento sin afectar el Rigidbody (sin fuerzas externas)
        transform.Rotate(0f, 0f, rotation);
        transform.Translate(Vector3.up * currentSpeed * Time.deltaTime, Space.Self);
    }

    private void HandleRandomMovement()
    {
        if (randomMoveTimer <= 0f)
        {
            randomDirection = Random.Range(-rotationSpeedAI, rotationSpeedAI);
            randomMoveTimer = Random.Range(1f, 3f);
        }

        randomMoveTimer -= Time.deltaTime;

        transform.Rotate(0f, 0f, randomDirection * Time.deltaTime);
        currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeedAI / 2f, accelerationAI * Time.deltaTime);
        transform.Translate(Vector3.up * currentSpeed * Time.deltaTime, Space.Self);
    }

    private void FindTarget()
    {
        GameObject[] noobEnemies = GameObject.FindGameObjectsWithTag("EnemyNoob");
        GameObject[] shootEnemies = GameObject.FindGameObjectsWithTag("EnemyShoot");
        GameObject[] meteorites = GameObject.FindGameObjectsWithTag("Meteorite");

        List<GameObject> allEnemies = new List<GameObject>();
        allEnemies.AddRange(noobEnemies);
        allEnemies.AddRange(shootEnemies);
        allEnemies.AddRange(meteorites);

        float minDistance = float.MaxValue;

        // Busca al enemigo mas cercano
        foreach (GameObject enemy in allEnemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                target = enemy.transform;
            }
        }

        if (allEnemies.Count == 0)
        {
            target = null;
        }
    }

    private void HandleAIMovement()
    {
        if (target == null)
        {
            FindTarget();
        }

        if (target != null)
        {
            Vector2 direction = ((Vector2)target.position - (Vector2)transform.position).normalized;

            // Rotar hacia el objetivo
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            float rotationStep = rotationSpeedAI * Time.deltaTime;
            float newAngle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, angle - 90, rotationStep);
            transform.rotation = Quaternion.Euler(0, 0, newAngle);

            currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeedAI, accelerationAI * Time.deltaTime);
            transform.Translate(Vector3.up * currentSpeed * Time.deltaTime, Space.Self);
        }
        else
        {
            HandleRandomMovement();
        }

        HandleFrames();
    }

    private void HandleFrames()
    {
        Vector3 currentPosition = transform.position;

        if (currentPosition.y <= bottomMargin)
        {
            currentPosition.y = topMargin;
        }
        else if (currentPosition.y >= topMargin)
        {
            currentPosition.y = bottomMargin;
        }
        if (currentPosition.x <= leftMargin)
        {
            currentPosition.x = rightMargin;
        }
        else if (currentPosition.x >= rightMargin)
        {
            currentPosition.x = leftMargin;
        }

        transform.position = currentPosition;
    }

    private void HandleShooting()
    {
        if (playerNumber == 1)
        {
            // Detecta si la tecla de disparo (Espacio) est� presionada
            if (Input.GetKey(KeyCode.Space) && Time.time > lastShot + timeBetweenBullets)
            {
                Shoot();
                lastShot = Time.time; // Actualiza el tiempo del �ltimo disparo
            }
        }
        else if (playerNumber == 2)
        {
            // Detecta si la tecla de disparo (Q) est� presionada
            if (Input.GetKey(KeyCode.Q) && Time.time > lastShot + timeBetweenBullets)
            {
                Shoot();
                lastShot = Time.time; // Actualiza el tiempo del �ltimo disparo
            }
        }
    }

    private void HandleAIShooting()
    {
        if (target != null && Time.time > lastShot + timeBetweenBullets)
        {
            Shoot();
            lastShot = Time.time;
        }
    }

    private void Shoot()
    {
        // Instancia la bala en el punto de disparo
        GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation);
        Rigidbody2D rigidBullet = bullet.GetComponent<Rigidbody2D>();
        rigidBullet.velocity = shootingPoint.up * bulletSpeed;

        // Desactivar colisi�n entre la bala y el jugador
        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());

        // Reproducir el sonido de disparo
        if (audioSource != null && shootingSound != null)
        {
            audioSource.PlayOneShot(shootingSound);
        }

        Destroy(bullet, 4f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            GameManager.Instance.TakeLife(playerNumber, "EnemyBullet");
        }
        else if (collision.gameObject.CompareTag("PowerUp"))
        {
            Debug.Log($"Jugador {playerNumber} recogi� un PowerUp.");
        }
        else if (collision.gameObject.CompareTag("Planet"))
        {
            Planet planet = collision.gameObject.GetComponent<Planet>();
            if (planet != null)
            {
                // Concatenar "Planet" con el tipo del planeta
                string reason = "Planet" + planet.planetType.ToString();
                GameManager.Instance.TakeLife(playerNumber, reason);
                Debug.Log($"Impactaste un {reason}. Vida reducida.");
            }
        }
        else if (collision.gameObject.CompareTag("Meteorite"))
        {
            GameManager.Instance.TakeLife(playerNumber, "Meteorite");
        }
        else if (collision.gameObject.CompareTag("EnemyNoob"))
        {
            GameManager.Instance.TakeLife(playerNumber, "EnemyNoob");
        }
        else if (collision.gameObject.CompareTag("EnemyShoot"))
        {
            GameManager.Instance.TakeLife(playerNumber, "EnemyShoot");
        }
    }

    public void TriggerDeath()
    {
        if (!isDead)
        {
            isDead = true;
            if (audioSource != null && explosionSound != null)
            {
                audioSource.PlayOneShot(explosionSound);
            }
            StartCoroutine(EnhancedDeathAnimation());
        }
    }

    private IEnumerator EnhancedDeathAnimation()
    {
        Debug.Log($"Jugador {playerNumber} est� ejecutando su animaci�n de muerte.");

        GameObject redSmoke = CreateSmokeObject(smokeSprites[2], 0.3f, -0.1f);
        GameObject yellowSmoke = CreateSmokeObject(smokeSprites[1], 0.25f, -0.07f);
        GameObject whiteSmoke = CreateSmokeObject(smokeSprites[0], 0.2f, -0.05f);

        float totalDuration = 3f;
        float redDuration = 2.5f;
        float yellowDuration = 2.7f;
        float whiteDuration = 3f;
        float fadeStartTime = 2f;

        float elapsedTime = 0f;

        while (elapsedTime < totalDuration)
        {
            elapsedTime += Time.deltaTime;

            // Escalado progresivo
            if (elapsedTime < redDuration)
            {
                UpdateSmokeScale(redSmoke, elapsedTime / redDuration, 0.45f);
            }
            if (elapsedTime < yellowDuration)
            {
                UpdateSmokeScale(yellowSmoke, elapsedTime / yellowDuration, 0.5f);
            }
            UpdateSmokeScale(whiteSmoke, elapsedTime / whiteDuration, 0.75f);

            // Transparencia progresiva, comienza a los 2.5 segundos
            if (elapsedTime > fadeStartTime && elapsedTime <= redDuration)
            {
                UpdateSmokeTransparency(redSmoke, 1f - (elapsedTime - fadeStartTime) / (redDuration - fadeStartTime));
            }
            if (elapsedTime > fadeStartTime && elapsedTime <= yellowDuration)
            {
                UpdateSmokeTransparency(yellowSmoke, 1f - (elapsedTime - fadeStartTime) / (yellowDuration - fadeStartTime));
            }
            if (elapsedTime > fadeStartTime && elapsedTime <= whiteDuration)
            {
                UpdateSmokeTransparency(whiteSmoke, 1f - (elapsedTime - fadeStartTime) / (whiteDuration - fadeStartTime));
            }

            yield return null;
        }

        Destroy(redSmoke);
        Destroy(yellowSmoke);
        Destroy(whiteSmoke);

        GameManager.Instance.CheckGameOver();
        Destroy(gameObject);
    }

    private GameObject CreateSmokeObject(Sprite sprite, float initialScale, float zOffset)
    {
        GameObject smoke = new GameObject("Smoke");
        smoke.transform.position = transform.position + new Vector3(0, 0, zOffset);
        smoke.transform.localScale = new Vector3(initialScale, initialScale, 1);

        SpriteRenderer renderer = smoke.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        renderer.sortingOrder = 10;
        renderer.color = new Color(1f, 1f, 1f, 1f);

        return smoke;
    }

    private void UpdateSmokeScale(GameObject smoke, float progress, float maxScale)
    {
        float scale = Mathf.Lerp(smoke.transform.localScale.x, maxScale, progress);
        smoke.transform.localScale = new Vector3(scale, scale, 1);
    }

    private void UpdateSmokeTransparency(GameObject smoke, float alpha)
    {
        SpriteRenderer renderer = smoke.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            Color color = renderer.color;
            color.a = Mathf.Clamp01(alpha);
            renderer.color = color;
        }
    }

}
