using UnityEngine;

namespace DustEngine
{
    public interface IDynamicState
    {
        int GetDynamicStateHashCode();
    }

    public static class DynamicState
    {
        public static int Normalize(int state)
        {
            return state != 0 ? state : 1;
        }

        //--------------------------------------------------------------------------------------------------------------

        public static void Append(ref int dynamicState, int sequenceIndex, bool value)
        {
            dynamicState ^= sequenceIndex * 854837 + value.GetHashCode();
        }

        public static void Append(ref int dynamicState, int sequenceIndex, int value)
        {
            dynamicState ^= sequenceIndex * 330177 + value.GetHashCode();
        }

        public static void Append(ref int dynamicState, int sequenceIndex, System.Enum value)
        {
            dynamicState ^= sequenceIndex * 366250 + value.GetHashCode();
        }

        public static void Append(ref int dynamicState, int sequenceIndex, float value)
        {
            dynamicState ^= sequenceIndex * 974003 + value.GetHashCode();
        }

        public static void Append(ref int dynamicState, int sequenceIndex, Vector3 value)
        {
            dynamicState ^= sequenceIndex * 575673 + value.x.GetHashCode();
            dynamicState ^= sequenceIndex * 124751 + value.y.GetHashCode();
            dynamicState ^= sequenceIndex * 917254 + value.z.GetHashCode();
        }

        public static void Append(ref int dynamicState, int sequenceIndex, Color value)
        {
            dynamicState ^= sequenceIndex * 625751 + value.r.GetHashCode();
            dynamicState ^= sequenceIndex * 328127 + value.g.GetHashCode();
            dynamicState ^= sequenceIndex * 874751 + value.b.GetHashCode();
            dynamicState ^= sequenceIndex * 127345 + value.a.GetHashCode();
        }

        public static void Append(ref int dynamicState, int sequenceIndex, Gradient value)
        {
            dynamicState ^= sequenceIndex * 238715 + (Dust.IsNotNull(value) ? value.GetHashCode() : 123456);
        }

        public static void Append(ref int dynamicState, int sequenceIndex, AnimationCurve value)
        {
            dynamicState ^= sequenceIndex * 772937 + (Dust.IsNotNull(value) ? value.GetHashCode() : 123456);
        }

        //--------------------------------------------------------------------------------------------------------------

        public static void Append(ref int dynamicState, int sequenceIndex, GameObject gameObject)
        {
            if (Dust.IsNotNull(gameObject))
            {
                Append(ref dynamicState, sequenceIndex, gameObject.transform);
            }
            else
            {
                dynamicState ^= 123456;
            }
        }

        public static void Append(ref int dynamicState, int sequenceIndex, Transform transform)
        {
            if (Dust.IsNotNull(transform))
            {
                dynamicState ^= sequenceIndex * 784449 + transform.position.GetHashCode();
                dynamicState ^= sequenceIndex * 807525 + transform.rotation.GetHashCode();
                dynamicState ^= sequenceIndex * 371238 + transform.lossyScale.GetHashCode();
            }
            else
            {
                dynamicState ^= sequenceIndex * 784449 + 123456;
                dynamicState ^= sequenceIndex * 807525 + 123456;
                dynamicState ^= sequenceIndex * 371238 + 123456;
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        public static void Append(ref int dynamicState, int sequenceIndex, Remapping remapping)
        {
            dynamicState ^= sequenceIndex * 291422 + (Dust.IsNotNull(remapping) ? remapping.GetDynamicStateHashCode() : 123456);
        }

#if DUST_NEW_FEATURE_DEFORMER
        public static void Append(ref int dynamicState, int sequenceIndex, DuDeformer deformer)
        {
            dynamicState ^= sequenceIndex * 291422 + (Dust.IsNotNull(deformer) ? deformer.GetDynamicStateHashCode() : 123456);
        }
#endif

        public static void Append(ref int dynamicState, int sequenceIndex, FieldsMap fieldsMap)
        {
            dynamicState ^= sequenceIndex * 955735 + (Dust.IsNotNull(fieldsMap) ? fieldsMap.GetDynamicStateHashCode() : 123456);
        }

        public static void Append(ref int dynamicState, int sequenceIndex, Field field)
        {
            dynamicState ^= sequenceIndex * 512661 + (Dust.IsNotNull(field) ? field.GetDynamicStateHashCode() : 123456);
        }

#if DUST_NEW_FEATURE_FACTORY
        public static void Append(ref int dynamicState, int sequenceIndex, FactoryMachine factoryMachine)
        {
            dynamicState ^= sequenceIndex * 814356 + (Dust.IsNotNull(factoryMachine) ? factoryMachine.GetDynamicStateHashCode() : 123456);
        }
#endif

        //--------------------------------------------------------------------------------------------------------------

        public static void Append(ref int dynamicState, int sequenceIndex, Mesh mesh)
        {
            dynamicState ^= sequenceIndex * 848409 + (Dust.IsNotNull(mesh) ? mesh.GetHashCode() : 123456);
        }

        public static void Append(ref int dynamicState, int sequenceIndex, Texture2D texture2D)
        {
            dynamicState ^= sequenceIndex * 828581 + (Dust.IsNotNull(texture2D) ? texture2D.GetHashCode() : 123456);
        }

        //--------------------------------------------------------------------------------------------------------------

        public static void AppendRandomState(ref int dynamicState)
        {
            dynamicState ^= Random.Range(-1000000, +1000000);
        }
    }
}
