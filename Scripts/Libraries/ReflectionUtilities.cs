using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DandyDino.Elements
{
    public class ReflectionUtilities
    {
        public void AddClassInstanceBar<T>(
            SerializedObject so,
            Func<Type, T> instantiator,
            Action<T> onAdd,
            string groupLabel,
            GUIContent itemLabel,
            List<Type> types,
            List<T> target,
            Color buttonColor,
            Type[] excludeTypes = null) where T : class
        {
            GenericMenu menuHeader = new GenericMenu();
            foreach (Type item in types)
            {
                if (excludeTypes != null && excludeTypes.Contains(item))
                {
                    continue;
                }
                
                string label = item.ToString().Split('.').Last();

                if (target.Any(x => x != null && x.GetType() == item))
                {
                    menuHeader.AddDisabledItem(new GUIContent(label), false);
                }
                else
                {
                    Type capturedType = item;
                    menuHeader.AddItem(new GUIContent(label), false, () => AddItemToClassList(so, instantiator, onAdd, target, capturedType));
                }
            }

            DDElements.Layout.Row(() =>
            {
                DDElements.Layout.Column(() =>
                {
                    DDElements.Layout.Space(5);
                    DDElements.Layout.Row(() =>
                    {
                        DDElements.Rendering.LabelField(groupLabel.ToGUIContent(), DDElements.Styles.Label(16, FontStyle.Bold, TextAnchor.MiddleCenter));
                        DDElements.Rendering.FlatColorButton(itemLabel, buttonColor, () =>
                        {
                            menuHeader.ShowAsContext();
                        }, new []{GUILayout.Width(22),  GUILayout.Height(18)});
                    });
                    DDElements.Layout.Space(5);
                });
            }, style: DDElements.Styles.FlatColor(DDElements.Colors.MidDarkGray));
        }

        private void AddItemToClassList<T>(
            SerializedObject so,
            Func<Type, T> instantiator,
            Action<T> onAdd,
            List<T> target,
            Type type) where T : class
        {
            if (target.Any(x => x != null && x.GetType() == type)) return;

            Undo.RegisterCompleteObjectUndo(so.targetObject, $"Add {type.Name}");

            T newItem = instantiator(type);
            if (newItem == null) return;

            if (!target.Contains(newItem))
            {
                target.Add(newItem);
                onAdd?.Invoke(newItem);
            }

            EditorUtility.SetDirty(so.targetObject);
        }

        public T InstantiateClass<T>(Type type) where T : class
        {
            object newObj = Activator.CreateInstance(type);
            return newObj as T;
        }
        
        public List<Type> GetAllConcreteImplementations<T>()
        {
            Type baseType = typeof(T);
            List<Type> implementations = new List<Type>();
            
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    implementations.AddRange(
                        assembly.GetTypes()
                            .Where(type => type.IsClass && !type.IsAbstract && baseType.IsAssignableFrom(type))
                    );
                }
                catch (ReflectionTypeLoadException ex)
                {
                    foreach (Type type in ex.Types)
                    {
                        if (type != null && type.IsClass && !type.IsAbstract && baseType.IsAssignableFrom(type))
                        {
                            implementations.Add(type);
                        }
                    }
                }
            }

            return implementations;
        }
    }
}