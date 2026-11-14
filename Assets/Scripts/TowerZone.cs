using UnityEngine;

public class TowerZone : MonoBehaviour
{
    public bool PlayerInside { get; private set; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) PlayerInside = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) PlayerInside = false;
    }
}