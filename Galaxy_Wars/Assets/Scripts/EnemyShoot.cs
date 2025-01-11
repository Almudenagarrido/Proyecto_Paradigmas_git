using UnityEngine;

public class EnemyShoot : Enemy
{
    public GameObject bulletPrefab;
    public Transform shootingPoint;
    public float bulletSpeed = 7f;
    public float timeBetweenBullets = 2f;

    protected override void Start()
    {
        base.Start();
        InvokeRepeating(nameof(Shoot), 1f, timeBetweenBullets);
    }

    private void Shoot()
    {
        if (player == null) return;

        GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation);
        Rigidbody2D rigidBullet = bullet.GetComponent<Rigidbody2D>();
        Vector2 direccion = ((Vector2)player.position - (Vector2)shootingPoint.position).normalized;
        rigidBullet.velocity = direccion * bulletSpeed;

        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("BulletPlayer"))
        {
            hits++;
            if (hits >= 2)
            {
                gameManager.AddPoints(1, "EnemyShoot");
                Explode();
            }
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            if (gameManager.numberOfPlayers == 1)
            {
                gameManager.TakeLife(1, "EnemyShoot");
            }
            Explode();
        }
    }
}
