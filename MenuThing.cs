using HeavenVR.ToggleAssistant.Editor;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class MenuThing : EditorWindow
{

    static readonly List<Parameter> parameters = new();
    static readonly List<SimpleToggle> toggles = new();

    [MenuItem("Tools/HeavenVR/Toggle Assistant")]
    public static void ShowWindow()
    {
        GetWindow<MenuThing>("Toggle Assistant").ShowTab();
    }

    [MenuItem("GameObject/HeavenVR/Create Toggles")]
    public static void CreateToggles()
    {
        foreach (GameObject go in Selection.gameObjects)
        {
            if (toggles.Exists(toggle => toggle.GameObject == go))
                continue;

            toggles.Add(new SimpleToggle()
            {
                Name = go.name,
                GameObject = go
            });
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("Toggle Assistant", EditorStyles.boldLabel);

        if (GUILayout.Button("Add parameter"))
        {
            string name = "Param";
            int i = 1;
            while (parameters.Exists(param => param.Name == name))
            {
                name = "Param" + i++;
            }
            parameters.Add(new Parameter(name));
        }

        foreach (var param in parameters)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Name");
            param.Name = EditorGUILayout.TextField(param.Name);
            GUILayout.Label("Type");
            param.Type = (Parameter.ValueType)EditorGUILayout.EnumPopup(param.Type);
            GUILayout.Label("Default");
            param.DefaultState = EditorGUILayout.Toggle(param.DefaultState);
            GUILayout.Label("Synced");
            param.NetworkSynced = EditorGUILayout.Toggle(param.NetworkSynced);
            GUILayout.Label("Saved");
            param.Saved = EditorGUILayout.Toggle(param.Saved);
            if (GUILayout.Button("-"))
            {
                parameters.Remove(param);
            }
            GUILayout.EndHorizontal();
        } 

        // Buttons
        if (GUILayout.Button("Add toggle"))
        {
            toggles.Add(new SimpleToggle());
        }

        // Fields
        foreach (SimpleToggle toggle in toggles)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Name");
            toggle.Name = EditorGUILayout.TextField(toggle.Name);
            GUILayout.Label("GameObject");
            toggle.GameObject = (GameObject)EditorGUILayout.ObjectField(toggle.GameObject, typeof(GameObject), true);
            GUILayout.Label("Parameter");
            // Dropdown
            if (parameters.Count > 0)
            {
                string[] paramNames = parameters.Select(param => param.Name).ToArray();
                int index = parameters.FindIndex(param => param.Name == toggle.Parameter?.Name);
                index = EditorGUILayout.Popup(index, paramNames);
                toggle.Parameter = index >= 0 ? parameters[index] : null;
            }
            else
            {
                GUILayout.Label("No parameters");
            }
            if (GUILayout.Button("-"))
            {
                toggles.Remove(toggle);
            }
            GUILayout.EndHorizontal();
        }


        if (GUILayout.Button("Generate toggles for selection"))
        {
            GenerateTogglesForSelection();
        }
    }

    private void GenerateTogglesForSelection()
    {
        AnimatorControllerUtils.CreateController("Assets/HeavenVR/ToggleAssistant/Temp/Toggles/Controller.controller", toggles);
        VRCExpressionParametersUtils.CreateParameters("Assets/HeavenVR/ToggleAssistant/Temp/Toggles/ExpressionParameters.asset", parameters);
    }
}
