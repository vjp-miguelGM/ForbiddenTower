using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }

    public GameObject pausePanel;   // arrastra aquí el HUD de pausa de tu compañero

    public bool IsPaused { get; private set; }
    public TMP_Text wavesText;
    public TMP_Text killsText;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (pausePanel != null) pausePanel.SetActive(false);
    }

    private void Update()
    {
        // Esc (o P, por si el navegador captura Esc en WebGL)
        if (Keyboard.current == null) return;

        if (Keyboard.current.escapeKey.wasPressedThisFrame || Keyboard.current.pKey.wasPressedThisFrame)
        {
            if (IsPaused) Resume();
            else Pause();
        }
    }

    public void Pause()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;

        IsPaused = true;
        Time.timeScale = 0f;

        if (WaveManager.Instance != null)
        {
            if (wavesText != null) wavesText.text = "Oleadas superadas: " + WaveManager.Instance.WavesSurvived;
            if (killsText != null) killsText.text = "Enemigos matados: " + WaveManager.Instance.EnemiesKilled;
        }

        if (pausePanel != null) pausePanel.SetActive(true);
    }

    // Para el botón "Continuar"
    public void Resume()
    {
        IsPaused = false;
        Time.timeScale = 1f;
        if (pausePanel != null) pausePanel.SetActive(false);
    }

    // Para el botón "Reiniciar" del menú de pausa
    public void Restart()
    {
        IsPaused = false;
        if (GameManager.Instance != null) GameManager.Instance.RestartGame();
    }
}