using System.IO;
using UnityEditor;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

namespace HeavenVR.ToggleAssistant.Editor
{
    public static class PathUtils
    {
        public static string GetPath(GameObject gameObject)
        {
            string path = gameObject.name;

            Transform parent = gameObject.transform.parent;

            // Traverse up the hierarchy until avatar is reached
            while (parent != null && parent.GetComponent<VRCAvatarDescriptor>() == null)
            {
                path = parent.name + "/" + path;
                parent = parent.parent;
            }

            return path;
        }

        public static string EnsureParentDir(string path)
        {
            // Get parent directory
            var pathParent = Path.GetDirectoryName(path);

            // Check if path exists
            if (!AssetDatabase.IsValidFolder(pathParent))
            {
                // Separate path into folders
                string[] folders = pathParent.Split('\\', '/');

                if (folders.Length < 1 || folders[0] != "Assets") throw new System.ArgumentException("Path must start with 'Assets'" + pathParent);

                // Create folders
                for (int i = 1; i < folders.Length; i++)
                {
                    string parent = string.Join("/", folders, 0, i);

                    if (string.IsNullOrEmpty(AssetDatabase.CreateFolder(parent, folders[i])))
                    {
                        throw new System.Exception("Failed to create folder: " + folders[i]);
                    }
                }
            }

            return path;
        }
    }
}