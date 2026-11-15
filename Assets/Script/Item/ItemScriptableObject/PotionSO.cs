using UnityEngine;

[CreateAssetMenu(fileName = "NewPotion", menuName = "Item System/Potion")]
public class PotionSO : ItemSO
{
    [Header("Potion Settings")]
    public int healAmount = 20;

    public override void ApplyEffect(Player player)
    {
        if (player != null)
        {
            player.Heal(healAmount);
            
            // เปลี่ยนจาก player.IdentityName เป็น player.gameObject.name
            Debug.Log($"{player.gameObject.name} used {itemName} and recovered {healAmount} HP.");
        }
    }
}