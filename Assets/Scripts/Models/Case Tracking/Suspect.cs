
using UnityEngine;

namespace CGD.Case
{

    public enum Title 
    {
        Mr,
        Ms,
        Miss,
        Mrs
    }

    public class Suspect : CaseElement
    {
        [HideInInspector]
        public string fullName;

        [HideInInspector]
        public Title title;

        [HideInInspector]
        public int age;

        [HideInInspector]
        public string modelId;
    }
}