using UnityEngine;

public class WallButton : Identity, IInteractable
{
    [Header("Target Plank / Bridge")]
    [SerializeField] private GameObject woodenPlank; 
    private bool isOn = false;

    public void Use(Player player)
    {
        isOn = !isOn; 

        if (woodenPlank != null)
        {
            woodenPlank.SetActive(isOn); 
        }

        Debug.Log($"{IdentityName} is now: {(isOn ? "ON" : "OFF")}");
    }
}