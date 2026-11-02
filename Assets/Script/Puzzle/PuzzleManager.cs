using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [Header("Puzzle Target")]
    [SerializeField] private GameObject woodenBridge; 

    
    private bool isPlate1Pressed = false;      
    private bool isPlate2Pressed = false;     
    private bool isPlayerPressed = false;      
    private bool isWallButtonActivated = false; 

    private void Start()
    {
        if (woodenBridge != null)
        {
            woodenBridge.SetActive(false); 
        }
    }

    public void SetPlateState(int plateID, bool isPressed)
    {
        switch (plateID)
        {
            case 1: isPlate1Pressed = isPressed; break;
            case 2: isPlate2Pressed = isPressed; break;
            case 3: isPlayerPressed = isPressed; break;
        }
        EvaluatePuzzle();
    }

    public void SetWallButtonState(bool isActivated)
    {
        isWallButtonActivated = isActivated;
        EvaluatePuzzle();
    }

    private void EvaluatePuzzle()
    {
        
        if (isPlate1Pressed && isPlate2Pressed && isPlayerPressed && isWallButtonActivated)
        {
            OpenBridge();
        }
        else
        {
            CloseBridge();
        }
    }

    private void OpenBridge()
    {
        if (woodenBridge != null && !woodenBridge.activeSelf)
        {
            woodenBridge.SetActive(true);
            Debug.Log("Puzzle Solved: Bridge is now open!");
        }
    }

    private void CloseBridge()
    {
        if (woodenBridge != null && woodenBridge.activeSelf)
        {
            woodenBridge.SetActive(false);
            Debug.Log("Puzzle Reset: Bridge closed.");
        }
    }
}