
using UnityEngine;

namespace CGD.Case
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

    public class Motive : CaseElement
    {
        [HideInInspector]
        public MotiveType type;
    }
}