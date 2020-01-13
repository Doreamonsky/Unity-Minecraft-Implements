using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace OPS.AntiCheat.Editor
{
    [CustomPropertyDrawer(typeof(OPS.AntiCheat.Field.IProtected))]
    public class ProtectedPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // Calculate rects
            var amountRect = new Rect(position.x, position.y, position.width, position.height);

            // Draw fields
            this.OnGUIProperty(amountRect, property, label);

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }

        protected virtual void OnGUIProperty(Rect position, SerializedProperty property, GUIContent label)
        {
        }

        private static Type GetScriptTypeFromProperty(SerializedProperty property)
        {
            SerializedProperty serializedProperty = property.serializedObject.FindProperty("m_Script");
            if (serializedProperty != null)
            {
                MonoScript monoScript = serializedProperty.objectReferenceValue as MonoScript;
                if (!((UnityEngine.Object)monoScript == (UnityEngine.Object)null))
                {
                    return monoScript.GetClass();
                }
                return null;
            }
            return null;
        }

        internal static FieldInfo GetFieldInfoFromProperty(SerializedProperty property, out Type type)
        {
            Type scriptTypeFromProperty = GetScriptTypeFromProperty(property);
            if (scriptTypeFromProperty != null)
            {
                return GetFieldInfoFromPropertyPath(scriptTypeFromProperty, property.propertyPath, out type);
            }
            type = null;
            return null;
        }

        private static FieldInfo GetFieldInfoFromPropertyPath(Type host, string path, out Type type)
        {
            FieldInfo fieldInfo = null;
            type = host;
            string[] array = path.Split('.');
            for (int i = 0; i < array.Length; i++)
            {
                string text = array[i];
                if (i < array.Length - 1 && text == "Array" && array[i + 1].StartsWith("data["))
                {
                    if (type.IsArrayOrList())
                    {
                        type = type.GetArrayOrListElementType();
                    }
                    i++;
                }
                else
                {
                    FieldInfo fieldInfo2 = null;
                    Type type2 = type;
                    while (fieldInfo2 == null && type2 != null)
                    {
                        fieldInfo2 = type2.GetField(text, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                        type2 = type2.BaseType;
                    }
                    if (fieldInfo2 == null)
                    {
                        type = null;
                        return null;
                    }
                    fieldInfo = fieldInfo2;
                    type = fieldInfo.FieldType;
                }
            }
            return fieldInfo;
        }
    }

    //Int
    [CustomPropertyDrawer(typeof(OPS.AntiCheat.Field.ProtectedInt32), true)]
    public class ProtectedInt32Drawer : ProtectedPropertyDrawer
    {
        protected override void OnGUIProperty(Rect position, SerializedProperty property, GUIContent label)
        {
            UnityEditor.EditorGUI.BeginChangeCheck();
            Type var_Type;
            FieldInfo var_FieldInfo = ProtectedPropertyDrawer.GetFieldInfoFromProperty(property, out var_Type);
            Int32 var_Value = UnityEditor.EditorGUI.IntField(position, label, (OPS.AntiCheat.Field.ProtectedInt32)var_FieldInfo.GetValue(property.serializedObject.targetObject));
            if (UnityEditor.EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(property.serializedObject.targetObject, label.text);
                var_FieldInfo.SetValue(property.serializedObject.targetObject, (OPS.AntiCheat.Field.ProtectedInt32)var_Value);
            }
        }
    }

    //Float
    [CustomPropertyDrawer(typeof(OPS.AntiCheat.Field.ProtectedFloat), true)]
    public class ProtectedFloatDrawer : ProtectedPropertyDrawer
    {
        protected override void OnGUIProperty(Rect position, SerializedProperty property, GUIContent label)
        {
            UnityEditor.EditorGUI.BeginChangeCheck();
            Type var_Type;
            FieldInfo var_FieldInfo = ProtectedPropertyDrawer.GetFieldInfoFromProperty(property, out var_Type);
            float var_Value = UnityEditor.EditorGUI.FloatField(position, label, (OPS.AntiCheat.Field.ProtectedFloat)var_FieldInfo.GetValue(property.serializedObject.targetObject));
            if (UnityEditor.EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(property.serializedObject.targetObject, label.text);
                var_FieldInfo.SetValue(property.serializedObject.targetObject, (OPS.AntiCheat.Field.ProtectedFloat)var_Value);
            }
        }
    }

    //String
    [CustomPropertyDrawer(typeof(OPS.AntiCheat.Field.ProtectedString), true)]
    public class ProtectedStringDrawer : ProtectedPropertyDrawer
    {
        protected override void OnGUIProperty(Rect position, SerializedProperty property, GUIContent label)
        {
            UnityEditor.EditorGUI.BeginChangeCheck();
            Type var_Type;
            FieldInfo var_FieldInfo = ProtectedPropertyDrawer.GetFieldInfoFromProperty(property, out var_Type);
            string var_Value = UnityEditor.EditorGUI.TextField(position, label, (OPS.AntiCheat.Field.ProtectedString)var_FieldInfo.GetValue(property.serializedObject.targetObject));
            if (UnityEditor.EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(property.serializedObject.targetObject, label.text);
                var_FieldInfo.SetValue(property.serializedObject.targetObject, (OPS.AntiCheat.Field.ProtectedString)var_Value);
            }
        }
    }
}