using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TileShooter : MonoBehaviour
{
    public GameObject arrowPrefab; // Prefab for the arrow
    public float shootInterval = 1f; // Time between shots
    public float detectionRange = 10f; // Distance to detect enemies

    private float lastShotTime;

    //HITPOINTS
    public float maxHitpoints = 100f; // Maximum health of the TileShooter
    private float currentHitpoints;
    public GameObject healthBarCanvas; // Reference to the health bar image (foreground)
    private Image healthBar; // Reference to the health bar image (foreground)
    private GameObject instantiatedHealthBar;

    public void Initialize(Transform parent)
    {
        transform.parent = parent;
    }


    void Start()
    {
        // HITPOINTS
        // Initialize hitpoints
        currentHitpoints = maxHitpoints;
        // Create the health bar above the TileShooter
        instantiatedHealthBar = Instantiate(healthBarCanvas, transform.position + new Vector3(0, 1.0f, 0), Quaternion.identity, transform);
        // Assuming healthBarCanvas is the parent Canvas
        Transform healthBarTransform = healthBarCanvas.transform;
        // Find the green bar child by name (ensure that you have set the name in the Inspector)
        Transform greenBarTransform = healthBarTransform.Find("GreenHealthBar");  // Name of the green bar GameObject
        // Get the Image component of the green bar
        if (greenBarTransform != null)
        {
            Image greenBar = greenBarTransform.GetComponent<Image>();
            healthBar = greenBar;
        }
        // Make sure health bar UI is correctly positioned
        if (healthBar != null)
        {
            UpdateHealthBar();
        }
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


        // HITPOINTS
        // Update the position of the health bar to follow the TileShooter's position
        if (healthBar != null)
        {
            // Update health bar's position based on TileShooter's position (with offset)
            instantiatedHealthBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 1.5f, 0)); // Position above the sprite
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


    // Method to reduce hitpoints and update the health bar
    public void TakeDamage(float damage)
    {
        currentHitpoints -= damage;
        if (currentHitpoints <= 0)
        {
            Destroy(gameObject); // Destroy the object when health is 0 or below
        }
        else
        {
            UpdateHealthBar(); // Update the health bar display
        }
    }

    // Update the health bar UI
    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = currentHitpoints / maxHitpoints; // Adjust the fill amount based on current hitpoints
        }
    }
}