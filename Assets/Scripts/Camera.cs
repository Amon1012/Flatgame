using UnityEngine;

public class Camera : MonoBehaviour
{


    public Transform target;
    public float smoothSpeed = 0.125f;   // 越小越平滑
    public Vector3 offset = new Vector3(0f, 1.5f, -10f);


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
