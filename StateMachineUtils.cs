using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

namespace HeavenVR.ToggleAssistant.Editor
{
    public static class StateMachineUtils
    {
        private static AnimatorStateMachine CreateStateMachine(string name)
        {
            return new AnimatorStateMachine
            {
                name = name,
                entryPosition = new Vector3(50, 120),
                anyStatePosition = new Vector3(50, 160),
                exitPosition = new Vector3(50, 200)
            };
        }
        private static AnimatorStateTransition CreateStateTransition(AnimatorState source, AnimatorState destination)
        {
            var transition = source.AddTransition(destination, true);
            transition.hasExitTime = false;
            transition.exitTime = 0;
            transition.duration = 0.1f;
            return transition;
        }
        public static AnimatorStateMachine CreateBooleanToggleMachine(string name, Motion activeMotion, string paramName, bool paramValueDefault)
        {
            var stateMachine = CreateStateMachine(name);

            AnimatorState defaultState = stateMachine.AddState("Default", new Vector3(240, 120));
            AnimatorState activeState = stateMachine.AddState("Active", new Vector3(470, 120));

            defaultState.writeDefaultValues = true;
            activeState.writeDefaultValues = true;
            activeState.motion = activeMotion;

            AnimatorState enabledState;
            AnimatorState disabledState;
            if (paramValueDefault)
            {
                enabledState = defaultState;
                disabledState = activeState;
            }
            else
            {
                enabledState = activeState;
                disabledState = defaultState;
            }

            enabledState.name = "Enabled";
            disabledState.name = "Disabled";

            var enableTransition = CreateStateTransition(disabledState, enabledState);
            enableTransition.AddCondition(AnimatorConditionMode.If, 0, paramName);

            var disableTransition = CreateStateTransition(enabledState, disabledState);
            disableTransition.AddCondition(AnimatorConditionMode.IfNot, 0, paramName);

            return stateMachine;
        }
    }
}