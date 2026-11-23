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
    private Transform camTransform;

    void Start()
    {
        Camera childCam = GetComponentInChildren<Camera>(true);
        if (childCam != null)
        {
            camTransform = childCam.transform;
        }
    }

    void LateUpdate()
    {
        if (Time.timeScale == 0f) return;
        if (target == null) return;

        Vector3 targetPosition = target.position + Vector3.up * targetHeight;
        transform.position = targetPosition;

        float currentSensMultiplier = PlayerPrefs.GetFloat("MouseSensitivity", 1.0f);
        float finalSensitivity = sensitivity * currentSensMultiplier;

        if (Input.GetMouseButton(0))
        {
            currentX += Input.GetAxis("Mouse X") * finalSensitivity;
            currentY -= Input.GetAxis("Mouse Y") * finalSensitivity;
            currentY = Mathf.Clamp(currentY, minYAngle, maxYAngle);
        }

        transform.rotation = Quaternion.Euler(currentY, currentX, 0);

        if (camTransform != null)
        {
            camTransform.localPosition = new Vector3(0, 0, -distance);
        }
    }
}