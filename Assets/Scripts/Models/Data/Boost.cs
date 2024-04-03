using System;
using UnityEngine;


namespace CGD.Data
{
    public enum BoostType
    {
        XP,
        Gold,
        Trust
    }

    public class Boost : ScriptableObject
    {
        public BoostType BoostType;
        public DateTime EndTime;
        public float Amount;
    }
}