using UnityEngine;

public enum EquipSlot { Weapon, Shield }

[CreateAssetMenu(fileName = "NewEquipment", menuName = "Item System/Equipment")]
public class EquipmentSO : ItemSO
{
    [Header("Equipment Settings")]
    public EquipSlot slot;
    public int bonusAttack;
    public int bonusDefense;
    public GameObject modelPrefab;

    public override void ApplyEffect(Player player)
    {
        player.Equip(this);
    }
}
