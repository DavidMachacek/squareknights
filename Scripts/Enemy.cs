using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float detectionRange = 3f; // Range within which Orc detects the player
    public LayerMask playerLayer; // Layer of the player

    private GameObject player;

    public float moveSpeed = 2f;
    private float destroyDelay = 2f; // Delay before destroying the arrow after 5 seconds


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {

        player = GameObject.FindGameObjectWithTag("Player");

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
}
