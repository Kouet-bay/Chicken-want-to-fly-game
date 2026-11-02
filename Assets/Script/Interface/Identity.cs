using UnityEngine;

public class Identity : MonoBehaviour
{
    [SerializeField] private string identityName;
    [SerializeField] private int positionX;
    [SerializeField] private int positionZ;

    public string IdentityName
    {
        get { return identityName; }
        set { identityName = value; }
    }

    public int PositionX
    {
        get { return positionX; }
        set { positionX = value; }
    }

    public int PositionZ
    {
        get { return positionZ; }
        set { positionZ = value; }
    }

    public void PrintIdentityInfo()
    {
        Debug.Log("Identity Name: " + IdentityName + ", Position: (" + PositionX + ", " + PositionZ + ")");
    }
}
