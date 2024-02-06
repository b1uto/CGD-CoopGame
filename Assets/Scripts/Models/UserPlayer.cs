using UnityEngine;

/// <summary>
/// Placeholder Data Object. Will hold customisation data of users player character. will eventually be sent by server.
/// </summary>
[CreateAssetMenu(fileName = "UserPlayer", menuName = "User Models/Players", order = 0)]
[System.Serializable]
public class UserPlayer : ScriptableObject 
{
    public string modelName;

    /**Placeholders**/
    public int headAccessory;
    public int bodyAccessory;
    public int legAccessory;
    public int footAccessory;
}
