using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // INIT PART
    public GameObject tilePrefab; // Prefab for each tile (shooter)
    public int gridSize = 5; // Size of the grid (5x5)
    public float tileSpacing = 0.3f; // Spacing between tiles
    public float movementSpeed = 5.0f; // Speed of movement
    public float rotationSpeed = 200.0f; // Rotation speed

    private GameObject[,] playerTiles = new GameObject[5, 5]; // 5x5 grid for player tiles
    private Vector2 gridVelocity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Create a 5x5 grid of tiles
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                Vector2 tilePosition = new Vector2(
                    transform.position.x + (x - (gridSize - 1) / 2f) * tileSpacing,
                    transform.position.y + (y - (gridSize - 1) / 2f) * tileSpacing
                );
                GameObject tile = Instantiate(tilePrefab, tilePosition, Quaternion.identity, transform);
                tile.GetComponent<Animator>().SetBool("isIdle", true);
                playerTiles[x, y] = tile;  // Parent them to the player object
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Movement direction based on WASD keys, independent of rotation
        if (Input.GetKey(KeyCode.W))
        {
            gridVelocity = Vector2.up * movementSpeed;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            gridVelocity = Vector2.down * movementSpeed;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            gridVelocity = Vector2.left * movementSpeed;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            gridVelocity = Vector2.right * movementSpeed;
        }
        else
        {
            gridVelocity = Vector2.zero;
        }

        // Move the entire grid
        MoveGrid();

        // Apply movement to each tile
        MoveTiles();

        // Handle rotation input (Q/E keys)
        if (Input.GetKey(KeyCode.Q))
        {
            // Rotate counterclockwise
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            // Rotate clockwise
            transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
        }
        
    }

    // Move the entire grid based on WASD input
    private void MoveGrid()
    {
        transform.position += (Vector3)gridVelocity * Time.deltaTime;
    }

    // Move tiles independently but maintain the square formation relative to the grid
    private void MoveTiles()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                Rigidbody2D rb = playerTiles[x, y].GetComponent<Rigidbody2D>();
                Animator animator = playerTiles[x, y].GetComponent<Animator>();

                if (rb != null)
                {
                    animator.SetBool("isIdle", false);
                    animator.SetBool("isWalking", true);

                    // Tiles move with the grid, but maintain relative position
                    Vector2 targetTilePosition = new Vector2(
                        (x - (gridSize - 1) / 2f) * tileSpacing,
                        (y - (gridSize - 1) / 2f) * tileSpacing
                    );

                    // Apply the movement velocity to the tile
                    rb.linearVelocity = gridVelocity;
                }
                else
                {
                    animator.SetBool("isWalking", false);
                    animator.SetBool("isIdle", true);
                }
            }
        }
    
        // Handle rotation input (Q/E keys)
        if (Input.GetKey(KeyCode.Q))
        {
            // Rotate counterclockwise
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            // Rotate clockwise
            transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
        }

        // Maintain the square formation
        MaintainGridFormation();

    }

    // Maintain the square grid formation after movement and rotation
    private void MaintainGridFormation()
    {
        // Iterate over each tile and correct its position based on the grid
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                // Calculate the desired local position of each tile based on the grid
                Vector2 targetLocalPosition = new Vector2(
                    (x - (gridSize - 1) / 2f) * tileSpacing,
                    (y - (gridSize - 1) / 2f) * tileSpacing
                );

                // Apply rotation to the target local position to maintain the rotation of the grid
                Vector2 rotatedPosition = RotatePoint(targetLocalPosition, transform.eulerAngles.z);

                // Calculate the world position of the tile based on the parent's position and rotation
                Vector2 targetWorldPosition = (Vector2)transform.position + rotatedPosition;


                //Debug.Log("Original x " + transform.position.x + ", Original y: " + transform.position.y);

                // Calculate the world position of the tile based on the parent's position and rotation
                // Move the tile towards its target position to keep the formation
                Rigidbody2D rb = playerTiles[x, y].GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    Vector2 direction = targetWorldPosition - (Vector2)playerTiles[x, y].transform.position;
                    float distance = direction.magnitude;

                    // Only apply correction if the tile is too far from its target position
                    if (distance > tileSpacing * 0.1f) // Threshold to prevent jittering
                    {
                        // Apply force to maintain distance
                        rb.AddForce(direction.normalized * movementSpeed * 2f); // Force towards target position
                    }
                }
            }
        }
    }

    // Helper function to rotate a point around the origin (0,0) by an angle in degrees
    private Vector2 RotatePoint(Vector2 point, float angle)
    {
        float radians = angle * Mathf.Deg2Rad;
        float cosAngle = Mathf.Cos(radians);
        float sinAngle = Mathf.Sin(radians);

        float xNew = point.x * cosAngle - point.y * sinAngle;
        float yNew = point.x * sinAngle + point.y * cosAngle;

        return new Vector2(xNew, yNew);
    }
}
