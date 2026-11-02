using UnityEngine; 

public interface ICollectible
{
    void OnPickUp(Transform carrySocket); 
    void OnUse(Player player);            
    void OnDrop();                        
}