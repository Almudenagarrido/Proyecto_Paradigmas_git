using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public float minVelocidad = 2f;
    public float maxVelocidad = 5f;
    private float velocidad;
    private Camera camara;

    public GameObject bulletPrefab;
    public Transform shootingPoint;
    public float bulletSpeed = 7f;
    public float timeBetweenBullets = 2f;

    private Transform player;

    void Start()
    {
        camara = Camera.main;

        // Determinar desde qué borde aparece el enemigo
        int borde = Random.Range(0, 4);
        Vector3 puntoDeSalida = ObtenerPosicionBorde(borde);
        transform.position = puntoDeSalida;

        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        InvokeRepeating(nameof(Shoot), 1f, timeBetweenBullets);
    }

    void Update()
    {
        velocidad = Random.Range(minVelocidad, maxVelocidad);

        if (player != null)
        {
            Vector2 direccion = ((Vector2)player.position - (Vector2)transform.position).normalized;
            transform.Translate(direccion * velocidad * Time.deltaTime);
            RotarHaciaDireccion(direccion);
        }

        if (!EnPantalla())
        {
            Destroy(gameObject);
        }
    }

    void RotarHaciaDireccion(Vector2 direccion)
    {
        float angle = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90)); // Ajustar rotación hacia la dirección
    }

    Vector3 ObtenerPosicionBorde(int borde)
    {
        float bordeExtra = 1f;
        float anchoPantalla = camara.aspect * camara.orthographicSize;
        float altoPantalla = camara.orthographicSize;

        switch (borde)
        {
            case 0: return new Vector3(Random.Range(-anchoPantalla, anchoPantalla), altoPantalla + bordeExtra, 0);
            case 1: return new Vector3(Random.Range(-anchoPantalla, anchoPantalla), -altoPantalla - bordeExtra, 0);
            case 2: return new Vector3(-anchoPantalla - bordeExtra, Random.Range(-altoPantalla, altoPantalla), 0);
            case 3: return new Vector3(anchoPantalla + bordeExtra, Random.Range(-altoPantalla, altoPantalla), 0);
            default: return Vector3.zero;
        }
    }

    bool EnPantalla()
    {
        Vector3 posicionEnPantalla = camara.WorldToViewportPoint(transform.position);
        return posicionEnPantalla.x > -0.1f && posicionEnPantalla.x < 1.1f &&
               posicionEnPantalla.y > -0.1f && posicionEnPantalla.y < 1.1f;
    }

    void Shoot()
    {
        if (player == null) return;

        GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation);
        Rigidbody2D rigidBullet = bullet.GetComponent<Rigidbody2D>();

        // Calcular la dirección hacia el jugador
        Vector2 direccion = ((Vector2)player.position - (Vector2)shootingPoint.position).normalized;
        rigidBullet.velocity = direccion * bulletSpeed;

        // Ignorar colisión entre el proyectil y el enemigo
        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }
}
