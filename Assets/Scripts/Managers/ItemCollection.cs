using CGD.Case;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemCollection : Singleton<ItemCollection>
{
    [Header("Collections")]
    [SerializeField] private CaseFile[] caseData;
    [SerializeField] private CaseElement[] elementData;
    [SerializeField] private Clue[] clueData;

    [Header("Items")]
    [SerializeField] private Suspect[] suspects;
    [SerializeField] private Motive[] motives;
    [SerializeField] private Weapon[] weapons;


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

    public InputSettings DefaultInputSettings { get { return inputSettings[0]; } }
    public InputSettings InputSettings { get { return inputSettings[1]; } }
    #endregion

    #region temp
    /// <summary>
    /// TODO store elsewhere in Item Collection. Randomise if player has not chosen avatar.
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


    private void Start()
    {
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


    #region DEBUG
    public string[] GetClueKeys() { return Clues.Keys.ToArray(); }

    public static string GetRandomModelName()
    {
        return modelNames[Random.Range(0, modelNames.Length)];
    }

    #endregion


}
