using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 1.5f; // segundos entre enemigos dentro de una ronda

    public void adStartWave(int count)
    {
        StartCoroutine(SpawnRoutine(count));
    }

    private IEnumerator SpawnRoutine(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (enemyPrefab != null)
            {
                Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("EnemySpawner: falta asignar el prefab del enemigo");
            }

            // Se cuenta siempre, para que la ronda no se quede bloqueada
            if (WaveManager.Instance != null) WaveManager.Instance.EnemySpawned();

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }

    public void StartWave(int perSpawner)
    {
        adStartWave(perSpawner);
    }
}