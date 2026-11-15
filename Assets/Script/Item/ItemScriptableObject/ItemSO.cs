using UnityEngine;

// Abstract class + ScriptableObject: เก็บข้อมูลพื้นฐานของไอเทมทุกชนิด
// และบังคับให้แต่ละชนิดกำหนดผลลัพธ์ตอนถูกใช้งานเอง (polymorphism ผ่าน ApplyEffect)
public abstract class ItemSO : ScriptableObject
{
    [Header("Basic Item Info")]
    public string itemName = "New Item";
    public Sprite icon;
    [TextArea(2, 3)] public string description;

    public abstract void ApplyEffect(Player player);
}
