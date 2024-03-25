using System;
using UnityEngine;

namespace CGD.Audio
{
    public enum UI
    {
        Accept,
        Back,
        Forward,
        Scroll,
        Typewriter
    }



    [CreateAssetMenu(fileName = "UI", menuName = "Audio/UI", order = 0)]
    public class UICollection : AudioCollection
    {
        protected override string[] Names { get { return Enum.GetNames(typeof(UI)); } }
    }
}
