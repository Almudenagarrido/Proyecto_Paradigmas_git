using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public float minVelocidad = 2f;
    public float maxVelocidad = 5f;
    private Vector2 direccion;
    private float velocidad;
    private Camera camara;

    public GameObject bulletPrefab;
    public Transform shootingPoint;
    public float bulletSpeed = 7f;
    public float timeBetweenBullets = 2f;

    void Start()
    {
        camara = Camera.main;

        // Determinar desde qué borde aparece el enemigo
        int borde = Random.Range(0, 4);
        Vector3 puntoDeSalida = ObtenerPosicionBorde(borde);
        transform.position = puntoDeSalida;

        direccion = ObtenerDireccionMovimiento(borde);
        RotarHaciaDireccion();

        // Iniciar disparos
        InvokeRepeating(nameof(Shoot), 1f, timeBetweenBullets);
    }

    void Update()
    {
        velocidad = Random.Range(minVelocidad, maxVelocidad);
        transform.Translate(direccion * velocidad * Time.deltaTime);

        if (!EnPantalla())
        {
            Destroy(gameObject);
        }
    }

    void RotarHaciaDireccion()
    {
        transform.up = direccion;
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

    Vector2 ObtenerDireccionMovimiento(int borde)
    {
        return (Vector2.zero - (Vector2)transform.position).normalized; // Movimiento hacia el centro
    }

    bool EnPantalla()
    {
        Vector3 posicionEnPantalla = camara.WorldToViewportPoint(transform.position);
        return posicionEnPantalla.x > -0.1f && posicionEnPantalla.x < 1.1f &&
               posicionEnPantalla.y > -0.1f && posicionEnPantalla.y < 1.1f;
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation);
        Rigidbody2D rigidBullet = bullet.GetComponent<Rigidbody2D>();
        rigidBullet.velocity = shootingPoint.up * bulletSpeed;

        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }
}
