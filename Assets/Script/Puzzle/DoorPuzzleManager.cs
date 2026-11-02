using UnityEngine;

public class DoorPuzzleManager : MonoBehaviour
{
    [Header("Target Door")]
    [SerializeField] private GameObject switchDoor; 

    
    private bool isPlate1Pressed = false;
    private bool isPlate2Pressed = false;
    private bool isPlayerPressed = false;

    
    private bool isPuzzleSolved = false;

    private void Start()
    {
        if (switchDoor != null)
        {
            switchDoor.SetActive(true); 
        }
    }

    public void SetPlateState(int plateID, bool isPressed)
    {
        
        if (isPuzzleSolved) return;

        switch (plateID)
        {
            case 1: isPlate1Pressed = isPressed; break;
            case 2: isPlate2Pressed = isPressed; break;
            case 3: isPlayerPressed = isPressed; break;
        }

        EvaluateDoorPuzzle();
    }

    private void EvaluateDoorPuzzle()
    {
        
        if (isPlate1Pressed && isPlate2Pressed && isPlayerPressed)
        {
            OpenDoorPermanently();
        }
    }

    private void OpenDoorPermanently()
    {
        isPuzzleSolved = true; 

        if (switchDoor != null)
        {
            switchDoor.SetActive(false); 
            Debug.Log("Puzzle Solved! Door is now permanently open.");
        }
    }
}