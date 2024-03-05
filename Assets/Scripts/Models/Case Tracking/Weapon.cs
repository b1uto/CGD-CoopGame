
using UnityEngine;

namespace CGD.Case
{
    public enum WeaponType
    {
        Revolver,
        Knife,
        Poison
    }

    public class Weapon : CaseElement
    {
        [HideInInspector]
        public WeaponType type;
    }


}
