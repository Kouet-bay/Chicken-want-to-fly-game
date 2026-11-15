using UnityEngine;

[CreateAssetMenu(fileName = "NewBuffItem", menuName = "Item System/Buff Item")]
public class BuffItemSO : ItemSO
{
    [Header("Stamina Buff Settings")]
    [Tooltip("จำนวน Stamina ที่ต้องการฟื้นฟู (ใส่ 100 เพื่อให้เต็มหลอดทันที)")]
    public float staminaRestoreAmount = 100f;

    public override void ApplyEffect(Player player)
    {
        if (player != null)
        {
            player.RestoreStamina(staminaRestoreAmount);
            Debug.Log($"Used {itemName} and restored {staminaRestoreAmount} Stamina!");
        }
    }
}