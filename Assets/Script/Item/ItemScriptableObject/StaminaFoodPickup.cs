using UnityEngine;

// อาหารฟื้น Stamina วางไว้บนพื้น: กด E เพื่อกินและฟื้นพลังหนีศัตรูทันที
public class StaminaFoodPickup : Identity, IInteractable
{
    [SerializeField] private StaminaFoodSO foodData;

    public void Use(Player player)
    {
        if (foodData == null)
        {
            Debug.LogWarning($"{IdentityName} has no StaminaFoodSO assigned.");
            return;
        }

        // ส่งผลลัพธ์ไปยัง Player
        foodData.ApplyEffect(player);

        // ทำลายวัตถุชิ้นนี้ทิ้งหลังกินเสร็จ
        Destroy(gameObject);
    }
}