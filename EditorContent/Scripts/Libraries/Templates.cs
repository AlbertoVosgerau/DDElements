using System;
using UnityEditor;
using UnityEngine;

namespace DandyDino.Elements
{
    public partial class Templates
    {
        public void LeadingIconAndButton(GUIContent icon, string title, Color hoverColor, Action onClick,params GUILayoutOption[] options)
        {
            DDElements.Layout.Column(() =>
            {
                DDElements.Layout.Row(() =>
                {
                    DDElements.Layout.Space(10);
                    DDElements.Rendering.Icon(icon, 26);
                    DDElements.Rendering.FlatColorButton(title.ToGUIContent(), DDElements.Colors.Clear, onClick, options);
                    DDElements.Layout.Space(4);
                });
                
                Rect lastRect = DDElements.Helpers.GetLastRect();
                if (lastRect.Contains(Event.current.mousePosition))
                {
                    Handles.DrawSolidRectangleWithOutline(lastRect, hoverColor, Color.clear);
                }
                
                DDElements.Rendering.Line();
            }, options: options);
        }
    }
}