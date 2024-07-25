using UnityEngine;

namespace HeavenVR.ToggleAssistant.Editor
{
    public class SimpleToggle
    {
        public string Name;
        public GameObject GameObject;
        public Parameter Parameter;

        public string LayerName => Name;
        public string AnimationName => Name + (Parameter.DefaultState ? "Enable" : "Disable");
    }
}