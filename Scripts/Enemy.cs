using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float detectionRange = 3f; // Range within which Orc detects the player
    public LayerMask playerLayer; // Layer of the player

    private GameObject player;

    public float moveSpeed = 2f;
    private float destroyDelay = 2f; // Delay before destroying the arrow after 5 seconds

    // HITPOINTS
    public float maxHitpoints = 100f; // Maximum health of the TileShooter
    private float currentHitpoints;
    public GameObject healthBarCanvas; // Reference to the health bar image (foreground)
    private Image healthBar; // Reference to the health bar image (foreground)
    private GameObject instantiatedHealthBar;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        // HITPOINTS INITIALIZATION
        currentHitpoints = maxHitpoints;
        instantiatedHealthBar = Instantiate(healthBarCanvas, this.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity, transform);

        // Ensure Canvas is in World Space
        Canvas healthBarCanvasComponent = instantiatedHealthBar.GetComponent<Canvas>();
        if (healthBarCanvasComponent != null)
        {
            healthBarCanvasComponent.renderMode = RenderMode.WorldSpace; // Set Canvas to World Space
        }

        // Find the health bar image (Green Health Bar)
        Transform healthBarTransform = instantiatedHealthBar.transform;
        healthBar = healthBarTransform.Find("GreenHealthBar").GetComponent<Image>();  // Name of the green bar GameObject

        if (healthBar != null)
        {
            UpdateHealthBar();
        }
        // END HITPOINTS INITIALIZATION
    }

    void Update()
    {

        player = GameObject.FindGameObjectWithTag("Player");
        // HITPOINTS UPDATE
        if (healthBar != null)
        {
            // Convert world position of the enemy to screen position
            Vector3 worldPosition = this.transform.position + new Vector3(0, 0.5f, 0); // Position above the sprite
            instantiatedHealthBar.transform.position = worldPosition; // Set the health bar's world position

            // Make the health bar face the camera, preventing rotation
            Vector3 lookAtPosition = Camera.main.transform.position;
            lookAtPosition.y = instantiatedHealthBar.transform.position.y;  // Keep health bar upright by ignoring Y axis
            instantiatedHealthBar.transform.LookAt(lookAtPosition);
        }
        // END OF HITPOINTS UPDATE

        // Move towards player
        MoveTowardsPlayer();

        // Check if the player is within detection range
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            //Debug.Log("Distance to player: " + distanceToPlayer);
            Animator animator = this.GetComponent<Animator>();

            // If the player is within the detection range, check for attack proximity
            if (distanceToPlayer <= detectionRange)
            {
                animator.SetBool("isAttacking", true);
                // Get all components attached to this GameObject
                Component[] allComponents = player.GetComponents<Component>();

                // Log each component type
                foreach (Component component in allComponents)
                {
                    Debug.Log("Component attached: " + component.GetType().Name);
                }
                player.GetComponent<HealthManager>().TakeDamage(1f); // Attack the player
            }
            else
            {
                animator.SetBool("isAttacking", false);
            }
        }
    }

    // Method to move towards the player instead of the center
    public void MoveTowardsPlayer()
    {
        // Ensure the enemy moves towards the player
        if (player != null)
        {
            Vector2 directionToPlayer = (player.transform.position - transform.position).normalized;
            GetComponent<Rigidbody2D>().linearVelocity = directionToPlayer * moveSpeed;

            Animator animator = this.GetComponent<Animator>();
            animator.SetBool("isIdle", false);
            animator.SetBool("isWalking", true);
        }
    }

    // HITPOINTS
    public void TakeDamage(float damage)
    {
        //Debug.Log("Enemy taking damage, hitpoints left " + currentHitpoints);
        Animator animator = this.GetComponent<Animator>();
        animator.SetBool("isHurt", true);
        currentHitpoints -= damage;

        if (currentHitpoints <= 0)
        {
            Rigidbody2D rb = this.GetComponent<Rigidbody2D>();
            rb.freezeRotation = true;
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("isDead", true);
            Destroy(this, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + destroyDelay);
            Destroy(gameObject); // Destroy the object when health is 0 or below
        }
        else
        {
            UpdateHealthBar(); // Update the health bar display
        }
    }

    // HITPOINTS
    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = currentHitpoints / maxHitpoints; // Adjust the fill amount based on current hitpoints
        }
    }
}
