using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    public Transform target;          
    public float distance = 8.0f;     
    public float sensitivity = 3.0f;  
    public float minYAngle = 10f;     
    public float maxYAngle = 80f;     
    public float targetHeight = 1.2f; 

    private float currentX = 0.0f;
    private float currentY = 45.0f;   

    void LateUpdate()
    {
        if (target == null) return;

        
        Vector3 targetPosition = target.position + Vector3.up * targetHeight;
        transform.position = targetPosition;

        
        if (Input.GetMouseButton(0))
        {
            currentX += Input.GetAxis("Mouse X") * sensitivity;
            currentY -= Input.GetAxis("Mouse Y") * sensitivity;
            currentY = Mathf.Clamp(currentY, minYAngle, maxYAngle);
        }

        
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        transform.rotation = rotation;
        
        Transform cam = GetComponentInChildren<Camera>().transform;
        cam.localPosition = new Vector3(0, 0, -distance);
    }
}