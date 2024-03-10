using UnityEngine;

namespace CGD.Case
{

    public abstract class CaseData : ScriptableObject
    {
#if UNITY_EDITOR
        public string ID
        {
            get
            {
                if (string.IsNullOrEmpty(id))
                    GenerateUniqueID(this);

                return id;
            }
        }
#endif

        /// <summary>
        /// unique guid for each clue
        /// </summary>
        [ReadOnly]
        public string id;

        [MaxLength(68)]
        public string shortDescription;
        [MaxLength(68)]
        [Tooltip("This should include a color tag for key words")]
        public string analysedDescription;
        [TextArea(3, 10)]
        public string fullDescription;

        [HideInInspector]
        public Sprite icon;

#if UNITY_EDITOR
        public static void GenerateUniqueID(CaseData caseData)
        {
            if (string.IsNullOrEmpty(caseData.id))
            {
                caseData.id = System.Guid.NewGuid().ToString();
                UnityEditor.EditorUtility.SetDirty(caseData);
            }
        }
#endif
    }


}
