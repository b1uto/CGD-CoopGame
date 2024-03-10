using System;
using System.Collections;
using UnityEngine;

namespace CGD.Extensions
{
    public class CoroutineUtilities
    {
        public static void StartExclusiveCoroutine(IEnumerator coroutine, ref Coroutine coroutineRef, MonoBehaviour mono) 
        {
            if (coroutineRef != null) mono.StopCoroutine(coroutineRef);
            coroutineRef = mono.StartCoroutine(coroutine);
        } 
    }

}
