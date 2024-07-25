using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

namespace HeavenVR.ToggleAssistant.Editor
{
    public static class AnimationClipUtils
    {
        public static AnimationClip CreateGameObjectToggle(string path, GameObject gameObject, bool enable)
        {
            var curve = new AnimationCurve();

            float value = enable ? 1f : 0f;

            curve.AddKey(0, value);
            curve.AddKey(0.01f, value);

            var clip = new AnimationClip();

            clip.SetCurve(PathUtils.GetPath(gameObject), typeof(GameObject), "m_IsActive", curve);

            // Save the clip to temporary location
            AssetDatabase.CreateAsset(clip, path);

            return clip;
        }

    }
}