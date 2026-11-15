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

    private float finalSensitivity; 

    void Start()
    {
        // ดึงค่าความไวเมาส์ที่บันทึกไว้จากหน้า Settings (ถ้าไม่มี จะใช้ค่า 1.0)
        float savedSensMultiplier = PlayerPrefs.GetFloat("MouseSensitivity", 1.0f);
        
        // คำนวณความไวเมาส์จริง
        finalSensitivity = sensitivity * savedSensMultiplier;

        // ปรินท์เช็คใน Console ว่าดึงค่าอะไรมา (ถ้าเลื่อน Slider มา ค่าตรงนี้ต้องเปลี่ยนตาม!)
        Debug.Log("🎯 ความไวเมาส์ที่ใช้จริง: " + finalSensitivity + " (ตัวคูณจาก Settings: " + savedSensMultiplier + ")");
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPosition = target.position + Vector3.up * targetHeight;
        transform.position = targetPosition;

        // เช็คการคลิกเมาส์ซ้ายเพื่อหมุนมุมกล้อง
        if (Input.GetMouseButton(0))
        {
            // ใช้ finalSensitivity ที่คำนวณจากหน้า Settings
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