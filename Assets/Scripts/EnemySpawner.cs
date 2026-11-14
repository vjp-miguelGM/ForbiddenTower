using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject enemyPrefab;
    public Transform centro;          // el objeto del centro que defendéis
    public float sueloY = -3f;        // altura del suelo donde caminan
    public float intervalo = 2f;      // segundos entre apariciones
    public float margen = 2f;         // cuánto fuera de la pantalla aparecen

    float timer;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= intervalo)
        {
            timer = 0f;
            Spawn();
        }
    }

    private void Spawn()
    {
        Camera cam = Camera.main;
        float mitadAncho = cam.orthographicSize * cam.aspect;
        float borde = cam.transform.position.x + mitadAncho + margen;

        // Lado aleatorio: izquierda (-1) o derecha (1)
        int lado = Random.value < 0.5f ? -1 : 1;
        Vector3 pos = new Vector3(cam.transform.position.x + lado * (mitadAncho + margen), sueloY, 0f);

        GameObject e = Instantiate(enemyPrefab, pos, Quaternion.identity);
        e.GetComponent<Enemy>().objetivo = centro;
    }
}
