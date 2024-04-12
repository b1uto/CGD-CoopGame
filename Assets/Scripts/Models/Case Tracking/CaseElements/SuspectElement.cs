using UnityEngine;

namespace CGD.Case
{
    public class SuspectElement : CaseElement
    {
        [HideInInspector]
        public Suspect suspect;

        public override CaseItem GetItem() {  return suspect; }
    }
}