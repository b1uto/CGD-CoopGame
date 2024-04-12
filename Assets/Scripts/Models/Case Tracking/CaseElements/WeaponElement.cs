
using UnityEngine;

namespace CGD.Case
{
   
    public class WeaponElement : CaseElement
    {
        [HideInInspector]
        public Weapon weapon;

        public override CaseItem GetItem()
        {
            return weapon;
        }
    }


}
