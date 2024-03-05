using UnityEngine;

namespace CGD.Case
{

    [CreateAssetMenu(fileName = "CaseFile", menuName = "Case Data/Case File", order = 0)]
    public class Case : ScriptableObject
    {
        [Header("Case Settings")]
        [ReadOnly]
        public string id;
        [MaxLength(68)]
        public string shortDescription;
        [TextArea(3, 10)]
        public string description;

        [HideInInspector]
        public string[] caseElements;
    }

   
}