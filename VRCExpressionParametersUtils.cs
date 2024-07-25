using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace HeavenVR.ToggleAssistant.Editor
{
    public static class VRCExpressionParametersUtils
    {
        public static VRCExpressionParameters CreateParameters(string path, List<Parameter> parameters)
        {
            // Create a new scriptable object of type <VRCExpressionParameters>.
            var expressionParameters = ScriptableObject.CreateInstance<VRCExpressionParameters>();

            // Set the parameters.
            expressionParameters.parameters = parameters.Select(p => p.ToVRCExpressionParameter()).ToArray();

            // Save the asset to the specified path.
            AssetDatabase.CreateAsset(expressionParameters, path);

            return expressionParameters;
        }
    }
}