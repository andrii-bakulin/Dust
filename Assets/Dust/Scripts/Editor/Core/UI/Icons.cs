using System.Collections.Generic;
using UnityEngine;

namespace DustEngine.DustEditor.UI
{
    public static class Icons
    {
        // internal const string ACTION_DELETE = "UI/Action-Delete";
        internal const string ACTION_ADD_ACTION = "UI/Add-Action";
        // internal const string ACTION_ADD_FACTORY_MACHINE = "UI/Add-Factory-Machine";
        // internal const string ACTION_ADD_FIELD = "UI/Add-Field";

        internal const string ACTION_PLAY = "UI/Action-Play";
        internal const string ACTION_IDLE = "UI/Action-Idle";
        internal const string ACTION_NEXT = "UI/Action-Next";

        // internal const string STATE_ENABLED = "UI/State-Enabled";
        // internal const string STATE_DISABLED = "UI/State-Disabled";

        internal const string GAME_OBJECT_STATS = "UI/GameObject-Stats";
        internal const string TRANSFORM_RESET = "UI/Transform-Reset";

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
