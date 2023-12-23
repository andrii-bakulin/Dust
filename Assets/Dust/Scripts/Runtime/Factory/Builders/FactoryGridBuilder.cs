using UnityEngine;

namespace DustEngine
{
    public class FactoryGridBuilder : FactoryBuilder
    {
        public override void Initialize(Factory factory)
        {
            var gridFactory = factory as GridFactory;

            base.Initialize(factory);

            Vector3Int gridCount = Vector3Int.Max(Vector3Int.one, gridFactory.count);

            Vector3 zeroPoint;
            zeroPoint.x = -(gridCount.x - 1) / 2f * gridFactory.step.x;
            zeroPoint.y = -(gridCount.y - 1) / 2f * gridFactory.step.y;
            zeroPoint.z = -(gridCount.z - 1) / 2f * gridFactory.step.z;

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            for (int z = 0; z < gridFactory.count.z; z++)
            for (int y = 0; y < gridFactory.count.y; y++)
            for (int x = 0; x < gridFactory.count.x; x++)
            {
                var instanceState = new FactoryInstance.State();

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
                // Set position/rotation/scale

                Vector3 offsetByDirection = Vector3.zero;

                switch (gridFactory.offsetDirection)
                {
                    case GridFactory.OffsetDirection.Disabled:
                    default:
                        // Nothing to do
                        break;

                    case GridFactory.OffsetDirection.X:
                        offsetByDirection.x += y % 2 != 0 ? gridFactory.step.x * gridFactory.offsetHeight : 0f;
                        offsetByDirection.x += z % 2 != 0 ? gridFactory.step.x * gridFactory.offsetWidth  : 0f;
                        break;

                    case GridFactory.OffsetDirection.Y:
                        offsetByDirection.y += x % 2 != 0 ? gridFactory.step.y * gridFactory.offsetWidth  : 0f;
                        offsetByDirection.y += z % 2 != 0 ? gridFactory.step.y * gridFactory.offsetHeight : 0f;
                        break;

                    case GridFactory.OffsetDirection.Z:
                        offsetByDirection.z += x % 2 != 0 ? gridFactory.step.z * gridFactory.offsetWidth  : 0f;
                        offsetByDirection.z += y % 2 != 0 ? gridFactory.step.z * gridFactory.offsetHeight : 0f;
                        break;
                }

                instanceState.position = new Vector3(
                    zeroPoint.x + gridFactory.step.x * x + offsetByDirection.x,
                    zeroPoint.y + gridFactory.step.y * y + offsetByDirection.y,
                    zeroPoint.z + gridFactory.step.z * z + offsetByDirection.z);

                instanceState.rotation = Vector3.zero;
                instanceState.scale = Vector3.one;

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
                // UV

                float uvwX = gridCount.x > 1 ? 1f / (gridCount.x - 1) : 0f;
                float uvwY = gridCount.y > 1 ? 1f / (gridCount.y - 1) : 0f;
                float uvwZ = gridCount.z > 1 ? 1f / (gridCount.z - 1) : 0f;

                instanceState.uvw = new Vector3(x * uvwX, y * uvwY, z * uvwZ);

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                m_InstancesStates.Add(instanceState);
            }
            // end of for:3x
        }
    }
}
