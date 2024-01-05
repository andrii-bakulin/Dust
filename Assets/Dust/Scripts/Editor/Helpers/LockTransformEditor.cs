using UnityEngine;
using UnityEditor;

namespace Dust.DustEditor
{
    [CustomEditor(typeof(LockTransform))]
    [CanEditMultipleObjects]
    public class LockTransformEditor : DuEditor
    {
        protected DuProperty m_LockPosition;
        protected DuProperty m_LockRotation;
        protected DuProperty m_LockScale;

        protected DuProperty m_Position;
        protected DuProperty m_Rotation;
        protected DuProperty m_Scale;

        protected DuProperty m_Space;

        //--------------------------------------------------------------------------------------------------------------

        [MenuItem("Dust/Helpers/Lock Transform")]
        [MenuItem("GameObject/Dust/Helpers/Lock Transform")]
        public static void AddComponentToSelectedObjects()
        {
            AddComponentToSelectedObjects(typeof(LockTransform));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void InitializeEditor()
        {
            base.InitializeEditor();

            m_LockPosition = FindProperty("m_LockPosition", "Lock Position");
            m_LockRotation = FindProperty("m_LockRotation", "Lock Rotation");
            m_LockScale = FindProperty("m_LockScale", "Lock Scale");

            m_Position = FindProperty("m_Position", "Position");
            m_Rotation = FindProperty("m_Rotation", "Rotation");
            m_Scale = FindProperty("m_Scale", "Scale");

            m_Space = FindProperty("m_Space", "Lock In Space");
        }

        public override void OnInspectorGUI()
        {
            var main = target as LockTransform;

            InspectorInitStates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PropertyField(m_LockPosition);
            PropertyField(m_LockRotation);
            PropertyField(m_LockScale);

            Space();

            PropertyField(m_Space);

            if (targets.Length == 1)
            {
                Space();

                if (DustGUI.FoldoutBegin("Lock States At", "LockTransform.LockStatesAt"))
                {
                    DustGUI.Lock();

                    if (main.enabled && (m_LockPosition.IsTrue || m_LockRotation.IsTrue || m_LockScale.IsTrue))
                    {
                        var space = (DuTransform.Space) m_Space.valInt;

                        if (m_LockPosition.IsTrue)
                        {
                            string title = space == DuTransform.Space.Local
                                ? "Local Position"
                                : "World Position";
                            DustGUI.Field(title, m_Position.valVector3);
                        }

                        if (m_LockRotation.IsTrue)
                        {
                            string title = space == DuTransform.Space.Local
                                ? "Local Rotation"
                                : "World Rotation";
                            DustGUI.Field(title, m_Rotation.valQuaternion.eulerAngles);
                        }

                        if (m_LockScale.IsTrue)
                        {
                            string title = space == DuTransform.Space.Local
                                ? "Local Scale"
                                : "Local* Scale";
                            DustGUI.Field(title, m_Scale.valVector3);
                        }
                    }
                    else
                    {
                        DustGUI.Label("Transform is not locked");
                    }

                    DustGUI.Unlock();
                }
                DustGUI.FoldoutEnd();
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            InspectorCommitUpdates();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            bool reLockState = false;
            reLockState |= m_LockPosition.isChanged;
            reLockState |= m_LockRotation.isChanged;
            reLockState |= m_LockScale.isChanged;
            reLockState |= m_Space.isChanged;

            if (reLockState)
            {
                foreach (var subTarget in targets)
                {
                    var origin = subTarget as LockTransform;

                    origin.FixLockStates();
                }
            }
        }
    }
}
