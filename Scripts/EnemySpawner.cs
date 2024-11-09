using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab of the red triangle
    public float spawnInterval = 3f; // Time between spawns

    private float screenEdgeOffset = 1.5f;

    void Start()
    {
        // Start spawning enemies at regular intervals
        InvokeRepeating(nameof(SpawnEnemy), 1f, spawnInterval);
    }

    void SpawnEnemy()
    {
        // Get screen bounds in world space
        float screenWidth = Camera.main.orthographicSize * Camera.main.aspect;
        float screenHeight = Camera.main.orthographicSize;

        // Choose a random position on the screen's edge
        Vector2 spawnPosition = Random.value < 0.5f ?
            new Vector2(Random.Range(-screenWidth, screenWidth), Random.value > 0.5f ? screenHeight + screenEdgeOffset : -screenHeight - screenEdgeOffset) :
            new Vector2(Random.value > 0.5f ? screenWidth + screenEdgeOffset : -screenWidth - screenEdgeOffset, Random.Range(-screenHeight, screenHeight));

        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        enemy.GetComponent<Enemy>().MoveToCenter();
    }
}
