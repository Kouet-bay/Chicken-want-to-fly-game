using UnityEngine;

public class MudTrap : MonoBehaviour
{
    [Header("Mud Trap Settings")]
    [Range(0.1f, 0.9f)] [SerializeField] private float speedMultiplier = 0.4f;   
    [Range(0.1f, 0.9f)] [SerializeField] private float dashMultiplier = 0.5f;   

    // ใช้ OnTriggerStay เพื่อให้ตรวจจับ CharacterController ได้ต่อเนื่องตอนที่ผู้เล่นยืนหรือเดินอยู่ในบ่อ
    private void OnTriggerStay(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            Debug.Log("Player อยู่ในบ่อโคลน: " + other.name);
            player.EnterMud(speedMultiplier, dashMultiplier);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            Debug.Log("Player ออกจากบ่อโคลนแล้ว");
            player.ExitMud();
        }
    }
}