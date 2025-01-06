using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxSpeed;
    private float acceleration = 6f;
    private float deceleration = 3f;
    public int playerNumber;
    private float currentSpeed = 0f;
    private float rotationSpeed = 300f;
    private bool isDead = false;

    private float topMargin = 5.3f;
    private float bottomMargin = -5.35f;
    private float leftMargin = -10.4f;
    private float rightMargin = 10.4f;

    private bool shooting = false;
    public GameObject bulletPrefab;
    public Transform shootingPoint;
    public float bulletSpeed = 7f;
    public float timeBetweenBullets = 0.2f;
    private float lastShot = 0f;

    private Rigidbody2D rb;

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

    public int GetPlayerNumber() { return playerNumber; }

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
            if (Input.GetKeyUp(KeyCode.Space))
            {
                shooting = true;
            }
            else
            {
                shooting = false;
            }
            if (shooting && Time.time > lastShot + timeBetweenBullets)
            {
                Shoot();
                lastShot = Time.time;
            }
        }
        else if (playerNumber == 2)
        {
            if (Input.GetKeyUp(KeyCode.Q))
            {
                shooting = true;
            }
            else
            {
                shooting = false;
            }
            if (shooting && Time.time > lastShot + timeBetweenBullets)
            {
                Shoot();
                lastShot = Time.time;
            }
        }
    }

    void Shoot()
    {
        // Instancia la bala en el punto de disparo
        GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation); // Mantiene la rotación del shootingPoint

        // Aplica velocidad hacia adelante (sin importar la rotación global de la nave)
        Rigidbody2D rigidBullet = bullet.GetComponent<Rigidbody2D>();

        // La velocidad de la bala siempre se aplica hacia adelante respecto al frente de la nave
        rigidBullet.velocity = shootingPoint.up * bulletSpeed;  // 'shootingPoint.up' está alineado al frente de la nave, sin importar la rotación global

        // Asegurarse de que la nave no se ve afectada por el retroceso
        rb.velocity = Vector2.zero;  // El retroceso no afecta a la nave
    }


    private void TriggerDeath()
    {
        if (!isDead)
        {
            isDead = true;
            DeathAnimation();
        }
    }

    private void DeathAnimation()
    {
        // Lógica para mostrar una animación de muerte (aún no implementada)
        // Esto podría ser una animación de partículas, cambio de sprite, etc.
        // Aquí solo mostramos un ejemplo simple de un mensaje de depuración.
        bool isDead = false; // Sustituye esto por la lógica de detección de muerte real

        if (isDead)
        {
            Debug.Log("Jugador " + playerNumber + " murió");
            // Implementa la animación de muerte o lógica adicional
        }
    }
}
