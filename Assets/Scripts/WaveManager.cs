using System.Collections;
using UnityEngine;
using TMPro;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }

    [Header("Rondas")]
    public bool infiniteWaves = true;       // true = rondas sin fin
    public int totalWaves = 3;              // solo se usa si infiniteWaves es false
    public int baseEnemiesPerSpawner = 3;   // ronda 1
    public int increasePerWave = 2;         // enemigos extra por ronda y spawner
    public int maxEnemiesPerSpawner = 15;   // tope para que no se dispare (0 = sin tope)
    public float timeBeforeFirstWave = 2f;

    [Header("Entre rondas")]
    public float timeInTowerToStart = 3f;   // (ya no se usa, se puede dejar)

    [Header("Mejoras")]
    public UpgradePanel upgradePanel;       // arrastra aquí el HUD de mejoras

    [Header("HUD")]
    public TMP_Text objectiveText;          // arrastra aquí VolverTorre
    public TMP_Text enemyCountText;         // arrastra aquí el texto de EnemyCount

    public int CurrentWave { get; private set; }

    // Rondas completadas: si la torre cae en la ronda 4, has superado 3
    public int WavesSurvived => Mathf.Max(0, CurrentWave - 1);

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
        if (enemyCountText != null) enemyCountText.text = "0";
        yield return new WaitForSeconds(timeBeforeFirstWave);

        CurrentWave = 1;
        while (infiniteWaves || CurrentWave <= totalWaves)
        {
            StartWave();

            // La ronda acaba cuando han salido todos y no queda ninguno vivo
            yield return new WaitUntil(() => pendingToSpawn <= 0 && alive <= 0);

            // Solo en modo finito: la última ronda termina el juego
            if (!infiniteWaves && CurrentWave == totalWaves)
            {
                ShowObjective(""); // última ronda: sin mensaje
                break;
            }

            Debug.Log("<color=green>Ronda " + CurrentWave + " superada. Ve a la torre.</color>");
            ShowObjective("Vuelve a la torre");

            // 1) Esperar a que el player llegue a la torre
            yield return new WaitUntil(() => towerZone != null && towerZone.PlayerInside);

            // 2) Mostrar el panel de mejoras y esperar a que elija una
            ShowObjective("");
            if (upgradePanel != null)
            {
                bool chosen = false;
                upgradePanel.Show(index =>
                {
                    Debug.Log("Mejora elegida: " + index);
                    chosen = true;
                });
                yield return new WaitUntil(() => chosen);
            }

            CurrentWave++;
        }

        Debug.Log("<color=yellow>¡VICTORIA! Todas las rondas superadas</color>");
        // Aquí se puede mostrar un HUD de victoria
    }

    private void StartWave()
    {
        int perSpawner = baseEnemiesPerSpawner + (CurrentWave - 1) * increasePerWave;
        if (maxEnemiesPerSpawner > 0) perSpawner = Mathf.Min(perSpawner, maxEnemiesPerSpawner);

        pendingToSpawn = perSpawner * spawners.Length;
        UpdateEnemyCount(); // muestra el total de la horda al empezar

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

    private void UpdateEnemyCount()
    {
        if (enemyCountText == null) return;
        enemyCountText.text = (alive + pendingToSpawn).ToString();
    }

    // Lo llama el spawner al crear un enemigo
    public void EnemySpawned()
    {
        pendingToSpawn--;
        alive++;
        // El total (vivos + pendientes) no cambia al salir un enemigo,
        // así que no hace falta actualizar el contador aquí
    }

    // Lo llama EnemyHealth al morir
    public void EnemyDied()
    {
        alive--;
        UpdateEnemyCount(); // resta uno por cada enemigo muerto
    }
}