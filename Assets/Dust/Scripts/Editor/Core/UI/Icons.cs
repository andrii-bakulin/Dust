using System.Collections.Generic;
using UnityEngine;

namespace DustEngine.DustEditor.UI
{
    public static class Icons
    {
        // public const string ACTION_DELETE = "UI/Action-Delete";
        public const string ACTION_ADD_ACTION = "UI/Add-Action";
        // public const string ACTION_ADD_FACTORY_MACHINE = "UI/Add-Factory-Machine";
        // public const string ACTION_ADD_FIELD = "UI/Add-Field";

        public const string ACTION_PLAY = "UI/Action-Play";
        public const string ACTION_IDLE = "UI/Action-Idle";
        public const string ACTION_NEXT = "UI/Action-Next";

        // public const string STATE_ENABLED = "UI/State-Enabled";
        // public const string STATE_DISABLED = "UI/State-Disabled";

        public const string GAME_OBJECT_STATS = "UI/GameObject-Stats";
        public const string TRANSFORM_RESET = "UI/Transform-Reset";

        private static readonly Dictionary<string, Texture> classIconsCache = new Dictionary<string, Texture>();

        private static readonly string[] resourcePaths =
        {
            "Components/Actions/",
            "Components/Animations/",
            "Components/Events/",
            "Components/Gizmos/",
            "Components/Helpers/",
            "Components/Instances/",
            "Components/",
        };

        //--------------------------------------------------------------------------------------------------------------

        public static Texture GetTextureByComponent(Component component)
            => GetTextureByComponent(component, "");

        public static Texture GetTextureByComponent(Component component, string suffix)
        {
            if (Dust.IsNull(component))
                return null;

            return GetTextureByClassName(component.GetType().ToString(), suffix);
        }

        public static Texture GetTextureByClassName(string className)
            => GetTextureByClassName(className, "");

        public static Texture GetTextureByClassName(string className, string suffix)
        {
            string classNameId = className + "::" + suffix;

            if (!classIconsCache.ContainsKey(classNameId))
            {
                string resourceFilename = className;

                if (suffix != "")
                    resourceFilename += "-" + suffix;

                foreach (var resourcePath in resourcePaths)
                {
                    var resourceId = resourcePath + resourceFilename;

                    classIconsCache[className] = Resources.Load(resourceId) as Texture;

                    if (Dust.IsNotNull(classIconsCache[className]))
                        break;
                }
            }

            return classIconsCache[className];
        }
    }
}
