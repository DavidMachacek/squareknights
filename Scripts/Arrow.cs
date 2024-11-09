using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        // Check if the collided object is tagged as "Enemy"
        if (other.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();  // Get TileShooter component


            if (enemy != null)
            {
                enemy.TakeDamage(35f);  // Call the TakeDamage method of the enemy
            }
            else
            {
                // If the enemy doesn't have an Animator component, just destroy it immediately
                Destroy(gameObject);
                Destroy(other.gameObject);
            }
        }
    }
}