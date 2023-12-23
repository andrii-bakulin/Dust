using UnityEditor;

namespace DustEngine.DustEditor
{
    public static class EditorHelper
    {
        internal static void OptimizeObjectReferencesArray(ref DuEditor.DuProperty itemsListProperty, string refObjectKey)
        {
            bool changed = false;

            for (int i = itemsListProperty.property.arraySize - 1; i >= 0; i--)
            {
                SerializedProperty record = itemsListProperty.property.GetArrayElementAtIndex(i);

                if (Dust.IsNotNull(record))
                {
                    SerializedProperty refObject = record.FindPropertyRelative(refObjectKey);

                    if (Dust.IsNotNull(refObject) && Dust.IsNotNull(refObject.objectReferenceValue))
                        continue;
                }

                itemsListProperty.property.DeleteArrayElementAtIndex(i);
                changed = true;
            }

            if (!changed)
                return;

            itemsListProperty.parentEditor.serializedObject.ApplyModifiedProperties();
        }
    }
}
