using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TileShooter : MonoBehaviour
{
    public GameObject arrowPrefab; // Prefab for the arrow
    public float shootInterval = 1f; // Time between shots
    public float detectionRange = 10f; // Distance to detect enemies

    private float lastShotTime;

    public void Initialize(Transform parent)
    {
        transform.parent = parent;
    }


    void Start()
    {
    }

    void Update()
    {
        // Find all enemies within range
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        Enemy closestEnemy = null;
        float closestDistance = detectionRange;

        foreach (Enemy enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                closestEnemy = enemy;
            }
        }

        // Shoot at the closest enemy if within range and ready to shoot
        if (closestEnemy != null && Time.time >= lastShotTime + shootInterval)
        {
            lastShotTime = Time.time;
            ShootAt(closestEnemy);
        }
        
    }

    void ShootAt(Enemy target)
    {

        Animator animator = this.GetComponent<Animator>();
        animator.SetBool("isIdle", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", true);
        // Aim and shoot an arrow at the target
        Vector2 direction = (target.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        GameObject arrow = Instantiate(arrowPrefab, transform.position, Quaternion.Euler(0, 0, angle));
        arrow.GetComponent<Rigidbody2D>().linearVelocity = direction * 10f; // Adjust speed as needed


        //animator.SetBool("isIdle", true);
        //animator.SetBool("isWalking", false);
        //animator.SetBool("isAttacking", false);
    }


}