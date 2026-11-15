using System.Collections.Generic;
using UnityEngine;

// Interactable (กด E) เหมือน Door/Switch เดิม: เปิดครั้งเดียวแล้วแจกไอเทมทั้งหมดในหีบ
public class TreasureChest : Identity, IInteractable
{
    [SerializeField] private bool isOpen = false;
    [SerializeField] private List<ItemSO> lootItems = new List<ItemSO>();
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Use(Player player)
    {
        if (isOpen)
        {
            Debug.Log($"{IdentityName} is already empty.");
            return;
        }

        isOpen = true;
        if (animator != null)
        {
            animator.SetBool("isOpen", true);
        }

        foreach (ItemSO item in lootItems)
        {
            if (item != null)
            {
                item.ApplyEffect(player);
            }
        }
        Debug.Log($"Opened {IdentityName} and received {lootItems.Count} item(s).");
    }
}
