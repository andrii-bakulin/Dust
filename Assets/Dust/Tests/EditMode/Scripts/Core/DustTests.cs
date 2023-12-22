using System;
using DustEngine;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace DustEngine.Test.EditMode
{
    public class DustTests
    {
        //--------------------------------------------------------------------------------------------------------------

        [Test]
        public void IsNull_ShouldTrue_WhenSystemObjectIsNull()
        {
            String sut = null;
            Assert.That(Dust.IsNull(sut), Is.True);
        }

        [Test]
        public void IsNull_ShouldFalse_WhenSystemObjectIsText()
        {
            String sut = "Test";
            Assert.That(Dust.IsNull(sut), Is.False);
        }

        [Test]
        public void IsNotNull_ShouldTrue_WhenSystemObjectIsText()
        {
            String sut = "Test";
            Assert.That(Dust.IsNotNull(sut), Is.True);
        }

        [Test]
        public void IsNotNull_ShouldFalse_WhenSystemObjectIsNull()
        {
            String sut = null;
            Assert.That(Dust.IsNotNull(sut), Is.False);
        }

        //--------------------------------------------------------------------------------------------------------------

        [Test]
        public void IsNull_ShouldTrue_WhenUnityEngineObjectIsNull()
        {
            var gameObject = new GameObject();
            var sut = gameObject.GetComponent<Collider2D>(); // Collider2D definitely NOT EXISTS in empty GameObject

            Assert.That(Dust.IsNull(sut), Is.True);

            UnityEngine.Object.DestroyImmediate(gameObject);
        }

        [Test]
        public void IsNull_ShouldFalse_WhenUnityEngineObjectIsTransform()
        {
            var gameObject = new GameObject();
            var sut = gameObject.GetComponent<Transform>(); // Transform definitely EXISTS in empty GameObject

            Assert.That(Dust.IsNull(sut), Is.False);

            UnityEngine.Object.DestroyImmediate(gameObject);
        }

        [Test]
        public void IsNotNull_ShouldTrue_WhenUnityEngineObjectIsTransform()
        {
            var gameObject = new GameObject();
            var sut = gameObject.GetComponent<Transform>(); // Transform definitely EXISTS in empty GameObject

            Assert.That(Dust.IsNotNull(sut), Is.True);

            UnityEngine.Object.DestroyImmediate(gameObject);
        }

        [Test]
        public void IsNotNull_ShouldFalse_WhenUnityEngineObjectIsNull()
        {
            var gameObject = new GameObject();
            var sut = gameObject.GetComponent<Collider2D>(); // Collider2D definitely NOT EXISTS in empty GameObject

            Assert.That(Dust.IsNotNull(sut), Is.False);

            UnityEngine.Object.DestroyImmediate(gameObject);
        }
    }
}
