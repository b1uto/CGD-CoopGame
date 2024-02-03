using UnityEngine;

[CreateAssetMenu(fileName = "Boots", menuName = "Settings/Input", order = 5)]
[System.Serializable]
public class InputSettings : ScriptableObject
{
    [Header("Movement")]
    public float moveSmoothTime;
    public float moveMaxSpeed;

    [Header("Camera")]
    public float sensitivityY;
    public float sensitivityX;
    public bool invertY;

    /// <summary>
    /// Inverts Y input if enabled
    /// </summary>
    public int InvertY 
    { 
        get 
        {
            return (invertY) ? -1 : 1;
        } 
    }

}
