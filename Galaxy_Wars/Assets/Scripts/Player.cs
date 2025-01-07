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

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!isDead)
        {
            HandleMovement();
            HandleFrames();
            HandleShooting();
        }
    }

    private void HandleMovement()
    {
        float rotation = 0f;

        // Control de movimiento para el Jugador 1
        if (playerNumber == 1)
        {
            // Control de rotación con las flechas izquierda y derecha
            if (Input.GetKey(KeyCode.RightArrow))
            {
                rotation = -rotationSpeed * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                rotation = rotationSpeed * Time.deltaTime;
            }

            // Control de movimiento hacia adelante y atrás con las flechas arriba y abajo
            if (Input.GetKey(KeyCode.UpArrow))
            {
                currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                currentSpeed = Mathf.MoveTowards(currentSpeed, -maxSpeed / 2f, acceleration * Time.deltaTime); // Hacia atrás, con velocidad limitada
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
            // Control de rotación con las teclas A y D
            if (Input.GetKey(KeyCode.D))
            {
                rotation = -rotationSpeed * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                rotation = rotationSpeed * Time.deltaTime;
            }

            // Control de movimiento hacia adelante y atrás con las teclas W y S
            if (Input.GetKey(KeyCode.W))
            {
                currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                currentSpeed = Mathf.MoveTowards(currentSpeed, -maxSpeed / 2f, acceleration * Time.deltaTime); // Hacia atrás, con velocidad limitada
            }
            else
            {
                // Desacelerar gradualmente cuando no hay entrada
                currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * Time.deltaTime);
            }
        }

        // Aplicar la rotación y movimiento sin afectar el Rigidbody (sin fuerzas externas)
        transform.Rotate(0f, 0f, rotation);
        transform.Translate(Vector3.up * currentSpeed * Time.deltaTime, Space.Self);
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
            // Detecta si la tecla de disparo (Espacio) está presionada
            if (Input.GetKey(KeyCode.Space) && Time.time > lastShot + timeBetweenBullets)
            {
                Shoot();
                lastShot = Time.time; // Actualiza el tiempo del último disparo
            }
        }
        else if (playerNumber == 2)
        {
            // Detecta si la tecla de disparo (Q) está presionada
            if (Input.GetKey(KeyCode.Q) && Time.time > lastShot + timeBetweenBullets)
            {
                Shoot();
                lastShot = Time.time; // Actualiza el tiempo del último disparo
            }
        }
    }

    void Shoot()
    {
        // Instancia la bala en el punto de disparo
        GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation);

        // Configurar la velocidad de la bala
        Rigidbody2D rigidBullet = bullet.GetComponent<Rigidbody2D>();
        rigidBullet.velocity = shootingPoint.up * bulletSpeed;

        // Desactivar colisión entre la bala y el jugador
        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }

    // Para sumar o restar vida segun con lo que colisione
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            GameManager.Instance.TakeLife(playerNumber, "EnemyBullet");
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
    }

    public void TriggerDeath()
    {
        if (!isDead)
        {
            isDead = true;
            StartCoroutine(EnhancedDeathAnimation());
        }
    }

    private IEnumerator EnhancedDeathAnimation()
    {
        Debug.Log($"Jugador {playerNumber} está ejecutando su animación de muerte.");

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
