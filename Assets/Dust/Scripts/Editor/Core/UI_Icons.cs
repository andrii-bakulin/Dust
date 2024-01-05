using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dust.DustEditor
{
    public partial class UI
    {
        public static class Icons
        {
            public static readonly string ADD_ACTION = "UI/Add-Action";
            public static readonly string ADD_FACTORY_MACHINE = "UI/Add-Factory-Machine";
            public static readonly string ADD_FIELD = "UI/Add-Field";

            public static readonly string ACTION_PLAY = "UI/Action-Play";
            public static readonly string ACTION_IDLE = "UI/Action-Idle";
            public static readonly string ACTION_NEXT = "UI/Action-Next";

            public static readonly string DELETE = "UI/Delete";

            public static readonly string STATE_ENABLED = "UI/State-Enabled";
            public static readonly string STATE_DISABLED = "UI/State-Disabled";

            public static readonly string GAME_OBJECT_STATS = "UI/GameObject-Stats";
            public static readonly string TRANSFORM_RESET = "UI/Transform-Reset";

            private static readonly Dictionary<string, Texture> classIconsCache = new Dictionary<string, Texture>();

            private static readonly string[] resourcePaths =
            {
                "Components/Actions/",
                "Components/Animations/", 
    #if DUST_NEW_FEATURE_DEFORMER
                "Components/Deformers/",
    #endif
                "Components/Events/",
                "Components/Factory/",
                "Components/FactoryMachines/",
                "Components/Fields/",
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
}
