using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGD.Audio
{
    public enum Gameplay
    {
        Collect,
        CloseDoor1,
        CloseDoor2,
        CloseDoor3,
        OpenDoor1,
        OpenDoor2,
        OpenDoor3,
        Grab,
        LightSwitchOn,
        LightSwitchOff,
        LightToolOn,
        LightToolOff,
        MetalDetectorBeep,
        Spray
    }

    [CreateAssetMenu(fileName = "Gameplay", menuName = "Audio/Gameplay", order = 2)]
    public class GameplayCollection : AudioCollection
    {
        protected override string[] Names { get { return Enum.GetNames(typeof(Gameplay)); } }
    }

}