using UnityEngine;

public class Trap : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.collider.CompareTag("Player"))
        {
            var p = c.collider.GetComponent<Player>();
            if (p != null) p.Respawn();
        }
    }
}
