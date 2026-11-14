using System.Collections;
using UnityEngine;
using TMPro;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }

    [Header("Rondas")]
    public int totalWaves = 3;
    public int baseEnemiesPerSpawner = 3;   // ronda 1
    public int increasePerWave = 2;         // enemigos extra por ronda y spawner
    public float timeBeforeFirstWave = 2f;

    [Header("Entre rondas")]
    public float timeInTowerToStart = 3f;   // segundos que el player debe estar en la torre

    [Header("HUD")]
    public TMP_Text objectiveText;          // arrastra aquí VolverTorre

    public int CurrentWave { get; private set; }

    private int alive;            // enemigos vivos
    private int pendingToSpawn;   // enemigos que faltan por salir en esta ronda
    public EnemySpawner[] spawners;
    private TowerZone towerZone;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // Recoge todos los spawns de la escena, da igual cómo se llamen
        spawners = FindObjectsByType<EnemySpawner>(FindObjectsSortMode.None);
        towerZone = FindFirstObjectByType<TowerZone>();

        if (spawners.Length == 0) Debug.LogWarning("WaveManager: no hay EnemySpawner en la escena");
        if (towerZone == null) Debug.LogWarning("WaveManager: falta el script TowerZone en la torre");

        StartCoroutine(WaveLoop());
    }

    private IEnumerator WaveLoop()
    {
        ShowObjective(""); // oculto hasta que empiece la primera ronda
        yield return new WaitForSeconds(timeBeforeFirstWave);

        for (CurrentWave = 1; CurrentWave <= totalWaves; CurrentWave++)
        {
            StartWave();

            // La ronda acaba cuando han salido todos y no queda ninguno vivo
            yield return new WaitUntil(() => pendingToSpawn <= 0 && alive <= 0);

            if (CurrentWave == totalWaves)
            {
                ShowObjective(""); // última ronda: sin mensaje
                break;
            }

            Debug.Log("<color=green>Ronda " + CurrentWave + " superada. Ve a la torre.</color>");
            ShowObjective("Vuelve a la torre");

            // El player debe estar en la torre 3 s seguidos; si sale, se reinicia la cuenta
            float timer = 0f;
            while (timer < timeInTowerToStart)
            {
                if (towerZone != null && towerZone.PlayerInside) timer += Time.deltaTime;
                else timer = 0f;
                yield return null;
            }
        }

        Debug.Log("<color=yellow>¡VICTORIA! Todas las rondas superadas</color>");
        // Aquí se puede mostrar un HUD de victoria
    }

    private void StartWave()
    {
        int perSpawner = baseEnemiesPerSpawner + (CurrentWave - 1) * increasePerWave;
        pendingToSpawn = perSpawner * spawners.Length;

        Debug.Log("<color=cyan>Ronda " + CurrentWave + ": " + perSpawner + " enemigos por spawner</color>");

        ShowObjective("Acaba con los pecadores");

        foreach (EnemySpawner spawner in spawners)
        {
            spawner.StartWave(perSpawner);
        }
    }

    private void ShowObjective(string message)
    {
        if (objectiveText == null) return;

        objectiveText.gameObject.SetActive(!string.IsNullOrEmpty(message));
        objectiveText.text = message;
    }

    // Lo llama el spawner al crear un enemigo
    public void EnemySpawned()
    {
        pendingToSpawn--;
        alive++;
    }

    // Lo llama EnemyHealth al morir
    public void EnemyDied()
    {
        alive--;
    }
}