using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxSpeed = 20f;
    private float acceleration = 8f;
    private float deceleration = 1f;
    public int playerNumber;
    private float currentSpeed = 0f;
    private float rotationSpeed = 300f;
    private bool isDead = false;
    private float topMargin = 5.3f;
    private float bottomMargin = -5.35f;
    private float leftMargin = -10.4f;
    private float rightMargin = 10.4f;
    private float blackhole1x = 8.1f;
    private float blackhole1y = -3.5f;
    private float blackhole2x = -8.1f;
    private float blackhole2y = 3.5f;

    private void Update()
    {
        if (!isDead)
        {
            HandleMovement();
            HandleShooting();
            HandleFrames();
            HandleBlackholes();
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

        // Aplicar la rotación y movimiento
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

    private void HandleBlackholes()
    {
        Vector3 currentPosition = transform.position;

        float tolerance = 0.01f;

        if (Mathf.Abs(currentPosition.x - blackhole1x) < tolerance && Mathf.Abs(currentPosition.y - blackhole1y) < tolerance)
        {
            currentPosition.x = blackhole2x;
            currentPosition.y = blackhole2y;
        }
        else if (Mathf.Abs(currentPosition.x - blackhole2x) < tolerance && Mathf.Abs(currentPosition.y - blackhole2y) < tolerance)
        {
            currentPosition.x = blackhole1x;
            currentPosition.y = blackhole1y;
        }

        transform.position = currentPosition;
    }

    private void HandleShooting()
    {
        // Lógica de disparo (aún no implementada)
        // Puedes agregar aquí la detección de entrada y la creación de proyectiles.
        
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
