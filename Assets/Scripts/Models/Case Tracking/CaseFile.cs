using UnityEngine;

namespace CGD.Case
{

    [CreateAssetMenu(fileName = "CaseFile", menuName = "Case Data/Case File", order = 0)]
    public class CaseFile : CaseData
    {
        [HideInInspector]
        public string[] elements;
    }

   
}