using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform objetivo;
    public float velocidad = 2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (objetivo == null) return;

        float dir = Mathf.Sign(objetivo.position.x - transform.position.x);
        transform.position += Vector3.right * dir * velocidad * Time.deltaTime;

        // Girar el sprite según hacia dónde mira
        transform.localScale = new Vector3(dir, 1, 1);
    }
}
