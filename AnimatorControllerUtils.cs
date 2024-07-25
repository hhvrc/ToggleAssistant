using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

namespace HeavenVR.ToggleAssistant.Editor
{
    public static class AnimatorControllerUtils
    {
        private static void RenameControllerParameters(AnimatorTransitionBase transition, string oldName, string newName)
        {
            for (int i = 0; i < transition.conditions.Length; i++)
            {
                if (transition.conditions[i].parameter == oldName)
                {
                    transition.conditions[i].parameter = newName;
                }
            }
        }
        private static void RenameControllerParameters(BlendTree blendTree, string oldName, string newName)
        {
            foreach (var childMotion in blendTree.children)
            {
                RenameControllerParameters(childMotion.motion, oldName, newName);
            }
        }
        private static void RenameControllerParameters(Motion motion, string oldName, string newName)
        {
            if (motion is BlendTree blendTree)
            {
                RenameControllerParameters(blendTree, oldName, newName);
            }
        }
        private static void RenameControllerParameters(AnimatorState state, string oldName, string newName)
        {
            foreach (var transition in state.transitions)
            {
                RenameControllerParameters(transition, oldName, newName);
            }

            RenameControllerParameters(state.motion, oldName, newName);
        }
        private static void RenameControllerParameters(AnimatorStateMachine stateMachine, string oldName, string newName)
        {
            foreach (var transition in stateMachine.anyStateTransitions)
            {
                RenameControllerParameters(transition, oldName, newName);
            }

            foreach (var transition in stateMachine.entryTransitions)
            {
                RenameControllerParameters(transition, oldName, newName);
            }

            foreach (var state in stateMachine.states)
            {
                RenameControllerParameters(state.state, oldName, newName);
            }
        }
        public static void RenameParameters(AnimatorController controller, string oldName, string newName)
        {
            foreach (var parameter in controller.parameters)
            {
                if (parameter.name == oldName)
                {
                    parameter.name = newName;
                }
            }

            foreach (var layer in controller.layers)
            {
                RenameControllerParameters(layer.stateMachine, oldName, newName);
            }
        }
    
        public static AnimatorController CreateController(string path, List<SimpleToggle> toggles)
        {
            // Create a animator controller
            AnimatorController controller = AnimatorController.CreateAnimatorControllerAtPath(PathUtils.EnsureParentDir(path));

            foreach (SimpleToggle toggle in toggles)
            {
                var parameter = toggle.Parameter;

                // Create a bool parameter
                controller.AddParameter(new AnimatorControllerParameter
                {
                    name = parameter.Name,
                    type = AnimatorControllerParameterType.Bool,
                    defaultBool = parameter.DefaultState
                });

                string animationPath = $"Assets/HeavenVR/ToggleAssistant/Temp/Toggles/{toggle.AnimationName}.anim";

                // Create the clip
                AnimationClip clip = AnimationClipUtils.CreateGameObjectToggle(PathUtils.EnsureParentDir(animationPath), toggle.GameObject, !parameter.DefaultState);

                controller.AddLayer(new AnimatorControllerLayer
                {
                    name = toggle.LayerName,
                    stateMachine = StateMachineUtils.CreateBooleanToggleMachine(toggle.Name, clip, parameter.Name, parameter.DefaultState),
                    defaultWeight = 1,
                });
            }

            return controller;
        }
    }
}