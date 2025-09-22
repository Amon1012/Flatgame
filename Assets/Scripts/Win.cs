using UnityEngine;

public class Win : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    bool win = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            win = true;
        }
    }

    void OnGUI()
    {
        if (win)
        {
            GUIStyle style = new GUIStyle();
            style.fontSize = 50;
            style.normal.textColor = Color.green;

            // »­ÎÄ×Ö
            GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 25, 200, 50), "YOU WIN", style);
        }
    }
}
