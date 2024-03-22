
using UnityEngine;

namespace CGD.Case
{
    public enum WeaponType
    {
        Firearm,
        Sharp,
        Blunt,
        Poison,
        Fists,
        Fall
    }

    public class Weapon : CaseElement
    {
        [HideInInspector]
        public WeaponType type;
    }


}
