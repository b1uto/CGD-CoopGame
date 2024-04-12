
using UnityEngine;

namespace CGD.Case
{
    public class MotiveElement : CaseElement
    {
        [HideInInspector]
        public Motive motive;

        public override CaseItem GetItem()
        {
            return motive;
        }
    }
}