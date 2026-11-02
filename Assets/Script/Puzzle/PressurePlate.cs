using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private DoorPuzzleManager doorPuzzleManager;
    [SerializeField, Range(1, 3)] private int plateID = 1;

    private int objectsOnPlate = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (IsValidTarget(other))
        {
            objectsOnPlate++;
            UpdatePlateState(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsValidTarget(other))
        {
            objectsOnPlate = Mathf.Max(0, objectsOnPlate - 1);
            if (objectsOnPlate == 0)
            {
                UpdatePlateState(false);
            }
        }
    }

    private bool IsValidTarget(Collider other)
    {
        if (plateID == 1 || plateID == 2)
        {
            return other.CompareTag("Player") || other.GetComponent<CarryableObject>() != null;
        }
        else if (plateID == 3)
        {
            return other.CompareTag("Player");
        }
        return false;
    }

    private void UpdatePlateState(bool isPressed)
    {
        if (doorPuzzleManager != null)
        {
            doorPuzzleManager.SetPlateState(plateID, isPressed);
        }
    }
}