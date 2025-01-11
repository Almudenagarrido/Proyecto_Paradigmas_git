using UnityEngine;

public class EnemyShoot : Enemy
{
    public GameObject bulletPrefab;
    public Transform shootingPoint;
    public float bulletSpeed = 7f;
    public float timeBetweenBullets = 2f;
    public AudioClip explosionSound;
    public AudioClip shootingSound;
    private AudioSource audioSource;

    protected override void Start()
    {
        minVelocidad = 2f;
        maxVelocidad = 3.5f;
        base.Start();
        InvokeRepeating(nameof(Shoot), 1f, timeBetweenBullets);
        audioSource = GetComponent<AudioSource>();
    }

    private void Shoot()
    {
        if (player == null) return;

        GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation);
        Rigidbody2D rigidBullet = bullet.GetComponent<Rigidbody2D>();
        Vector2 direccion = ((Vector2)player.position - (Vector2)shootingPoint.position).normalized;
        rigidBullet.velocity = direccion * bulletSpeed;

        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        
        if (audioSource != null && shootingSound != null)
        {
            audioSource.PlayOneShot(shootingSound);
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("BulletPlayer"))
        {
            hits++;
            if (hits >= 2)
            {
                gameManager.AddPoints(1, "EnemyShoot");
                if (audioSource != null && explosionSound != null)
                {
                    audioSource.PlayOneShot(explosionSound);
                }
                Explode();
            }
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            if (gameManager.numberOfPlayers == 1)
            {
                gameManager.TakeLife(1, "EnemyShoot");
            }
            if (audioSource != null && explosionSound != null)
            {
                audioSource.PlayOneShot(explosionSound);
            }
            Explode();
        }
    }
}
