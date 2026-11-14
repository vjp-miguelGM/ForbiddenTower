using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("HUD of GAME OVER")]
    public GameObject gameOverPanel; // panel de Game Over, desactivado al inicio

    public bool IsGameOver { get; private set; }

    private void Awake()
    {
        Instance = this;
        Time.timeScale = 1f; // por si venimos de un reinicio con el juego parado
    }

    private void Start()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    public void GameOver()
    {
        if (IsGameOver) return; // evita que se llame dos veces

        IsGameOver = true;
        Time.timeScale = 0f; // el juego se para

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        Debug.Log("GAME OVER");
    }

    // Para el botón "Reintentar" del HUD
    public void RestartGame()
    {
        Time.timeScale = 1f; // importante: reactivar el tiempo antes de recargar
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}