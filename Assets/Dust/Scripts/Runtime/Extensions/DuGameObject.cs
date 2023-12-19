using System;
using UnityEngine;

namespace DustEngine
{
    public class DuGameObject
    {
        public struct Data : IEquatable<Data>
        {
            public int meshesCount { get; set; }
            public int vertexCount { get; set; }
            public int triangleCount { get; set; }
            public int unreadableCount { get; set; }

            public static Data operator +(Data a, Data b)
            {
                a.meshesCount += b.meshesCount;
                a.vertexCount += b.vertexCount;
                a.triangleCount += b.triangleCount;
                a.unreadableCount += b.unreadableCount;
                return a;
            }

            public bool Equals(Data other)
            {
                throw new NotImplementedException();
            }
        }

        public static Data GetStats(GameObject gameObject)
            => GetStats(gameObject, false);

        public static Data GetStats(GameObject gameObject, bool recursive)
        {
            Data result = new Data();

            if (recursive)
            {
                MeshFilter[] meshFilters = gameObject.GetComponentsInChildren<MeshFilter>();

                foreach (var meshFilter in meshFilters)
                    result += ReadData(meshFilter);
            }
            else
            {
                result = ReadData(gameObject.GetComponent<MeshFilter>());
            }

            return result;
        }

        protected static Data ReadData(MeshFilter meshFilter)
        {
            Data result = new Data();

            if (Dust.IsNotNull(meshFilter) && Dust.IsNotNull(meshFilter.sharedMesh))
            {
                var mesh = meshFilter.sharedMesh;

                result.vertexCount += mesh.vertexCount;

                if (mesh.isReadable)
                    result.triangleCount += mesh.triangles.Length / 3;
                else
                    result.unreadableCount++;

                result.meshesCount = mesh.subMeshCount;
            }

            return result;
        }
    }
}
