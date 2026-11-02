using UnityEngine;

public class CarryableObject : Identity, ICollectible
{
    private Rigidbody rb;
    private Collider col;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    public void OnPickUp(Transform carrySocket)
    {
        if (rb != null)
        {
            rb.isKinematic = true; 
        }
        if (col != null)
        {
            col.enabled = false;   
        }

        transform.SetParent(carrySocket);
        transform.localPosition = new Vector3(0f, 0.5f, 0f); 
        transform.localRotation = Quaternion.identity;
    }

    
    public void OnUse(Player player)
    {
        Debug.Log($"{IdentityName} is being carried.");
        
        
    }

    public void OnDrop()
    {
        transform.SetParent(null); 

        if (col != null)
        {
            col.enabled = true; 
        }
        if (rb != null)
        {
            rb.isKinematic = false; 
            rb.AddForce(Vector3.up * 3f + transform.forward * 2f, ForceMode.Impulse); 
        }
    }
}