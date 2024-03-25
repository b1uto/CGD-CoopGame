using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGD.Audio
{
  

    public enum MiniGame 
    {
        Pass,
        Fail
    }
   

    public abstract class AudioCollection : ScriptableObject
    {
        protected virtual string[] Names { get; set; }

        public AudioClip[] clips;

        [Header("Collections")]
        protected Dictionary<string, AudioClip> clipDict = new Dictionary<string, AudioClip>();
       


        private void OnEnable()
        {
          PopulateDict();
        }

        protected virtual void PopulateDict() 
        {
            if (clips == null || Names == null) { return; }
            var min = Mathf.Min(clips.Length, Names.Length);

            for (int i = 0; i < min; i++)
            {
                if (!clipDict.ContainsKey(Names[i]))
                {
                    clipDict.Add(Names[i], clips[i]);
                }
            }
        }
        public AudioClip GetClip(string name)
        {
            if (clipDict.ContainsKey(name))
                return clipDict[name];
            else
                return null;
        }
    }
}
