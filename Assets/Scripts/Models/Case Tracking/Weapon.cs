
using UnityEngine;

namespace CGD.Case
{
    public enum WeaponType
    {
        Revolver,
        Knife,
        Poison
    }

    [CreateAssetMenu(fileName = "WeaponElement", menuName = "Case Data/Weapon Element", order = 1)]
    public class Weapon : CaseElement
    {
        [HideInInspector]
        public WeaponType type;
    }


}
