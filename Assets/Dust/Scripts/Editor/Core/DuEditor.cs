using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    public class DuEditor : Editor
    {
        public class DuProperty
        {
            internal string propertyPath;
            internal string title;
            internal string tooltip;
            internal SerializedProperty property;
            internal bool isChanged;

            internal Editor parentEditor;

            //--------------------------------------------------------------------------------------------------------------
            // Helpers

            public SerializedProperty FindInnerProperty(string relativePropertyPath)
            {
                return property.FindPropertyRelative(relativePropertyPath);
            }

            public bool IsTrue => property.propertyType == SerializedPropertyType.Boolean ? property.boolValue : false;

            public bool valBool
            {
                get => property.boolValue;
                set => property.boolValue = value;
            }

            public int valInt
            {
                get => property.intValue;
                set => property.intValue = value;
            }

            public float valFloat
            {
                get => property.floatValue;
                set => property.floatValue = value;
            }

            public string valString
            {
                get => property.stringValue;
                set => property.stringValue = value;
            }

            public Vector3 valVector3
            {
                get => property.vector3Value;
                set => property.vector3Value = value;
            }

            public Vector3Int valVector3Int
            {
                get => property.vector3IntValue;
                set => property.vector3IntValue = value;
            }

            public Quaternion valQuaternion
            {
                get => property.quaternionValue;
                set => property.quaternionValue = value;
            }

            public AnimationCurve valAnimationCurve
            {
                get => property.animationCurveValue;
                set => property.animationCurveValue = value;
            }

            public Color valColor
            {
                get => property.colorValue;
                set => property.colorValue = value;
            }

            public SerializedProperty valUnityEvent
            {
                get => property.FindPropertyRelative("m_PersistentCalls.m_Calls");
            }

            public GameObject GameObjectReference => property.objectReferenceValue as GameObject;

            public bool ObjectReferenceExists => Dust.IsNotNull(property.objectReferenceValue);
        }

        //--------------------------------------------------------------------------------------------------------------

        public DuProperty FindProperty(string propertyPath)
            => FindProperty(propertyPath, "", "");

        public DuProperty FindProperty(string propertyPath, string title)
            => FindProperty(propertyPath, title, "");

        public DuProperty FindProperty(string propertyPath, string title, string tooltip)
        {
            var duProperty = new DuProperty
            {
                propertyPath = propertyPath,
                title = title,
                tooltip = tooltip,
                property = serializedObject.FindProperty(propertyPath),
                isChanged = false,
                parentEditor = this
            };
            return duProperty;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public static DuProperty FindProperty(SerializedObject parentObject, string propertyPath)
            => FindProperty(null, parentObject, propertyPath, "", "");

        public static DuProperty FindProperty(SerializedObject parentObject, string propertyPath, string title)
            => FindProperty(null, parentObject, propertyPath, title, "");

        public static DuProperty FindProperty(DuEditor parentEditor, SerializedObject parentObject, string propertyPath, string title)
            => FindProperty(parentEditor, parentObject, propertyPath, title, "");

        public static DuProperty FindProperty(DuEditor parentEditor, SerializedObject parentObject, string propertyPath, string title, string tooltip)
        {
            var duProperty = new DuProperty
            {
                propertyPath = propertyPath,
                title = title,
                tooltip = tooltip,
                property = parentObject.FindProperty(propertyPath),
                isChanged = false,
                parentEditor = parentEditor,
            };
            return duProperty;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public static DuProperty FindProperty(SerializedProperty parentProperty, string propertyPath)
            => FindProperty(null, parentProperty, propertyPath, "", "");

        public static DuProperty FindProperty(SerializedProperty parentProperty, string propertyPath, string title)
            => FindProperty(null, parentProperty, propertyPath, title, "");

        public static DuProperty FindProperty(DuEditor parentEditor, SerializedProperty parentProperty, string propertyPath, string title)
            => FindProperty(parentEditor, parentProperty, propertyPath, title, "");

        public static DuProperty FindProperty(DuEditor parentEditor, SerializedProperty parentProperty, string propertyPath, string title, string tooltip)
        {
            var duProperty = new DuProperty
            {
                propertyPath = propertyPath,
                title = title,
                tooltip = tooltip,
                property = parentProperty.FindPropertyRelative(propertyPath),
                isChanged = false,
                parentEditor = parentEditor,
            };
            return duProperty;
        }

        //--------------------------------------------------------------------------------------------------------------

        public class SerializedEntity
        {
            internal Object target;
            internal SerializedObject serializedObject;
        }

        // If I change some parameters for list of targets then I need to create SerializedObject for each target.
        // BUT only for self-target I need to use self-serializedObject object!
        public SerializedEntity[] GetSerializedEntitiesByTargets()
        {
            return GetSerializedEntitiesByTargets(this);
        }

        public static SerializedEntity[] GetSerializedEntitiesByTargets(DuEditor targetEditor)
        {
            var serializedTargets = new SerializedEntity[targetEditor.targets.Length];

            for (int i = 0; i < targetEditor.targets.Length; i++)
            {
                serializedTargets[i] = new SerializedEntity
                {
                    target = targetEditor.targets[i],
                    serializedObject = targetEditor.targets[i] == targetEditor.target
                        ? targetEditor.serializedObject :
                        new SerializedObject(targetEditor.targets[i])
                };
            }

            return serializedTargets;
        }

        //--------------------------------------------------------------------------------------------------------------

        public static void AddComponentToSelectedObjects(System.Type duComponentType)
        {
            if (Selection.gameObjects.Length == 0)
                return;

            foreach (var gameObject in Selection.gameObjects)
            {
                Undo.AddComponent(gameObject, duComponentType);
            }
        }

        public static void AddComponentToSelectedOrNewObject(string gameObjectName, System.Type duComponentType)
        {
            if (Selection.gameObjects.Length > 0)
            {
                foreach (var gameObject in Selection.gameObjects)
                {
                    Undo.AddComponent(gameObject, duComponentType);
                }
            }
            else
            {
                AddComponentToNewObject(gameObjectName, duComponentType);
            }
        }

        public static Component AddComponentToNewObject(string gameObjectName, System.Type duComponentType)
            => AddComponentToNewObject(gameObjectName, duComponentType, true);

        public static Component AddComponentToNewObject(string gameObjectName, System.Type duComponentType, bool fixUndoState)
        {
            var gameObject = new GameObject();
            gameObject.name = gameObjectName;

            if (Dust.IsNotNull(Selection.activeGameObject))
                gameObject.transform.parent = Selection.activeGameObject.transform;

            DuTransform.Reset(gameObject.transform);

            var component = gameObject.AddComponent(duComponentType);

            if (fixUndoState)
                Undo.RegisterCreatedObjectUndo(gameObject, "Create " + gameObject.name);

            Selection.activeGameObject = gameObject;
            return component;
        }

        protected static bool IsAllowExecMenuCommandOnce(MenuCommand menuCommand)
        {
            return Selection.objects.Length == 0 || menuCommand.context == Selection.objects[0];
        }

        //--------------------------------------------------------------------------------------------------------------

        // Notice: on redefine OnEnable() method require manually call InitializeEditor();
        void OnEnable()
        {
            InitializeEditor();
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        protected virtual void InitializeEditor()
        {

        }

        protected virtual void InspectorInitStates()
        {
            serializedObject.Update();
        }

        protected virtual void InspectorCommitUpdates()
        {
            serializedObject.ApplyModifiedProperties();
        }

        //--------------------------------------------------------------------------------------------------------------

        public static bool PropertyField(DuProperty duProperty, string label)
            => PropertyField(duProperty, label, "");

        public static bool PropertyField(DuProperty duProperty, string label, string tooltip)
        {
            if (Dust.IsNull(duProperty.property))
            {
                Dust.Debug.Warning("DuProperty[" + duProperty.propertyPath + "] is null");
                return false;
            }

            EditorGUI.BeginChangeCheck();
            DustGUI.Field(new GUIContent(label, tooltip), duProperty.property);
            duProperty.isChanged = EditorGUI.EndChangeCheck();
            return duProperty.isChanged;
        }

        public static bool PropertyFieldOrLock(DuProperty duProperty, bool isLocked, string label)
            => PropertyFieldOrLock(duProperty, isLocked, label, "");

        public static bool PropertyFieldOrLock(DuProperty duProperty, bool isLocked, string label, string tooltip)
        {
            if (isLocked) DustGUI.Lock();
            PropertyField(duProperty, label, tooltip);
            if (isLocked) DustGUI.Unlock();
            return duProperty.isChanged;
        }

        public static bool PropertyFieldOrHide(DuProperty duProperty, bool isHidden, string label)
            => PropertyFieldOrHide(duProperty, isHidden, label, "");

        public static bool PropertyFieldOrHide(DuProperty duProperty, bool isHidden, string label, string tooltip)
        {
            if (isHidden)
                return false;

            return PropertyField(duProperty, label, tooltip);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public static bool PropertyField(DuProperty duProperty)
        {
            return PropertyField(duProperty, duProperty.title, duProperty.tooltip);
        }

        public static bool PropertyFieldOrLock(DuProperty duProperty, bool isLocked)
        {
            return PropertyFieldOrLock(duProperty, isLocked, duProperty.title, duProperty.tooltip);
        }

        public static bool PropertyFieldOrHide(DuProperty duProperty, bool isHidden)
        {
            return PropertyFieldOrHide(duProperty, isHidden, duProperty.title, duProperty.tooltip);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public static bool PropertyDurationField(DuProperty duProperty)
            => PropertyDurationField(duProperty, 10f);

        public static bool PropertyDurationField(DuProperty duProperty, float rightValue)
        {
            PropertyExtendedSlider(duProperty, 0.0f, rightValue, 0.01f, 0.0f, float.MaxValue);
            
            if (duProperty.isChanged)
                duProperty.valFloat = Mathf.Clamp(duProperty.valFloat, 0f, float.MaxValue);

            return duProperty.isChanged;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public static bool PropertyFieldRange(DuProperty duProperty)
            => PropertyFieldRange(duProperty, "Range Min", "Range Max");

        public static bool PropertyFieldRange(DuProperty duProperty, string minTitle, string maxTitle)
        {
            if (Dust.IsNull(duProperty.property))
            {
                Dust.Debug.Warning("DuProperty[" + duProperty.propertyPath + "] is null");
                return false;
            }

            duProperty.isChanged |= PropertyField(duProperty.FindInnerProperty("m_Min"), minTitle);
            duProperty.isChanged |= PropertyField(duProperty.FindInnerProperty("m_Max"), maxTitle);
            return duProperty.isChanged;
        }

        public bool PropertyFieldRange(DuProperty duProperty,
            string minTitle, int minLeftValue, int minRightValue, int minStepValue, int minLeftLimit, int minRightLimit,  
            string maxTitle, int maxLeftValue, int maxRightValue, int maxStepValue, int maxLeftLimit, int maxRightLimit)
        {
            if (Dust.IsNull(duProperty.property))
            {
                Dust.Debug.Warning("DuProperty[" + duProperty.propertyPath + "] is null");
                return false;
            }

            var duMinProperty = FindProperty(this, duProperty.property, "m_Min", minTitle);
            var duMaxProperty = FindProperty(this, duProperty.property, "m_Max", maxTitle);

            duProperty.isChanged |= PropertyExtendedIntSlider(duMinProperty, 
                minLeftValue, minRightValue, minStepValue,
                minLeftLimit, minRightLimit);

            duProperty.isChanged |= PropertyExtendedIntSlider(duMaxProperty, 
                maxLeftValue, maxRightValue, maxStepValue,
                maxLeftLimit, maxRightLimit);

            if (duMinProperty.isChanged)
            {
                duMinProperty.valInt = Mathf.Clamp(duMinProperty.valInt, minLeftLimit, minRightLimit);

                if (duMinProperty.valInt > duMaxProperty.valInt)
                    duMaxProperty.valInt = duMinProperty.valInt;
            }
            
            if (duMaxProperty.isChanged)
            {
                duMaxProperty.valInt = Mathf.Clamp(duMaxProperty.valInt, maxLeftLimit, maxRightLimit);

                if (duMinProperty.valInt > duMaxProperty.valInt)
                    duMinProperty.valInt = duMaxProperty.valInt;
            }
            
            return duProperty.isChanged;
        }
        
        public bool PropertyFieldRange(DuProperty duProperty,
            int leftValue, int rightValue, int stepValue, int leftLimit, int rightLimit)
            => PropertyFieldRange(duProperty,
                duProperty.title + " Min", leftValue, rightValue, stepValue, leftLimit, rightLimit,
                duProperty.title + " Max", leftValue, rightValue, stepValue, leftLimit, rightLimit);

        public bool PropertyFieldRange(DuProperty duProperty,
            string minTitle, string maxTitle,
            int leftValue, int rightValue, int stepValue, int leftLimit, int rightLimit)
            => PropertyFieldRange(duProperty,
                minTitle, leftValue, rightValue, stepValue, leftLimit, rightLimit,
                maxTitle, leftValue, rightValue, stepValue, leftLimit, rightLimit);

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public bool PropertyFieldRange(DuProperty duProperty,
            string minTitle, float minLeftValue, float minRightValue, float minStepValue, float minLeftLimit, float minRightLimit,  
            string maxTitle, float maxLeftValue, float maxRightValue, float maxStepValue, float maxLeftLimit, float maxRightLimit)
        {
            if (Dust.IsNull(duProperty.property))
            {
                Dust.Debug.Warning("DuProperty[" + duProperty.propertyPath + "] is null");
                return false;
            }

            var duMinProperty = FindProperty(this, duProperty.property, "m_Min", minTitle);
            var duMaxProperty = FindProperty(this, duProperty.property, "m_Max", maxTitle);

            duProperty.isChanged |= PropertyExtendedSlider(duMinProperty, 
                minLeftValue, minRightValue, minStepValue,
                minLeftLimit, minRightLimit);

            duProperty.isChanged |= PropertyExtendedSlider(duMaxProperty, 
                maxLeftValue, maxRightValue, maxStepValue,
                maxLeftLimit, maxRightLimit);

            if (duMinProperty.isChanged)
            {
                duMinProperty.valFloat = Mathf.Clamp(duMinProperty.valFloat, minLeftLimit, minRightLimit);

                if (duMinProperty.valFloat > duMaxProperty.valFloat)
                    duMaxProperty.valFloat = duMinProperty.valFloat;
            }
            
            if (duMaxProperty.isChanged)
            {
                duMaxProperty.valFloat = Mathf.Clamp(duMaxProperty.valFloat, maxLeftLimit, maxRightLimit);

                if (duMinProperty.valFloat > duMaxProperty.valFloat)
                    duMinProperty.valFloat = duMaxProperty.valFloat;
            }
            
            return duProperty.isChanged;
        }

        public bool PropertyFieldRange(DuProperty duProperty,
            float leftValue, float rightValue, float stepValue, float leftLimit, float rightLimit)
            => PropertyFieldRange(duProperty,
                duProperty.title + " Min", leftValue, rightValue, stepValue, leftLimit, rightLimit,
                duProperty.title + " Max", leftValue, rightValue, stepValue, leftLimit, rightLimit);

        public bool PropertyFieldRange(DuProperty duProperty,
            string minTitle, string maxTitle,
            float leftValue, float rightValue, float stepValue, float leftLimit, float rightLimit)
            => PropertyFieldRange(duProperty,
                minTitle, leftValue, rightValue, stepValue, leftLimit, rightLimit,
                maxTitle, leftValue, rightValue, stepValue, leftLimit, rightLimit);

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        
        public bool PropertyFieldDurationRange(DuProperty duProperty)
            => PropertyFieldDurationRange(duProperty, duProperty.title + " Min", duProperty.title + " Max");

        public bool PropertyFieldDurationRange(DuProperty duProperty, 
            string minTitle, string maxTitle)
            => PropertyFieldDurationRange(duProperty,
                minTitle, maxTitle,
                0.0f, 10.0f, 0.01f, 0.0f, float.MaxValue);

        public bool PropertyFieldDurationRange(DuProperty duProperty, 
            string minTitle, string maxTitle,
            float leftValue, float rightValue, float stepValue, float leftLimit, float rightLimit)
            => PropertyFieldRange(duProperty,
                minTitle, leftValue, rightValue, stepValue, leftLimit, rightLimit,
                maxTitle, leftValue, rightValue, stepValue, leftLimit, rightLimit);

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public static bool PropertySeedRandomOrFixed(DuProperty duProperty)
            => PropertySeedRandomOrFixed(duProperty, Constants.RANDOM_SEED_DEFAULT);

        public static bool PropertySeedRandomOrFixed(DuProperty duProperty, int defValue)
        {
            bool isChanged = false;

            int seed = duProperty.valInt;

            bool curUseSeed = seed > 0;
            bool newUseSeed = DustGUI.Field("Use Fixed Seed", curUseSeed);

            if (curUseSeed != newUseSeed)
            {
                if (newUseSeed)
                    duProperty.valInt = duProperty.valInt == 0 ? defValue : -duProperty.valInt;
                else
                    duProperty.valInt = -duProperty.valInt;

                isChanged = true;
            }

            if (newUseSeed)
                isChanged = PropertySeedFixed(duProperty);

            return isChanged;
        }

        public static bool PropertySeedFixed(DuProperty duProperty)
        {
            int seedMin = Constants.RANDOM_SEED_MIN;
            int seedMax = Constants.RANDOM_SEED_MAX;
            int seedEditorMin = Constants.RANDOM_SEED_MIN_IN_EDITOR;
            int seedEditorMax = Constants.RANDOM_SEED_MAX_IN_EDITOR;

            EditorGUI.BeginChangeCheck();
            DustGUI.ExtraIntSlider.Create(seedEditorMin, seedEditorMax, 1, seedMin, seedMax).Draw("Seed", duProperty.property);
            duProperty.isChanged = EditorGUI.EndChangeCheck();
            return duProperty.isChanged;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public static bool PropertySlider(DuProperty duProperty, float leftValue, float rightValue)
        {
            if (Dust.IsNull(duProperty.property))
            {
                Dust.Debug.Warning("DuProperty[" + duProperty.propertyPath + "] is null");
                return false;
            }

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.Slider(duProperty.property, leftValue, rightValue, new GUIContent(duProperty.title, duProperty.tooltip));
            duProperty.isChanged = EditorGUI.EndChangeCheck();
            return duProperty.isChanged;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public static bool PropertyExtendedIntSlider(DuProperty duProperty, int leftValue, int rightValue, int stepValue)
            => PropertyExtendedIntSlider(duProperty, leftValue, rightValue, stepValue, int.MinValue, int.MaxValue);

        public static bool PropertyExtendedIntSlider(DuProperty duProperty, int leftValue, int rightValue, int stepValue, int leftLimit)
            => PropertyExtendedIntSlider(duProperty, leftValue, rightValue, stepValue, leftLimit, int.MaxValue);

        public static bool PropertyExtendedIntSlider(DuProperty duProperty, int leftValue, int rightValue, int stepValue, int leftLimit, int rightLimit)
        {
            var slider = new DustGUI.ExtraIntSlider(leftValue, rightValue, stepValue, leftLimit, rightLimit);
            slider.LinkEditor(duProperty.parentEditor);
            duProperty.isChanged = slider.Draw(new GUIContent(duProperty.title, duProperty.tooltip), duProperty.property);
            return duProperty.isChanged;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public static bool PropertyExtendedSlider(DuProperty duProperty, float leftValue, float rightValue, float stepValue)
            => PropertyExtendedSlider(duProperty, leftValue, rightValue, stepValue, float.MinValue, float.MaxValue);

        public static bool PropertyExtendedSlider(DuProperty duProperty, float leftValue, float rightValue, float stepValue, float leftLimit)
            => PropertyExtendedSlider(duProperty, leftValue, rightValue, stepValue, leftLimit, float.MaxValue);

        public static bool PropertyExtendedSlider(DuProperty duProperty, float leftValue, float rightValue, float stepValue, float leftLimit, float rightLimit)
        {
            var slider = new DustGUI.ExtraSlider(leftValue, rightValue, stepValue, leftLimit, rightLimit);
            slider.LinkEditor(duProperty.parentEditor);
            duProperty.isChanged = slider.Draw(new GUIContent(duProperty.title, duProperty.tooltip), duProperty.property);
            return duProperty.isChanged;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public static bool PropertyExtendedSlider01(DuProperty duProperty)
        {
            return PropertyExtendedSlider(duProperty, 0f, 1f, 0.01f, 0f, 1f);
        }

        //--------------------------------------------------------------------------------------------------------------

        public static bool PropertyFieldCurve(DuProperty duProperty)
            => PropertyFieldCurve(duProperty, 100);

        public static bool PropertyFieldCurve(DuProperty duProperty, int height)
        {
            return PropertyFieldCurve(duProperty, duProperty.title);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public static bool PropertyFieldCurve(DuProperty duProperty, string label)
            => PropertyFieldCurve(duProperty, label, 100);

        public static bool PropertyFieldCurve(DuProperty duProperty, string label, int height)
        {
            if (Dust.IsNull(duProperty.property))
            {
                Dust.Debug.Warning("DuProperty[" + duProperty.propertyPath + "] is null");
                return false;
            }

            EditorGUI.BeginChangeCheck();
            DustGUI.Field(label, duProperty.property, 0, 90);
            duProperty.isChanged = EditorGUI.EndChangeCheck();
            return duProperty.isChanged;
        }

        //--------------------------------------------------------------------------------------------------------------

        public static bool PropertyField(SerializedProperty property, string label)
            => PropertyField(property, label, "");

        public static bool PropertyField(SerializedProperty property, string label, string tooltip)
        {
            EditorGUI.BeginChangeCheck();
            DustGUI.Field(new GUIContent(label, tooltip), property);
            return EditorGUI.EndChangeCheck();
        }

        //--------------------------------------------------------------------------------------------------------------

        public static bool PropertyFieldOrLock(SerializedProperty property, bool isLocked, string label)
            => PropertyFieldOrLock(property, isLocked, label, "");

        public static bool PropertyFieldOrLock(SerializedProperty property, bool isLocked, string label, string tooltip)
        {
            if (isLocked) DustGUI.Lock();

            bool isChanged = PropertyField(property, label, tooltip);

            if (isLocked) DustGUI.Unlock();

            return isChanged;
        }

        //--------------------------------------------------------------------------------------------------------------

        public static int Popup(string label, int selectedIndex, string[] displayedOptions)
        {
            return EditorGUILayout.Popup(label, selectedIndex, displayedOptions);
        }

        //--------------------------------------------------------------------------------------------------------------

        public static void Space()
        {
            DustGUI.SpaceLine(0.5f);
        }
    }
}
