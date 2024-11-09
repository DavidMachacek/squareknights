using UnityEngine;

public class PlayerGridSetup : MonoBehaviour
{
    public GameObject tilePrefab; // Prefab for each tile (shooter)
    public int gridSize = 5; // Size of the grid (5x5)
    public float tileSpacing = 1f; // Spacing between tiles

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
                //tile.GetComponent<TileShooter>().Initialize(this);
            }
        }
    }
}
