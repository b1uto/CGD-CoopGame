using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSettings : MonoBehaviour
{
    public enum BuildTarget
    {
        Undefined = 0,
        Development = 1,
        Production = 2
    }

    [HideInInspector]
    public BuildTarget buildTarget;


    public const string SDS_Development = "DEBUGGING";
    public const string SDS_Production = "RELEASE";
}
