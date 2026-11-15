using UnityEngine;

// World pickup: กด E เพื่อเก็บและใช้ยา (implement IInteractable)
public class HealthPotion : Identity, IInteractable
{
    [SerializeField] private PotionSO potionData;

    public void Use(Player player)
    {
        if (potionData == null)
        {
            Debug.LogWarning($"{IdentityName} has no PotionSO assigned.");
            return;
        }
        potionData.ApplyEffect(player);
        Destroy(gameObject);
    }
}
