using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour {


    // HITPOINTS
    public float maxHitpoints = 100f; // Maximum health of the TileShooter
    private float currentHitpoints;
    public GameObject healthBarCanvas; // Reference to the health bar image (foreground)
    private Image healthBar; // Reference to the health bar image (foreground)
    private GameObject instantiatedHealthBar;
    private float destroyDelay = 2f; // Delay before destroying the arrow after 5 seconds

    public GameObject toBeDestroyed;

    void Start()
    {
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
            Debug.Log("Health bar found");
            UpdateHealthBar();
        }
        // END HITPOINTS INITIALIZATION
    }
    void Update()
    {
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

    }

    // HITPOINTS
    public void TakeDamage(float damage)
    {
        Debug.Log("Archer taking damage, hitpoints left " + currentHitpoints);
        Animator animator = toBeDestroyed.GetComponent<Animator>();
        animator.SetBool("isHurt", true);
        currentHitpoints -= damage;

        if (currentHitpoints <= 0)
        {
            Rigidbody2D rb = toBeDestroyed.GetComponent<Rigidbody2D>();
            rb.freezeRotation = true;
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("isDead", true);
            Destroy(toBeDestroyed, animator.GetCurrentAnimatorStateInfo(0).length + destroyDelay);
            //Destroy(toBeDestroyed); // Destroy the object when health is 0 or below
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