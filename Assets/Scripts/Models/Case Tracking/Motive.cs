using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Motive", menuName = "Case Data/Motive", order = 3)]
public class Motive : CaseItem
{
    public enum MotiveType
    {
        Revenge,
        Jealousy,
        LoveTriangle,
        Inheritance,
        InsuranceFraud,
        Theft,
        PoliticalGain,
        Suppression,
        Rivalry,
        Compulsion,
        Instability,
        Thrill,
        Protection,
        MistakenIdentity,
        Accidental
    }


    [HideInInspector]
    public MotiveType type;
}
