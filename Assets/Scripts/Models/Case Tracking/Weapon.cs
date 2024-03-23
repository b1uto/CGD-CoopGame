using UnityEngine;

namespace CGD.Case
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Case Data/Weapon", order = 2)]
    public class Weapon : CaseItem
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


        [HideInInspector]
        public WeaponType type;

    }
}
