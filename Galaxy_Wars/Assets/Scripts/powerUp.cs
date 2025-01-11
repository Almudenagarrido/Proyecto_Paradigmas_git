using UnityEngine;

public class powerUp : MonoBehaviour
{
    public enum PowerUpType { Shield, Points }
    public PowerUpType powerUpType;
    public float shieldDuration = 10f;
    public int pointsToAdd = 50;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                if (powerUpType == PowerUpType.Shield)
                {
                    GameManager.Instance.ActivateShield(player.playerNumber, shieldDuration);
                }
                else if (powerUpType == PowerUpType.Points)
                {
                    GameManager.Instance.AddPoints(player.playerNumber, "PowerUp");
                    GameManager.Instance.totalPoints += pointsToAdd;
                }
            }
            Destroy(gameObject);
        }
    }
}