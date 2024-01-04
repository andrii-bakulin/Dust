using UnityEngine;

namespace Dust
{
    public static class DuVector2_Extensions
    {
        public static void duAbs(ref this Vector2 self)
        {
            self = DuVector2.Abs(self);
        }
    }
}
