using UnityEngine;

[CreateAssetMenu(fileName = "InputSettings", menuName = "Settings/Input", order = 5)]
[System.Serializable]
public class InputSettings : ScriptableObject
{

    #region Values
    [Header("Movement")]
    [SerializeField]private float moveSmoothTime;
    [SerializeField]private float moveMaxSpeed;

    [Header("Camera")]
    [SerializeField]private float sensitivityY;
    [SerializeField]private float sensitivityX;
    [SerializeField]private bool invertY;
    #endregion

    #region Properties

    public float MoveSmoothTime { get { return moveSmoothTime; } }
    public float MoveMaxSpeed { get { return moveMaxSpeed; } }
    public float SensitivityY { get { return sensitivityX; } }
    public float SensitivityX { get { return sensitivityY; } }

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

    #endregion
    private void OnEnable()
    {
        Load();
    }

    public void ApplySettings(float x,  float y, bool invert) 
    {
        sensitivityX = x;
        sensitivityY = y;
        invertY = invert;
        Save();
    }

    #region Save/Load
    public void Save() => SaveData.SaveFile(this);
    public void Load()
    {
        InputSettings loadedSettings = this;

        if (SaveData.LoadFile(ref loadedSettings))
        {
            this.moveSmoothTime = loadedSettings.moveSmoothTime;
            this.moveMaxSpeed = loadedSettings.moveMaxSpeed;
            this.sensitivityY = loadedSettings.sensitivityY;
            this.sensitivityX = loadedSettings.sensitivityX;
            this.invertY = loadedSettings.invertY;
        }
    }
    #endregion
}
