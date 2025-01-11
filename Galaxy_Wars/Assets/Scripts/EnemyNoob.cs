using UnityEngine;

public class EnemyNoob : Enemy
{
    protected override void Start()
    {
        minVelocidad = 1f;
        maxVelocidad = 3f;
        base.Start();
    }
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (isExploding) return;

        if (collision.gameObject.CompareTag("BulletPlayer"))
        {
            hits++;
            if (hits >= 2)
            {
                Destroy(collision.gameObject);
                isExploding = true;
                Explode();
                gameManager.AddPoints(1, "EnemyNoob");
            }
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            Explode();
        }
    }
}
