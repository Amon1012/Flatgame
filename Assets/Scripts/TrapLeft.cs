using UnityEngine;

public class TrapLeft : MonoBehaviour
{

    public Transform player;
    public float triggerRadius = 3.5f;   // ¥•∑¢∑∂Œß
    public float dashDistance = 2f;    // ÷±Ω”À≤“∆
    bool triggered = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      
        if (player == null)
        {
            var p = GameObject.FindWithTag("Player");
            if (p) player = p.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (!triggered && player != null)
        {
            float dist = Vector2.Distance(transform.position, player.position);
            if (dist <= triggerRadius)
            {
                transform.position += Vector3.left * dashDistance;
                triggered = true;
            }
        }


    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.collider.CompareTag("Player"))
        {
            var p = c.collider.GetComponent<Player>();
            if (p != null) p.Respawn();
        }
    }


#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, triggerRadius);
    }
#endif
}
