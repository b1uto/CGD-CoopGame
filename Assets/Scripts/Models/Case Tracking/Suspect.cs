using UnityEngine;

namespace CGD.Case
{

    [CreateAssetMenu(fileName = "Suspect", menuName = "Case Data/Suspect", order = 1)]
    public class Suspect : CaseItem
    {
        public enum Title
        {
            Mr,
            Ms,
            Miss,
            Mrs
        }
        public enum EyeColour
        {
            Brown,
            Green,
            Blue,
            Grey,
            Black
        }
        public enum Height
        {
            Short,
            Average,
            Tall
        }
        public enum Build
        {
            Stocky,
            Average,
            Lean
        }

        [HideInInspector]
        public string fullName;

        [HideInInspector]
        public Title title;

        [HideInInspector]
        public int age;

        [HideInInspector]
        public EyeColour eyeColour;

        [HideInInspector]
        public Height height;

        [HideInInspector]
        public Build build;

        [HideInInspector]
        public string modelId;

       
    }


}
