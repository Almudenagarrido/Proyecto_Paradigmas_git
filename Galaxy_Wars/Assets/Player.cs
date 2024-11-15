using UnityEngine;

public class Player : MonoBehaviour
{
    // Velocidad de la nave, ajustable desde el Inspector
    public float maxSpeed = 20f;
    private float acceleration = 8f;
    private float deceleration = 1f;
    public int playerNumber;
    private Vector2 velocity = Vector2.zero;
    public float rotationSpeed = 5f;

    void Update()
    {
        Vector2 direction = Vector2.zero;

        // Control de movimiento para el Jugador 1
        if (playerNumber == 1)
        {
            // Detectar movimiento en todas las direcciones
            if (Input.GetKey(KeyCode.RightArrow))
            {
                direction = Vector2.right;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                direction = Vector2.left;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                direction = Vector2.down;
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                direction = Vector2.up;
            }

            // Si hay dirección, aumentar velocidad hasta maxSpeed
            if (direction != Vector2.zero)
            {
                // Aumentar la velocidad en la dirección indicada
                velocity = Vector2.MoveTowards(velocity, direction * maxSpeed, acceleration * Time.deltaTime);
            }
            else // Si no hay entrada, desacelerar
            {
                velocity = Vector2.MoveTowards(velocity, Vector2.zero, deceleration * Time.deltaTime);
            }

            // Mover la nave
            transform.Translate(velocity * Time.deltaTime);
        }

        // Control de movimiento para el Jugador 2 (igual que el Jugador 1)
        else if (playerNumber == 2)
        {
            // Detectar movimiento en todas las direcciones (similar al Jugador 1)
            if (Input.GetKey(KeyCode.D))
            {
                direction = Vector2.right;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                direction = Vector2.left;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                direction = Vector2.down;
            }
            else if (Input.GetKey(KeyCode.W))
            {
                direction = Vector2.up;
            }

            // Si hay dirección, aumentar velocidad hasta maxSpeed
            if (direction != Vector2.zero)
            {
                // Aumentar la velocidad en la dirección indicada
                velocity = Vector2.MoveTowards(velocity, direction * maxSpeed, acceleration * Time.deltaTime);
            }
            else // Si no hay entrada, desacelerar
            {
                velocity = Vector2.MoveTowards(velocity, Vector2.zero, deceleration * Time.deltaTime);
            }

            // Mover la nave
            transform.Translate(velocity * Time.deltaTime);
        }
    }
}
