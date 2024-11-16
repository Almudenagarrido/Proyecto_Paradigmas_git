using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxSpeed = 20f;
    private float acceleration = 8f;
    private float deceleration = 1f;
    public int playerNumber;
    private float currentSpeed = 0f;
    private float rotationSpeed = 300f;

    void Update()
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

        // Control de movimiento para el Jugador 2 (igual que el Jugador 1)
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

        // Aplicar la rotación al jugador que corresponda
        transform.Rotate(0f, 0f, rotation);

        // Aplicar el movimiento hacia adelante o atrás en la dirección actual
        transform.Translate(Vector3.up * currentSpeed * Time.deltaTime, Space.Self);
    }
}
