using UnityEngine;

public class WorldBuffItem : MonoBehaviour, IInteractable
{
    [SerializeField] private BuffItemSO buffItemData; // ลาก ScriptableObject จากข้อ 1 มาใส่ตรงนี้

    public void Use(Player player)
    {
        if (buffItemData != null && player != null)
        {
            // เรียกใช้ฟังก์ชัน ApplyEffect ของ ItemSO ที่สืบทอดมา
            buffItemData.ApplyEffect(player);
            
            // ทำลายวัตถุไอเทมชิ้นนี้ทิ้งหลังใช้งาน
            Destroy(gameObject);
        }
        else
        {
            Debug.LogWarning("BuffItemSO หรือ Player เป็น null!");
        }
    }
}