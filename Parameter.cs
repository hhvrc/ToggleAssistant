using VRC.SDK3.Avatars.ScriptableObjects;

namespace HeavenVR.ToggleAssistant.Editor
{
    public class Parameter
    {
        public enum ValueType
        {
            Bool,
            Int,
            Float,
        }

        public Parameter(string name, bool defaultState = false, bool networkSynced = true, bool saved = true)
        {
            Name = name;
            DefaultState = defaultState;
            NetworkSynced = networkSynced;
            Saved = saved;
        }

        public string Name;
        public ValueType Type;
        public bool DefaultState;
        public bool NetworkSynced;
        public bool Saved;

        public VRCExpressionParameters.Parameter ToVRCExpressionParameter()
        {
            return new VRCExpressionParameters.Parameter
            {
                name = Name,
                valueType = Type switch
                {
                    ValueType.Int => VRCExpressionParameters.ValueType.Int,
                    ValueType.Float => VRCExpressionParameters.ValueType.Float,
                    ValueType.Bool => VRCExpressionParameters.ValueType.Bool,
                    _ => VRCExpressionParameters.ValueType.Int,
                },
                defaultValue = DefaultState ? 1 : 0,
                networkSynced = NetworkSynced,
                saved = Saved
            };
        }
    }
}