using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float detectionRange = 3f; // Range within which Orc detects the player
    public LayerMask playerLayer; // Layer of the player

    private GameObject player;

    public float moveSpeed = 2f;
    private float destroyDelay = 2f; // Delay before destroying the arrow after 5 seconds


    //HITPOINTS
    public float maxHitpoints = 100f; // Maximum health of the TileShooter
    private float currentHitpoints;
    public GameObject healthBarCanvas; // Reference to the health bar image (foreground)
    private Image healthBar; // Reference to the health bar image (foreground)
    private GameObject instantiatedHealthBar;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        MoveToCenter();

        // HITPOINTS
        currentHitpoints = maxHitpoints;
        instantiatedHealthBar = Instantiate(healthBarCanvas, this.transform.position + new Vector3(0, 1.0f, 0), Quaternion.identity, transform);
        Transform healthBarTransform = healthBarCanvas.transform;
        Transform greenBarTransform = healthBarTransform.Find("GreenHealthBar");  // Name of the green bar GameObject
        if (greenBarTransform != null)
        {
            Debug.Log("Green bar transform is not null");
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
        // HITPOINTS
        // Update the position of the health bar to follow the TileShooter's position
        if (healthBar != null)
        {
            // Update health bar's position based on TileShooter's position (with offset)
            instantiatedHealthBar.transform.position = Camera.main.WorldToScreenPoint(this.transform.position + new Vector3(0, 1.5f, 0)); // Position above the sprite
        }


        // Check if the player is within detection range
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            
            Animator animator = this.GetComponent<Animator>();
            // If the player is within the detection range, check for attack proximity
            if (distanceToPlayer <= detectionRange)
            {
                // Player is close enough to attack
                animator.SetBool("isAttacking", true);
            }
            else
            {
                // Player is within detection range but not close enough to attack
                animator.SetBool("isAttacking", false);
            }
        }
    }

    public void MoveToCenter()
    {
        Vector2 directionToCenter = Vector2.zero - (Vector2)transform.position;
        GetComponent<Rigidbody2D>().linearVelocity = directionToCenter.normalized * moveSpeed;
        Animator animator = this.GetComponent<Animator>();
        animator.SetBool("isIdle", false);
        animator.SetBool("isWalking", true);
    }

    // HITPOINTS
    public void TakeDamage(float damage)
    {   

        Debug.Log("Enemy taking damage, hitpoints left " + currentHitpoints);
        Animator animator = this.GetComponent<Animator>();
        animator.SetBool("isHurt", true);
        currentHitpoints -= damage;
        if (currentHitpoints <= 0)
        {
            Rigidbody2D rb = this.GetComponent<Rigidbody2D>();
            rb.freezeRotation = true; 
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("isDead", true);
            Destroy (this, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + destroyDelay); 
    
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