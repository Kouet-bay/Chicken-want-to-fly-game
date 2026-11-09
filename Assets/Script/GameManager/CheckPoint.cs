using UnityEngine;

public class CheckPoint : Identity, IInteractable
{
    public void Use(Player player)
    {
        GameManager.Instance.SetCheckPoint(transform.position);
        Destroy(gameObject);
    }
}