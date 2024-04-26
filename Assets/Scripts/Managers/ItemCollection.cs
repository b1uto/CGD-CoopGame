using CGD.Case;
using CGD.Gameplay;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollection : Singleton<ItemCollection>
{
    public static System.Action OnUserProfileUpdated;

    [Header("Case Data")]
    [SerializeField] private CaseFile[] caseData;
    [SerializeField] private CaseElement[] elementData;
    [SerializeField] private Clue[] clueData;

    [Header("Items")]
    [SerializeField] private Suspect[] suspects;
    [SerializeField] private Motive[] motives;
    [SerializeField] private Weapon[] weapons;

    [Header("Sprites")]
    [SerializeField] private Sprite[] avatars;

    #region Dictionaries
    private Dictionary<string, CaseFile> CaseFiles = new Dictionary<string, CaseFile>();
    private Dictionary<string, CaseElement> CaseElements = new Dictionary<string, CaseElement>();
    private Dictionary<string, Clue> Clues = new Dictionary<string, Clue>();


    private Dictionary<string, Suspect> Suspects = new Dictionary<string, Suspect>();
    private Dictionary<string, Motive> Motives = new Dictionary<string, Motive>();
    private Dictionary<string, Weapon> Weapons = new Dictionary<string, Weapon>();
    #endregion

    #region Settings
    [Header("Settings Data")]
    [SerializeField] private InputSettings[] inputSettings;
    #endregion

    #region temp
    private UserProfile userProfile;    

    /// <summary>
    /// model ids.
    /// </summary>
    private static readonly string[] modelNames = new string[5]
    {
        "female01",
        "female02",
        "male01",
        "male02",
        "male03"
    };

    #endregion

    #region Properties
    public InputSettings DefaultInputSettings { get { return inputSettings[0]; } }
    public InputSettings InputSettings { get { return inputSettings[1]; } }
    public UserProfile UserProfile { get { return userProfile; } }
    public Sprite[] Avatars { get { return avatars; } }
    #endregion

    private void Start()
    {
        LoadUserProfile();
        InitializeDictionaries();
    }

    private void InitializeDictionaries() 
    {
        PopulateDictionary(ref caseData, ref CaseFiles);
        PopulateDictionary(ref elementData, ref CaseElements);
        PopulateDictionary(ref clueData, ref Clues);

        PopulateGenericDictionary(ref suspects, ref Suspects);
        PopulateGenericDictionary(ref motives, ref Motives);
        PopulateGenericDictionary(ref weapons, ref Weapons);
    }

    private void PopulateDictionary<T>(ref T[] arr, ref Dictionary<string, T> dict) where T : CaseData
    {
        foreach(var item in arr) 
        {
            dict.Add(item.id, item);
        }
    }

    private void PopulateGenericDictionary<T>(ref T[] arr, ref Dictionary<string, T> dict) where T : ScriptableObject
    {
        foreach (var item in arr)
        {
            dict.Add(item.name, item);
        }
    }

    public bool TryGetCaseData<T>(string id, out T data) where T : CaseData
    {
        data = default;

        if (typeof(T) == typeof(CaseFile) && CaseFiles.TryGetValue(id, out var caseFile)) 
        {
            data = caseFile as T;
            return true;
        }
        else if (typeof(T) == typeof(CaseElement) && CaseElements.TryGetValue(id, out var caseElement))
        {
            data = caseElement as T;
            return true;
        }
        else if (typeof(T) == typeof(Clue) && Clues.TryGetValue(id, out var clue))
        {
            data = clue as T;
            return true;
        }

        return false;
    }
    public bool TryGetCaseItem<T>(string id, out T data) where T : CaseItem
    {
        data = default;

        if (typeof(T) == typeof(Weapon) && Weapons.TryGetValue(id, out var weapon))
        {
            data = weapon as T;
            return true;
        }
        else if (typeof(T) == typeof(Suspect) && Suspects.TryGetValue(id, out var suspect))
        {
            data = suspect as T;
            return true;
        }
        else if (typeof(T) == typeof(Motive) && Motives.TryGetValue(id, out var motive))
        {
            data = motive as T;
            return true;
        }

        return false;
    }

    public Weapon[] GetWeapons() { return weapons; }
    public Suspect[] GetSuspects() { return suspects; }
    public Motive[] GetMotives() { return motives; }

    public CaseItem[] GetActiveCaseItems() 
    {
        if(GameManager.Instance != null) 
        {
            var caseFile = GameManager.Instance.ActiveCase;

            var elements = caseFile.elements.Select(key => CaseElements[key]).ToArray();
            return elements.Select(item => item.GetItem()).ToArray();
        }

        return null;
    }


    private void LoadUserProfile() 
    {
        if(userProfile == null) { userProfile = ScriptableObject.CreateInstance<UserProfile>(); }

        if (!SaveData.LoadFile(ref userProfile))
        {
            userProfile.model = 0; 
            userProfile.icon = 0;
        }

        OnUserProfileUpdated?.Invoke();
    }

    public void UpdateUserProfile(int model, int icon) 
    {
        if(userProfile == null) 
        {
            userProfile = ScriptableObject.CreateInstance<UserProfile>();
        }

        UserProfile.model = model;
        UserProfile.icon = icon;
        SaveUserProfile();
    }
    public void SaveUserProfile() 
    {
        if(userProfile != null) 
        {
            SaveData.SaveFile(userProfile);
            OnUserProfileUpdated?.Invoke();
        }
    }

    #region DEBUG
    public string[] GetClueKeys() { return Clues.Keys.ToArray(); }

    public static string GetRandomModelName()
    {
        return modelNames[Random.Range(0, modelNames.Length)];
    }

    public static string GetModelName(int index) 
    {
        if(index >= 0 && index < modelNames.Length)
            return modelNames[index];
        else
            return null;
    }

    public static int GetModelCount() => modelNames.Length;

    #endregion


}
