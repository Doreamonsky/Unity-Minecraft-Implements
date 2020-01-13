using UnityEngine;
using System;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;

namespace OPS.Obfuscator.Gui
{
    public class StackTraceWindow : EditorWindow
    {        
        //Scroll
        private Vector2 scrollPosition = Vector2.zero;

        //
        private String stackTraceText;
        private String generatedStackTraceText;

        //
        private OPS.Obfuscator.StackTrace.StackTraceManager stackTraceManager;

        // Add menu item named "My Window" to the Window menu
        [MenuItem("OPS/Obfuscator/Obfuscator StackTrace")]
        public static void ShowWindow()
        {
            //Show existing window instance. If one doesn't exist, make one.
            EditorWindow.GetWindow<StackTraceWindow>("StackTrace");
        }

        private void OnEnable()
        {
            this.stackTraceManager = new StackTrace.StackTraceManager();
            this.stackTraceManager.LoadSettings();
            this.stackTraceManager.LoadMapping();
        }

        void OnGUI()
        {
            try
            {
                this.scrollPosition = GUILayout.BeginScrollView(this.scrollPosition, false, false, GUILayout.Width(position.width), GUILayout.Height(position.height));

                EditorGUILayout.HelpBox("Use this to create from an obfuscated stacktrace, an unobfuscated stacktrace.", MessageType.Info);
                EditorGUILayout.HelpBox("To use this function, you have to store the renaming mapping. To do this, go to Obfuscator Settings->Advanced->Mapping enable the 'Save Renaming Mapping' and enter a valid FilePath.", MessageType.Warning);
                EditorGUILayout.HelpBox("The entered stacktrace has to be in the form: \n at abc.a.b () ... \n at xyz.a.c () ... \n at b.d () ...", MessageType.Info);

                GUILayout.BeginHorizontal();
                GUILayout.Label("Obfuscated StackTrace: ");
                this.stackTraceText = EditorGUILayout.TextArea(this.stackTraceText);
                GUILayout.EndHorizontal();

                if (GUILayout.Button("Try to unobfuscate"))
                {
                    this.generatedStackTraceText = this.stackTraceManager.Process(this.stackTraceText);
                }

                GUILayout.BeginHorizontal();
                GUILayout.Label("Unobfuscated StackTrace: ");
                this.generatedStackTraceText = EditorGUILayout.TextArea(this.generatedStackTraceText);
                GUILayout.EndHorizontal();

                GUILayout.EndScrollView();
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
                this.Close();
            }
        }        
    }
}
#endif