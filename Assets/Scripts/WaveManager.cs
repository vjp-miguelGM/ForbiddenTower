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

    [Header("Mejoras")]
    public UpgradePanel upgradePanel;       // arrastra aquí el HUD de mejoras

    [Header("Sin recompensa")]
    public float skipDifficultyBonus = 0.5f;   // +50% enemigos la ronda siguiente

    [Header("HUD")]
    public TMP_Text objectiveText;          // arrastra aquí VolverTorre
    public TMP_Text enemyCountText;         // arrastra aquí el texto de EnemyCount

    public int CurrentWave { get; private set; }

    // Rondas completadas: si la torre cae en la ronda 4, has superado 3
    public int WavesSurvived => Mathf.Max(0, CurrentWave - 1);

    public int EnemiesKilled { get; private set; }

    public EnemySpawner[] spawners;

    private int alive;            // enemigos vivos
    private int pendingToSpawn;   // enemigos que faltan por salir en esta ronda
    private bool nextWaveHarder;
    private TowerZone towerZone;
    private Tower tower;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // Recoge todos los spawns de la escena, da igual cómo se llamen
        spawners = FindObjectsByType<EnemySpawner>(FindObjectsSortMode.None);
        towerZone = FindFirstObjectByType<TowerZone>();
        tower = FindFirstObjectByType<Tower>();

        if (spawners.Length == 0) Debug.LogWarning("WaveManager: no hay EnemySpawner en la escena");
        if (towerZone == null) Debug.LogWarning("WaveManager: falta el script TowerZone en la torre");
        if (tower == null) Debug.LogWarning("WaveManager: no hay ninguna Tower en la escena");

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

            // Si la torre cayó mientras tanto, no seguimos (sale el Game Over)
            if (tower != null && tower.IsDestroyed) yield break;

            // Solo en modo finito: la última ronda termina el juego
            if (!infiniteWaves && CurrentWave == totalWaves)
            {
                ShowObjective("");
                break;
            }

            Debug.Log("<color=green>Ronda " + CurrentWave + " superada. Ve a la torre.</color>");
            ShowObjective("Vuelve a la torre");

            // 1) Esperar a que el player llegue a la torre
            yield return new WaitUntil(() => towerZone != null && towerZone.PlayerInside);

            if (tower != null && tower.IsDestroyed) yield break;

            // 2) Mostrar el panel de mejoras y esperar a que elija una (o salte la recompensa)
            ShowObjective("");
            if (upgradePanel != null)
            {
                bool chosen = false;
                upgradePanel.Show(index =>
                {
                    nextWaveHarder = (index < 0); // -1 = ha saltado la recompensa
                    Debug.Log(index < 0 ? "Sin recompensa: próxima ronda más dura" : "Mejora elegida: " + index);
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

        if (nextWaveHarder)
        {
            perSpawner = Mathf.CeilToInt(perSpawner * (1f + skipDifficultyBonus));
            nextWaveHarder = false;
        }

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
        EnemiesKilled++;
        UpdateEnemyCount(); // resta uno por cada enemigo muerto
    }
}