using UnityEngine;

[CreateAssetMenu(fileName = "NewStaminaFood", menuName = "Item System/Stamina Food")]
public class StaminaFoodSO : ItemSO
{
    [Header("Stamina Settings")]
    [Tooltip("จำนวน Stamina ที่ต้องการฟื้นฟู (ใส่ 100 เพื่อให้เต็มหลอดทันที)")]
    public float staminaRestoreAmount = 100f;

    public override void ApplyEffect(Player player)
    {
        if (player != null)
        {
            // เรียกใช้ฟังก์ชันฟื้นฟู Stamina ใน Player
            player.RestoreStamina(staminaRestoreAmount);
            
            // แก้ไขตรงนี้จาก player.IdentityName เป็น player.gameObject.name แทน
            Debug.Log($"{player.gameObject.name} used {itemName} and recovered {staminaRestoreAmount} Stamina to escape!");
        }
    }
}