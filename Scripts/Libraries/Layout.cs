using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DandyDino.Elements
{
    public class Layout
    {
        public enum Alignment
        {
            START,
            END,
            MIDDLE,
            NONE
        }
        
        public void DrawBackground(EditorWindow window, Color color)
        {
            GUI.Box(new Rect(0,0, window.position.width, window.position.height), "", DDElements.Styles.FlatColor(color));
        }
        
        public void DrawBackground(Rect rect, Color color)
        {
            GUI.Box(rect, "", DDElements.Styles.FlatColor(color));
        }
        
        public bool RectIsHovered(Rect rect)
        {
            bool isHovered = rect.Contains(Event.current.mousePosition);
            return isHovered;
        }
        
        public void Row(Action content, Alignment alignment = Alignment.NONE, GUIStyle style = null, params GUILayoutOption[] options)
        {
            style = style == null ? GUIStyle.none : style;
            GUILayout.BeginHorizontal(style, options);
            ApplyAlignment(content, alignment);
            GUILayout.EndHorizontal();
        }
        
        public void Column(Action content, Alignment alignment = Alignment.NONE, GUIStyle style = null, params GUILayoutOption[] options)
        {
            style = style == null ? GUIStyle.none : style;
            GUILayout.BeginVertical(style, options);
            ApplyAlignment(content, alignment);
            GUILayout.EndVertical();
        }
        
        public void ScrollView(ref Vector2 scrollPos, Action content, params GUILayoutOption[] options)
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos, options);
            content();
            GUILayout.EndScrollView();
        }

        public void FlexibleSpace()
        {
            GUILayout.FlexibleSpace();
        }

        public void Space(int space = 5)
        {
            GUILayout.Space(space);
        }

        private void ApplyAlignment(Action content, Alignment alignment)
        {
            if (alignment == Alignment.MIDDLE || alignment == Alignment.END)
            {
                GUILayout.FlexibleSpace();
            }
            content();
            if (alignment == Alignment.MIDDLE || alignment == Alignment.START)
            {
                GUILayout.FlexibleSpace();
            }
        }
    }
}