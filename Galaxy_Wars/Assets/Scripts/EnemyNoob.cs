using UnityEngine;

public class EnemyNoob : Enemy
{
    public AudioClip explosionSound;
    private AudioSource audioSource;

    protected override void Start()
    {
        minVelocidad = 1f;
        maxVelocidad = 3f;
        base.Start();
        audioSource = GetComponent<AudioSource>();
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
                if (audioSource != null && explosionSound != null)
                {
                    audioSource.PlayOneShot(explosionSound);
                }
                Explode();
                gameManager.AddPoints(1, "EnemyNoob");
            }
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            if (audioSource != null && explosionSound != null)
            {
                audioSource.PlayOneShot(explosionSound);
            }
            Explode();
        }
    }
}
