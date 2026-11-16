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
    // ถ้าเกมถูก Pause อยู่ (เวลาเป็น 0) ให้หยุดการหมุนกล้องทันที ป้องกันกล้องขยับตอนกดปรับ Slider
    if (Time.timeScale == 0f) return;

    if (target == null) return;

    Vector3 targetPosition = target.position + Vector3.up * targetHeight;
    transform.position = targetPosition;

    // ดึงค่าความไวเมาส์ล่าสุดจาก PlayerPrefs แบบสดๆ
    float currentSensMultiplier = PlayerPrefs.GetFloat("MouseSensitivity", 1.0f);
    float finalSensitivity = sensitivity * currentSensMultiplier;

    // เช็คการคลิกเมาส์ซ้ายเพื่อหมุนมุมกล้อง
    if (Input.GetMouseButton(0))
    {
        currentX += Input.GetAxis("Mouse X") * finalSensitivity;
        currentY -= Input.GetAxis("Mouse Y") * finalSensitivity;
        currentY = Mathf.Clamp(currentY, minYAngle, maxYAngle);
    }

    Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
    transform.rotation = rotation;
    
    Transform cam = GetComponentInChildren<Camera>().transform;
    cam.localPosition = new Vector3(0, 0, -distance);
}
}