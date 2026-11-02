// สิ่งที่ Player กด E เพื่อโต้ตอบได้ (Door, Switch, TreasureChest ฯลฯ)
public interface IInteractable
{
    void Use(Player player);
}
