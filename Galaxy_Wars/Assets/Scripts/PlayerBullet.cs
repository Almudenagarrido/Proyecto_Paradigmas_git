using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    private Camera camara;

    void Start()
    {
        camara = Camera.main;
    }

    void Update()
    {
        // Destruir la bala si sale de los límites de la pantalla
        if (!EnPantalla())
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Si colisiona con un planeta, destruye la bala
        if (collision.gameObject.CompareTag("Planet"))
        {
            Destroy(gameObject);
        }
    }

    bool EnPantalla()
    {
        Vector3 posicionEnPantalla = camara.WorldToViewportPoint(transform.position);
        return posicionEnPantalla.x > -0.1f && posicionEnPantalla.x < 1.1f &&
               posicionEnPantalla.y > -0.1f && posicionEnPantalla.y < 1.1f;
    }
}
