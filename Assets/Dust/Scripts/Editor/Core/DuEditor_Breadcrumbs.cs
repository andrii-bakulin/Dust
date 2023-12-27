using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace DustEngine.DustEditor
{
    public partial class DuEditor
    {
        protected class BreadcrumbItem
        {
            public Component component;
        }
        
        //--------------------------------------------------------------------------------------------------------------

        protected static void InspectorBreadcrumbsForFactoryMachine(DuEditor editor)
        {
            var queue = new Queue<Type>(); 
            {
                queue.Enqueue(typeof(FactoryMachine));
                queue.Enqueue(typeof(Factory));
            }

            InspectorBreadcrumbs(editor, queue);
        }
        
        protected static void InspectorBreadcrumbsForField(DuEditor editor)
        {
            var queue = new Queue<Type>(); 
            {
                queue.Enqueue(typeof(Field));
                queue.Enqueue(typeof(FactoryMachine));
                queue.Enqueue(typeof(Factory));
            }

            InspectorBreadcrumbs(editor, queue);
        }
        
        private static void InspectorBreadcrumbs(DuEditor editor, Queue<Type> componentsQueue)
        {
            if (editor.targets.Length > 1)
                return;

            var breadcrumbs = new List<BreadcrumbItem>();

            //----------------------------------------------------------------------------------------------------------
            // Build breadcrumbs items

            var transform = (editor.target as Component).transform;
            Type lookupType = null; 

            while (Dust.IsNotNull(transform))
            {
                if (Dust.IsNull(lookupType))
                {
                    if (componentsQueue.Count == 0)
                        break;

                    lookupType = componentsQueue.Dequeue();
                }

                var component = transform.GetComponent(lookupType);

                if (Dust.IsNotNull(component))
                {
                    bool allowToAdd = true;

                    if (component is BasicFactoryMachine compFactoryMachine)
                    {
                        if (breadcrumbs.Count > 0 && breadcrumbs[0].component is Field field)
                            allowToAdd = compFactoryMachine.fieldsMap.HasField(field);
                    }
                    else if (component is Factory compFactory)
                    {
                        if (breadcrumbs.Count > 0 && breadcrumbs[0].component is FactoryMachine factoryMachine)
                            allowToAdd = compFactory.HasFactoryMachine(factoryMachine);
                    }

                    if (allowToAdd)
                    {
                        breadcrumbs.Insert(0, new BreadcrumbItem
                        {
                            component = component,
                        });
                        lookupType = null;
                    }
                }
                    
                transform = transform.parent;
            }
            
            //----------------------------------------------------------------------------------------------------------
            // Draw UI

            if (breadcrumbs.Count <= 1)
                return;

            DustGUI.BeginVerticalBox(0, DustGUI.Config.ICON_BUTTON_HEIGHT);
            DustGUI.BeginHorizontal();
            {
                for (int i = 0; i < breadcrumbs.Count; i++)
                {
                    var breadcrumb = breadcrumbs[i];
                    var icon = UI.Icons.GetTextureByComponent(breadcrumb.component);

                    if (DustGUI.IconButton(icon, UI.CELL_WIDTH_ICON, UI.CELL_WIDTH_ICON, ExtraList.styleMiniButton))
                        Selection.activeGameObject = breadcrumb.component.gameObject;

                    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                    if (i == breadcrumbs.Count - 1)
                        break;
                    
                    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                    DustGUI.BeginVertical(90f);
                    {
                        string componentName = breadcrumb.component.ToString();
                        
                        Match m = Regex.Match(componentName, @"\(DustEngine\.(.+)\)", RegexOptions.IgnoreCase);
                        if (m.Success && m.Groups.Count >= 2)
                            componentName = m.Groups[1].Value;
                            
                        DustGUI.SimpleLabel(breadcrumb.component.gameObject.name, 0, 14);
                        DustGUI.SimpleLabel(componentName, 0, 10, ExtraList.styleHintLabel);
                    }
                    DustGUI.EndVertical();

                    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                    DustGUI.BeginVertical(15f);
                    DustGUI.SimpleLabel("»", 0, DustGUI.Config.ICON_BUTTON_HEIGHT);
                    DustGUI.EndVertical();
                }
            }
            DustGUI.EndHorizontal();
            DustGUI.EndVertical();

            Space();
        }
    }
}
