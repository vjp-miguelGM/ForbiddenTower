using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;     // prefab del enemigo
    public float spawnInterval = 3f;   // segundos entre cada enemigo
    public int maxEnemies = 10;        // total de enemigos que aparecerán (0 = infinitos)
    public bool spawnOnStart = true;   // si empieza a generar nada más dar a Play

    private float timer;
    private int spawnedCount;

    private void Start()
    {
        // Genera uno al empezar y luego sigue el temporizador
        if (spawnOnStart)
        {
            SpawnEnemy();
        }
        timer = spawnInterval;
    }

    private void Update()
    {
        if (maxEnemies > 0 && spawnedCount >= maxEnemies) return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            SpawnEnemy();
            timer = spawnInterval;
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogWarning("EnemySpawner: falta asignar el prefab del enemigo");
            return;
        }

        Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        spawnedCount++;
    }

    // Dibuja un círculo en la Scene para ver dónde está el spawn
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}