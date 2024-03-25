using System;
using UnityEngine;

namespace CGD.Audio
{
    public enum Music
    {
        Ambient,
        Menu,
        Investigation,
        Meeting
    }

    [CreateAssetMenu(fileName = "Music", menuName = "Audio/Music", order = 1)]
    public class MusicCollection : AudioCollection
    {
        protected override string[] Names { get { return Enum.GetNames(typeof(Music)); } }
    }
}
