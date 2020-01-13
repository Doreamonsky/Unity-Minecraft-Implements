using UnityEngine;
using System;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;

namespace Obfuscator.Gui
{
    public class SettingsWindow : EditorWindow
    {
        private OPS.Obfuscator.Setting.Settings settings;

        private void ReloadSettings()
        {
            this.settings = new OPS.Obfuscator.Setting.Settings();
            this.settings.LoadSettings();
        }

        private void SaveSettings()
        {
            if (this.settings != null)
            {
                this.settings.SaveSettings();
            }
        }        

        private class Text
        {
            private static GUIStyle styleMiddleLeft;
            private static GUIStyle styleMiddleLeftBold;

            private static GUIStyle styleMiddleCenter;
            private static GUIStyle styleMiddleCenterBold;

            public static void Gui(String _Text, int _Width = 220, int _Height = 24)
            {
                if (styleMiddleLeft == null)
                {
                    styleMiddleLeft = new GUIStyle("label");
                    styleMiddleLeft.alignment = TextAnchor.MiddleLeft;
                    styleMiddleLeft.fontSize = 12;
                }
                GUILayout.Label(_Text, styleMiddleLeft, GUILayout.Width(_Width), GUILayout.Height(_Height));
            }
            public static void GuiBold(String _Text, int _Width = 220, int _Height = 24)
            {
                if (styleMiddleLeftBold == null)
                {
                    styleMiddleLeftBold = new GUIStyle("label");
                    styleMiddleLeftBold.alignment = TextAnchor.MiddleLeft;
                    styleMiddleLeftBold.fontStyle = FontStyle.Bold;
                    styleMiddleLeftBold.fontSize = 13;
                }
                GUILayout.Label(_Text, styleMiddleLeftBold, GUILayout.Width(_Width), GUILayout.Height(_Height));
            }

            public static void GuiCenter(String _Text, int _Width = 220, int _Height = 24)
            {
                if (styleMiddleCenter == null)
                {
                    styleMiddleCenter = new GUIStyle("label");
                    styleMiddleCenter.alignment = TextAnchor.MiddleCenter;
                    styleMiddleCenter.fontSize = 12;
                }

                GUILayout.BeginHorizontal();

                GUILayout.FlexibleSpace();
                GUILayout.Label(_Text, styleMiddleCenter, GUILayout.Width(_Width), GUILayout.Height(_Height));
                GUILayout.FlexibleSpace();

                GUILayout.EndHorizontal();

            }
            public static void GuiCenterBold(String _Text, int _Width = 220, int _Height = 24)
            {
                if (styleMiddleCenterBold == null)
                {
                    styleMiddleCenterBold = new GUIStyle("label");
                    styleMiddleCenterBold.alignment = TextAnchor.MiddleCenter;
                    styleMiddleCenterBold.fontStyle = FontStyle.Bold;
                    styleMiddleCenterBold.fontSize = 13;
                }

                GUILayout.BeginHorizontal();

                GUILayout.FlexibleSpace();
                GUILayout.Label(_Text, styleMiddleLeftBold, GUILayout.Width(_Width), GUILayout.Height(_Height));
                GUILayout.FlexibleSpace();

                GUILayout.EndHorizontal();
            }
        }

        private class Switch
        {
            private static Texture onTexture;
            private static Texture offTexture;
            private static Texture onGoldTexture;
            private static Texture offGoldTexture;
            private static GUIStyle style;

            public static void Gui(ref bool _Value)
            {
                try
                {
                    if (onTexture == null)
                    {
                        onTexture = (Texture)EditorGUIUtility.Load("Assets/OPS/Obfuscator.Free/Editor/Gui/Button_On_24x.png");
                    }
                    if (offTexture == null)
                    {
                        offTexture = (Texture)EditorGUIUtility.Load("Assets/OPS/Obfuscator.Free/Editor/Gui/Button_Off_24x.png");
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());
                }
                if (style == null)
                {
                    style = new GUIStyle("button");
                    style.normal.background = null;
                    style.active.background = null;
                }
                if (_Value)
                {
                    if (GUILayout.Button(onTexture, style, GUILayout.Width(64), GUILayout.Height(24)))
                    {
                        _Value = false;
                    }
                }
                else
                {
                    if (GUILayout.Button(offTexture, style, GUILayout.Width(64), GUILayout.Height(24)))
                    {
                        _Value = true;
                    }
                }
            }

            public static void GuiGold(ref bool _Value)
            {
                try
                {
                    if (onGoldTexture == null)
                    {
                        onGoldTexture = (Texture)EditorGUIUtility.Load("Assets/OPS/Obfuscator.Free/Editor/Gui/Button_On_Gold_24x.png");
                    }
                    if (offGoldTexture == null)
                    {
                        offGoldTexture = (Texture)EditorGUIUtility.Load("Assets/OPS/Obfuscator.Free/Editor/Gui/Button_Off_Gold_24x.png");
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());
                }
                if (style == null)
                {
                    style = new GUIStyle("button");
                    style.normal.background = null;
                    style.active.background = null;
                }
                if (_Value)
                {
                    if (GUILayout.Button(onGoldTexture, style, GUILayout.Width(64), GUILayout.Height(24)))
                    {
                        _Value = false;
                    }
                }
                else
                {
                    if (GUILayout.Button(offGoldTexture, style, GUILayout.Width(64), GUILayout.Height(24)))
                    {
                        _Value = true;
                    }
                }
            }

            public static void GuiCenter(ref bool _Value)
            {
                try
                {
                    if (onTexture == null)
                    {
                        onTexture = (Texture)EditorGUIUtility.Load("Assets/OPS/Obfuscator.Free/Editor/Gui/Button_On_24x.png");
                    }
                    if (offTexture == null)
                    {
                        offTexture = (Texture)EditorGUIUtility.Load("Assets/OPS/Obfuscator.Free/Editor/Gui/Button_Off_24x.png");
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());
                }
                if (style == null)
                {
                    style = new GUIStyle("button");
                    style.normal.background = null;
                    style.active.background = null;
                }
                if (_Value)
                {
                    GUILayout.BeginHorizontal();

                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button(onTexture, style, GUILayout.Width(64), GUILayout.Height(24)))
                    {
                        _Value = false;
                    }
                    GUILayout.FlexibleSpace();

                    GUILayout.EndHorizontal();
                }
                else
                {
                    GUILayout.BeginHorizontal();

                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button(offTexture, style, GUILayout.Width(64), GUILayout.Height(24)))
                    {
                        _Value = true;
                    }
                    GUILayout.FlexibleSpace();

                    GUILayout.EndHorizontal();
                }
            }

            public static void GuiCenterGold(ref bool _Value)
            {
                bool var_Enabled = GUI.enabled;
                if (OPS.Obfuscator.Setting.Settings.ObfuscatorType != OPS.Obfuscator.Setting.EObfuscatorType.Pro)
                {
                    GUI.enabled = false;
                }

                try
                {
                    if (onGoldTexture == null)
                    {
                        onGoldTexture = (Texture)EditorGUIUtility.Load("Assets/OPS/Obfuscator.Free/Editor/Gui/Button_On_Gold_24x.png");
                    }
                    if (offGoldTexture == null)
                    {
                        offGoldTexture = (Texture)EditorGUIUtility.Load("Assets/OPS/Obfuscator.Free/Editor/Gui/Button_Off_Gold_24x.png");
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());
                }
                if (style == null)
                {
                    style = new GUIStyle("button");
                    style.normal.background = null;
                    style.active.background = null;
                }
                if (_Value)
                {
                    GUILayout.BeginHorizontal();

                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button(onGoldTexture, style, GUILayout.Width(64), GUILayout.Height(24)))
                    {
                        _Value = false;
                    }
                    GUILayout.FlexibleSpace();

                    GUILayout.EndHorizontal();
                }
                else
                {
                    GUILayout.BeginHorizontal();

                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button(offGoldTexture, style, GUILayout.Width(64), GUILayout.Height(24)))
                    {
                        _Value = true;
                    }
                    GUILayout.FlexibleSpace();

                    GUILayout.EndHorizontal();
                }
                GUI.enabled = var_Enabled;
            }

            public static void Gui(ref bool _Value, String _TextureOn, String _TextureOff, int _Width = 128, int _Height = 32)
            {
                Texture var_OnTexture = null;
                Texture var_OffTexture = null;
                try
                {
                    var_OnTexture = (Texture)EditorGUIUtility.Load(_TextureOn);
                    var_OffTexture = (Texture)EditorGUIUtility.Load(_TextureOff);
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());
                }
                if (style == null)
                {
                    style = new GUIStyle("button");
                    style.normal.background = null;
                    style.active.background = null;
                }
                if (_Value)
                {
                    if (GUILayout.Button(var_OnTexture, style, GUILayout.Width(_Width), GUILayout.Height(_Height)))
                    {
                        _Value = false;
                    }
                }
                else
                {
                    if (GUILayout.Button(var_OffTexture, style, GUILayout.Width(_Width), GUILayout.Height(_Height)))
                    {
                        _Value = true;
                    }
                }
            }

            public static bool GuiNoRef(bool _Value, String _TextureOn, String _TextureOff, int _Width = 128, int _Height = 32)
            {
                Texture var_OnTexture = null;
                Texture var_OffTexture = null;
                try
                {
                    var_OnTexture = (Texture)EditorGUIUtility.Load(_TextureOn);
                    var_OffTexture = (Texture)EditorGUIUtility.Load(_TextureOff);
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());
                }
                if (style == null)
                {
                    style = new GUIStyle("button");
                    style.normal.background = null;
                    style.active.background = null;
                }
                if (_Value)
                {
                    if (GUILayout.Button(var_OnTexture, style, GUILayout.Width(_Width), GUILayout.Height(_Height)))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (GUILayout.Button(var_OffTexture, style, GUILayout.Width(_Width), GUILayout.Height(_Height)))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        private class Row
        {
            public static void Gui(String _Text, ref bool _Value)
            {
                GUILayout.BeginHorizontal();
                Text.Gui(_Text);
                Switch.Gui(ref _Value);
                GUILayout.EndHorizontal();
            }
            public static void GuiGold(String _Text, ref bool _Value)
            {
                GUILayout.BeginHorizontal();
                Text.Gui(_Text);
                Switch.GuiGold(ref _Value);
                GUILayout.EndHorizontal();
            }
            public static void GuiBold(String _Text, ref bool _Value)
            {
                GUILayout.BeginHorizontal();
                Text.GuiBold(_Text);
                Switch.Gui(ref _Value);
                GUILayout.EndHorizontal();
            }
        }

        private class Table
        {
            public static void Gui(String _TH, String _T1, String _T2, String _T3, ref bool _B1, ref bool _B2, ref bool _B3)
            {
                int var_Width = 72;
                GUILayout.BeginHorizontal();

                GUILayout.BeginVertical();
                Text.Gui(" ", var_Width);
                Text.Gui(_TH, var_Width);
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                Text.GuiCenter(_T1, var_Width);
                Switch.GuiCenter(ref _B1);
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                Text.GuiCenter(_T2, var_Width);
                Switch.GuiCenter(ref _B2);
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                Text.GuiCenter(_T3, var_Width);
                Switch.GuiCenter(ref _B3);
                GUILayout.EndVertical();

                GUILayout.EndHorizontal();
            }

            public static void Gui(String _TH, String _T1, String _T2, String _T3, String _T4, ref bool _B1, ref bool _B2, ref bool _B3, ref bool _B4)
            {
                int var_Width = 72;
                GUILayout.BeginHorizontal();

                GUILayout.BeginVertical();
                Text.Gui(" ", var_Width);
                Text.Gui(_TH, var_Width);
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                Text.GuiCenter(_T1, var_Width);
                Switch.GuiCenter(ref _B1);
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                Text.GuiCenter(_T2, var_Width);
                Switch.GuiCenter(ref _B2);
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                Text.GuiCenter(_T3, var_Width);
                Switch.GuiCenter(ref _B3);
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                Text.GuiCenter(_T4, var_Width);
                Switch.GuiCenter(ref _B4);
                GUILayout.EndVertical();

                GUILayout.EndHorizontal();
            }

            public static void GuiLastGold(String _TH, String _T1, String _T2, String _T3, ref bool _B1, ref bool _B2, ref bool _B3)
            {
                int var_Width = 72;
                GUILayout.BeginHorizontal();

                GUILayout.BeginVertical();
                Text.Gui(" ", var_Width);
                Text.Gui(_TH, var_Width);
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                Text.GuiCenter(_T1, var_Width);
                Switch.GuiCenter(ref _B1);
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                Text.GuiCenter(_T2, var_Width);
                Switch.GuiCenter(ref _B2);
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                Text.GuiCenter(_T3, var_Width);
                Switch.GuiCenterGold(ref _B3);
                GUILayout.EndVertical();

                GUILayout.EndHorizontal();
            }

            public static void GuiLastGold(String _TH, String _T1, String _T2, String _T3, String _T4, ref bool _B1, ref bool _B2, ref bool _B3, ref bool _B4)
            {
                int var_Width = 72;
                GUILayout.BeginHorizontal();

                GUILayout.BeginVertical();
                Text.Gui(" ", var_Width);
                Text.Gui(_TH, var_Width);
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                Text.GuiCenter(_T1, var_Width);
                Switch.GuiCenter(ref _B1);
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                Text.GuiCenter(_T2, var_Width);
                Switch.GuiCenter(ref _B2);
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                Text.GuiCenter(_T3, var_Width);
                Switch.GuiCenter(ref _B3);
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                Text.GuiCenter(_T4, var_Width);
                Switch.GuiCenterGold(ref _B4);
                GUILayout.EndVertical();

                GUILayout.EndHorizontal();
            }

            public static void Gui(String _TH, String _T1, String _T2, String _T3, String _T4, String _T5, ref bool _B1, ref bool _B2, ref bool _B3, ref bool _B4, ref bool _B5)
            {
                int var_Width = 72;
                GUILayout.BeginHorizontal();

                GUILayout.BeginVertical();
                Text.Gui(" ", var_Width);
                Text.Gui(_TH, var_Width);
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                Text.GuiCenter(_T1, var_Width);
                Switch.GuiCenter(ref _B1);
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                Text.GuiCenter(_T2, var_Width);
                Switch.GuiCenter(ref _B2);
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                Text.GuiCenter(_T3, var_Width);
                Switch.GuiCenter(ref _B3);
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                Text.GuiCenter(_T4, var_Width);
                Switch.GuiCenter(ref _B4);
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                Text.GuiCenter(_T5, var_Width);
                Switch.GuiCenter(ref _B5);
                GUILayout.EndVertical();

                GUILayout.EndHorizontal();
            }
        }

        //
        private int activeOptions
        {
            get
            {
                return (this.settings.AddRandomCode ? 1 : 0)
                    + (this.settings.ObfuscateClass ? 1 : 0)
                    + (this.settings.ObfuscateClassAbstract ? 1 : 0)
                    + (this.settings.ObfuscateClassGeneric ? 1 : 0)
                    + (this.settings.ObfuscateClassInternal ? 1 : 0)
                    + (this.settings.ObfuscateClassPrivate ? 1 : 0)
                    + (this.settings.ObfuscateClassProtected ? 1 : 0)
                    + (this.settings.ObfuscateClassPublic ? 1 : 0)
                    + (this.settings.ObfuscateEnumValues ? 1 : 0)
                    + (this.settings.ObfuscateEvent ? 1 : 0)
                    + (this.settings.ObfuscateField ? 1 : 0)
                    + (this.settings.ObfuscateFieldInternal ? 1 : 0)
                    + (this.settings.ObfuscateFieldPrivate ? 1 : 0)
                    + (this.settings.ObfuscateFieldProtected ? 1 : 0)
                    + (this.settings.ObfuscateFieldPublic ? 1 : 0)
                    + (this.settings.ObfuscateMethod ? 1 : 0)
                    + (this.settings.ObfuscateMethodInternal ? 1 : 0)
                    + (this.settings.ObfuscateMethodPrivate ? 1 : 0)
                    + (this.settings.ObfuscateMethodProtected ? 1 : 0)
                    + (this.settings.ObfuscateMethodPublic ? 1 : 0)
                    + (this.settings.ObfuscateNamespace ? 1 : 0)
                    + (this.settings.ObfuscateProperty ? 1 : 0)
                    + (this.settings.ObfuscateString ? 1 : 0)
                    + (this.settings.ObfuscateUnityClasses ? 1 : 0)
                    + (this.settings.ObfuscateUnityPublicFields ? 1 : 0);
            }
        }

        private int securityLevel
        {
            get
            {
                if (activeOptions >= 18)
                {
                    return 3;
                }
                if (activeOptions >= 12)
                {
                    return 2;
                }
                return 1;
            }
            set
            {
                switch (value)
                {
                    case 1:
                        {
                            this.settings.ObfuscateNamespace = false;

                            this.settings.ObfuscateClass = true;
                            this.settings.ObfuscateClassPublic = false;
                            this.settings.ObfuscateClassProtected = true;
                            this.settings.ObfuscateClassPrivate = true;
                            this.settings.ObfuscateClassInternal = true;

                            this.settings.ObfuscateField = true;
                            this.settings.ObfuscateFieldPublic = false;
                            this.settings.ObfuscateFieldProtected = true;
                            this.settings.ObfuscateFieldPrivate = true;
                            this.settings.ObfuscateFieldInternal = true;

                            this.settings.ObfuscateFieldSerializeAble = false;

                            this.settings.ObfuscateProperty = false;

                            this.settings.ObfuscateEvent = false;

                            this.settings.ObfuscateMethod = true;
                            this.settings.ObfuscateMethodPublic = false;
                            this.settings.ObfuscateMethodProtected = true;
                            this.settings.ObfuscateMethodPrivate = true;
                            this.settings.ObfuscateMethodInternal = true;
                            this.settings.ObfuscateUnityMethod = false;

                            //

                            this.settings.ObfuscateClassGeneric = false;
                            this.settings.ObfuscateClassAbstract = false;
                            this.settings.ObfuscateClassSerializeAble = false;
                            this.settings.ObfuscateUnityClasses = false;

                            this.settings.ObfuscateUnityPublicFields = false;
                            this.settings.ObfuscateEnumValues = false;

                            this.settings.TryFindGuiMethods = true;
                            this.settings.TryFindAnimationMethods = true;
                            
                            this.settings.CheckForReflectionAndCoroutine = true;

                            //

                            this.settings.AddRandomCode = false;
                            this.settings.ObfuscateString = false;
                            this.settings.StoreObfuscatedStrings = false;

                            break;
                        }
                    case 2:
                        {
                            if (OPS.Obfuscator.Setting.Settings.ObfuscatorType == OPS.Obfuscator.Setting.EObfuscatorType.Pro)
                            {
                                this.settings.ObfuscateNamespace = true;
                            }
                            else
                            {
                                this.settings.ObfuscateNamespace = false;
                            }

                            this.settings.ObfuscateClass = true;
                            this.settings.ObfuscateClassPublic = true;
                            this.settings.ObfuscateClassProtected = true;
                            this.settings.ObfuscateClassPrivate = true;
                            this.settings.ObfuscateClassInternal = true;

                            this.settings.ObfuscateField = true;
                            this.settings.ObfuscateFieldPublic = true;
                            this.settings.ObfuscateFieldProtected = true;
                            this.settings.ObfuscateFieldPrivate = true;
                            this.settings.ObfuscateFieldInternal = true;

                            this.settings.ObfuscateFieldSerializeAble = false;

                            this.settings.ObfuscateProperty = true;

                            this.settings.ObfuscateEvent = true;

                            this.settings.ObfuscateMethod = true;
                            this.settings.ObfuscateMethodPublic = true;
                            this.settings.ObfuscateMethodProtected = true;
                            this.settings.ObfuscateMethodPrivate = true;
                            this.settings.ObfuscateMethodInternal = true;
                            this.settings.ObfuscateUnityMethod = false;

                            //

                            this.settings.ObfuscateClassGeneric = false;
                            this.settings.ObfuscateClassAbstract = false;
                            this.settings.ObfuscateClassSerializeAble = false;
                            this.settings.ObfuscateUnityClasses = false;

                            this.settings.ObfuscateUnityPublicFields = false;
                            this.settings.ObfuscateEnumValues = false;

                            this.settings.TryFindGuiMethods = true;
                            this.settings.TryFindAnimationMethods = true;

                            this.settings.CheckForReflectionAndCoroutine = true;

                            //

                            this.settings.AddRandomCode = false;
                            this.settings.ObfuscateString = false;
                            this.settings.StoreObfuscatedStrings = false;

                            break;
                        }
                    case 3:
                        {
                            if (OPS.Obfuscator.Setting.Settings.ObfuscatorType == OPS.Obfuscator.Setting.EObfuscatorType.Pro)
                            {
                                this.settings.ObfuscateNamespace = true;
                            }
                            else
                            {
                                this.settings.ObfuscateNamespace = false;
                            }

                            this.settings.ObfuscateClass = true;
                            this.settings.ObfuscateClassPublic = true;
                            this.settings.ObfuscateClassProtected = true;
                            this.settings.ObfuscateClassPrivate = true;
                            this.settings.ObfuscateClassInternal = true;

                            this.settings.ObfuscateField = true;
                            this.settings.ObfuscateFieldPublic = true;
                            this.settings.ObfuscateFieldProtected = true;
                            this.settings.ObfuscateFieldPrivate = true;
                            this.settings.ObfuscateFieldInternal = true;

                            this.settings.ObfuscateFieldSerializeAble = false;

                            this.settings.ObfuscateProperty = true;

                            this.settings.ObfuscateEvent = true;

                            this.settings.ObfuscateMethod = true;
                            this.settings.ObfuscateMethodPublic = true;
                            this.settings.ObfuscateMethodProtected = true;
                            this.settings.ObfuscateMethodPrivate = true;
                            this.settings.ObfuscateMethodInternal = true;
                            this.settings.ObfuscateUnityMethod = false;

                            //

                            this.settings.ObfuscateClassGeneric = true;
                            this.settings.ObfuscateClassAbstract = true;
                            this.settings.ObfuscateClassSerializeAble = false;
                            if (OPS.Obfuscator.Setting.Settings.ObfuscatorType == OPS.Obfuscator.Setting.EObfuscatorType.Pro)
                            {
                                this.settings.ObfuscateUnityClasses = true;
                                this.settings.ObfuscateUnityPublicFields = true;
                            }
                            else
                            {
                                this.settings.ObfuscateUnityClasses = false;
                                this.settings.ObfuscateUnityPublicFields = false;
                            }

                            this.settings.ObfuscateEnumValues = true;

                            this.settings.TryFindGuiMethods = true;
                            this.settings.TryFindAnimationMethods = true;

                            this.settings.CheckForReflectionAndCoroutine = true;

                            //

                            if (OPS.Obfuscator.Setting.Settings.ObfuscatorType == OPS.Obfuscator.Setting.EObfuscatorType.Pro)
                            {
                                this.settings.AddRandomCode = false;
                                this.settings.ObfuscateString = true;
                                this.settings.StoreObfuscatedStrings = true;
                            }
                            else
                            {
                                this.settings.AddRandomCode = false;
                                this.settings.ObfuscateString = false;
                                this.settings.StoreObfuscatedStrings = false;
                            }

                            break;
                        }
                }
            }
        }

        //Tabs
        private int tabIndex;

        //Namespace
        public string[] AssemblyArray = { "" };

        //Namespace
        public string[] DependencyAssemblyArray = { "" };

        //Namespace
        public string[] NamespaceArray = { "" };

        //Attribute
        public string[] AttributeArray = { "" };

        //Scroll
        private Vector2 scrollPosition = Vector2.zero;

        // Add menu item named "My Window" to the Window menu
        [MenuItem("OPS/Obfuscator/Obfuscator Free Settings")]
        public static void ShowWindow()
        {
            //Show existing window instance. If one doesn't exist, make one.
            EditorWindow.GetWindow<SettingsWindow>("Obfuscator");
        }

        void OnEnable()
        {
            this.ReloadSettings();
            AssemblyArray = this.settings.AdditionalAssemblyList == null ? new string[0] : this.settings.AdditionalAssemblyList.ToArray();
            DependencyAssemblyArray = this.settings.DependencyAssemblyDirectoryList == null ? new string[0] : this.settings.DependencyAssemblyDirectoryList.ToArray();
            NamespaceArray = this.settings.NamespacesToIgnoreList == null ? new string[0] : this.settings.NamespacesToIgnoreList.ToArray();
            AttributeArray = this.settings.AttributesBehaveLikeDoNotRenameList == null ? new string[0] : this.settings.AttributesBehaveLikeDoNotRenameList.ToArray();
        }

        void OnDisable()
        {
            this.SaveSettings();
        }

        void OnGUI()
        {
            try
            {
                this.scrollPosition = GUILayout.BeginScrollView(this.scrollPosition, false, false, GUILayout.Width(position.width), GUILayout.Height(position.height));

                GUIStyle var_Style = new GUIStyle("button");
                var_Style.normal.background = null;
                var_Style.active.background = null;

                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button((Texture)EditorGUIUtility.Load("Assets/OPS/Obfuscator.Free/Editor/Gui/Rate.png"), var_Style, GUILayout.MaxWidth(100), GUILayout.MaxHeight(26)))
                {
                    Application.OpenURL("https://assetstore.unity.com/packages/tools/utilities/obfuscator-free-89420");
                }
                if (GUILayout.Button((Texture)EditorGUIUtility.Load("Assets/OPS/Obfuscator.Free/Editor/Gui/BugQuestion.png"), var_Style, GUILayout.MaxWidth(200), GUILayout.MaxHeight(26)))
                {
                    Application.OpenURL("mailto:orangepearsoftware@gmail.com?subject=ObfuscatorFree_Bug");
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label((Texture)EditorGUIUtility.Load("Assets/OPS/Obfuscator.Free/Editor/Gui/Header_Icon.png"), GUILayout.MaxWidth(24), GUILayout.MaxHeight(24), GUILayout.MinWidth(24), GUILayout.MinHeight(24));
                Text.GuiBold("Obfuscator", 150, 24);
                GUILayout.EndHorizontal();

                EditorGUILayout.HelpBox("De-/Activate Obfuscator here.", MessageType.Info);

                Row.GuiBold("Obfuscate Globally: ", ref this.settings.ObfuscateGlobally);

                GUILayout.Space(10);

                GUI.enabled = this.settings.ObfuscateGlobally;

                this.tabIndex = GUILayout.Toolbar(this.tabIndex, new Texture[] { (Texture)EditorGUIUtility.Load("Assets/OPS/Obfuscator.Free/Editor/Gui/General_T32x.png"), (Texture)EditorGUIUtility.Load("Assets/OPS/Obfuscator.Free/Editor/Gui/Advanced_T32x.png"), (Texture)EditorGUIUtility.Load("Assets/OPS/Obfuscator.Free/Editor/Gui/Security_T32x.png"), (Texture)EditorGUIUtility.Load("Assets/OPS/Obfuscator.Free/Editor/Gui/Addon_T32x.png") });
                switch (this.tabIndex)
                {
                    case 0:
                        {
                            this.GeneralTab();
                            break;
                        }
                    case 1:
                        {
                            this.AdvancedTab();
                            break;
                        }
                    case 2:
                        {
                            this.SecurityTab();
                            break;
                        }
                    case 3:
                        {
                            this.AddonTab();
                            break;
                        }
                }

                GUI.enabled = true;

                GUILayout.EndScrollView();

                if (GUI.changed)
                {
                    this.SaveSettings();
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
                this.Close();
            }
        }

        private void GeneralTab()
        {
            Text.GuiBold("Profile");

            EditorGUILayout.HelpBox("The three following buttons show your current level of obfuscation.\nAdditionally you can press on of those to activate a predefined obfuscation profile.", MessageType.Info);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            int var_SecurityLevel = this.securityLevel;
            bool var_Profile_Simple = false;
            bool var_Profile_Standard = false;
            bool var_Profile_Optimal = false;
            if (var_SecurityLevel == 1)
            {
                var_Profile_Simple = true;
            }
            if (var_SecurityLevel == 2)
            {
                var_Profile_Standard = true;
            }
            if (var_SecurityLevel == 3)
            {
                var_Profile_Optimal = true;
            }

            if (Switch.GuiNoRef(var_Profile_Simple, "Assets/OPS/Obfuscator.Free/Editor/Gui/Profile_Simple.png", "Assets/OPS/Obfuscator.Free/Editor/Gui/Profile_Simple_Unselect.png"))
            {
                this.securityLevel = 1;
            }
            if (Switch.GuiNoRef(var_Profile_Standard, "Assets/OPS/Obfuscator.Free/Editor/Gui/Profile_Standard.png", "Assets/OPS/Obfuscator.Free/Editor/Gui/Profile_Standard_Unselect.png"))
            {
                this.securityLevel = 2;
            }
            if (OPS.Obfuscator.Setting.Settings.ObfuscatorType != OPS.Obfuscator.Setting.EObfuscatorType.Pro)
            {
                GUI.enabled = false;
            }
            if (Switch.GuiNoRef(var_Profile_Optimal, "Assets/OPS/Obfuscator.Free/Editor/Gui/Profile_Optimal.png", "Assets/OPS/Obfuscator.Free/Editor/Gui/Profile_Optimal_Unselect.png"))
            {
                this.securityLevel = 3;
            }
            GUI.enabled = this.settings.ObfuscateGlobally;

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            
            if (OPS.Obfuscator.Setting.Settings.ObfuscatorType != OPS.Obfuscator.Setting.EObfuscatorType.Pro)
            {
                GUI.enabled = false;
            }

            Text.GuiBold("Assemblies");

            EditorGUILayout.HelpBox("Define here which assemblies shall get obfuscated!", MessageType.Info);

            EditorGUILayout.HelpBox("Activation: Obfuscate all Assembly Definition Files. Deactivation: Obfuscate only Assembly-CSharp.dll.", MessageType.Info);
            Row.GuiGold("All Assembly Definition Files:", ref this.settings.AllAsmdefAssemblies);

            EditorGUILayout.HelpBox("Add here names of external assemblies you want to obfuscate. (For example MyAssembly.dll).", MessageType.Info);
            ScriptableObject var_ScriptTarget = this;
            SerializedObject var_SerializedObject = new SerializedObject(var_ScriptTarget);
            SerializedProperty var_AssemblyStringsProperty = var_SerializedObject.FindProperty("AssemblyArray");

            EditorGUILayout.PropertyField(var_AssemblyStringsProperty, new GUIContent("Assemblies"), true);
            var_SerializedObject.ApplyModifiedProperties();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Add Line"))
            {
                List<String> var_TempList = new List<string>(this.AssemblyArray);
                var_TempList.Add("");
                this.AssemblyArray = var_TempList.ToArray();
            }
            if (GUILayout.Button("Remove Line"))
            {
                List<String> var_TempList = new List<string>(this.AssemblyArray);
                if (var_TempList.Count > 0)
                {
                    var_TempList.RemoveAt(var_TempList.Count - 1);
                }
                this.AssemblyArray = var_TempList.ToArray();
            }
            GUILayout.EndHorizontal();

            this.settings.AdditionalAssemblyList = new List<string>(this.AssemblyArray);

            GUI.enabled = this.settings.ObfuscateGlobally;

	    //Dependency Assembly

            EditorGUILayout.HelpBox("If you receive some errors, like 'Assembly XYZ could not be resolved!', 'Assembly XYZ could not be found!' or similar, add their directory path here. Those assemblies can be mostly found in your projects 'Asset' or 'Packages' directory. For example if the assembly GameAnalytics was not found, you can locate it here 'Assets/GameAnalytics/Plugins/GameAnalytics.dll'. Add the relative directory path to the below list 'Assets/GameAnalytics/Plugins'. If you still receive the error add the full path like '[D:]/[Your Project]/Assets/GameAnalytics/Plugins'.", MessageType.Info);
            EditorGUILayout.HelpBox("Do not add editor assemblies!", MessageType.Warning);

            SerializedProperty var_DependencyAssemblyArrayStringsProperty = var_SerializedObject.FindProperty("DependencyAssemblyArray");

            EditorGUILayout.PropertyField(var_DependencyAssemblyArrayStringsProperty, new GUIContent("Assembly Dependencies"), true);
            var_SerializedObject.ApplyModifiedProperties();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Add Line"))
            {
                List<String> var_TempList = new List<string>(this.DependencyAssemblyArray);
                var_TempList.Add("");
                this.DependencyAssemblyArray = var_TempList.ToArray();
            }
            if (GUILayout.Button("Remove Line"))
            {
                List<String> var_TempList = new List<string>(this.DependencyAssemblyArray);
                if (var_TempList.Count > 0)
                {
                    var_TempList.RemoveAt(var_TempList.Count - 1);
                }
                this.DependencyAssemblyArray = var_TempList.ToArray();
            }
            GUILayout.EndHorizontal();

            this.settings.DependencyAssemblyDirectoryList = new List<string>(this.DependencyAssemblyArray);

            Text.GuiBold("Obfuscation");

            EditorGUILayout.HelpBox("Define here generally what names shall get obfuscated!", MessageType.Info);

            Table.Gui("Obfuscate:", "Class", "Field", "Property", "Event", "Method", ref this.settings.ObfuscateClass, ref this.settings.ObfuscateField, ref this.settings.ObfuscateProperty, ref this.settings.ObfuscateEvent, ref this.settings.ObfuscateMethod);

            Text.GuiBold("Namespace");

            if (OPS.Obfuscator.Setting.Settings.ObfuscatorType != OPS.Obfuscator.Setting.EObfuscatorType.Pro)
            {
                GUI.enabled = false;
            }
            EditorGUILayout.HelpBox("Define here generally if namespace names shall get obfuscated.", MessageType.Info);
            Row.GuiGold("Namespace:", ref this.settings.ObfuscateNamespace);
            GUI.enabled = this.settings.ObfuscateGlobally;

            EditorGUILayout.HelpBox("To make Obfuscator Free ignore easily some Namespaces, enter it here. All Namespaces beginning like your entered one will get ignored. (Example the entry: 'UnityStandardAssets'. All Namespaces beginning with 'UnityStandardAssets' will get ignored (and so it's Content)).", MessageType.Info);

            SerializedProperty var_NamespaceStringsProperty = var_SerializedObject.FindProperty("NamespaceArray");

            EditorGUILayout.PropertyField(var_NamespaceStringsProperty, new GUIContent("Namespaces"), true);
            var_SerializedObject.ApplyModifiedProperties();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Add Line"))
            {
                List<String> var_TempList = new List<string>(this.NamespaceArray);
                var_TempList.Add("");
                this.NamespaceArray = var_TempList.ToArray();
            }
            if (GUILayout.Button("Remove Line"))
            {
                List<String> var_TempList = new List<string>(this.NamespaceArray);
                if (var_TempList.Count > 0)
                {
                    var_TempList.RemoveAt(var_TempList.Count - 1);
                }
                this.NamespaceArray = var_TempList.ToArray();
            }
            GUILayout.EndHorizontal();

            this.settings.NamespacesToIgnoreList = new List<string>(this.NamespaceArray);

            EditorGUILayout.HelpBox("When you activate 'Vice Versa Namespace skipping', every content (class/methods/..) belonging to a namespace that is in the bottom list gets obfuscated. Every namespace that is not in the list will get skipped. The advantage is, if you use many external plugins that shall get skipped while obfuscation and you only want your namespaces to get obfuscated, it reduces your administration effort.", MessageType.Info);
            Row.Gui("Vice Versa Namespace skipping:", ref this.settings.NamespaceViceVersa);
        }

        private void AdvancedTab()
        {
            Text.GuiBold("Class");

            Table.Gui("Obfuscate:", "Internal", "Private", "Protected", "Public", ref this.settings.ObfuscateClassInternal, ref this.settings.ObfuscateClassPrivate, ref this.settings.ObfuscateClassProtected, ref this.settings.ObfuscateClassPublic);

            EditorGUILayout.HelpBox("Obfuscation of serializeable classes and members will generate problems if you are trying to deserialize/load data that got obfuscated with different names! So use the Save Mapping setting.", MessageType.Warning);

            Row.Gui("Obfuscate serializeables:", ref this.settings.ObfuscateClassSerializeAble);

            EditorGUILayout.HelpBox("Obfuscation of Generic or Abstract classes can lead to warning messages (like 'XYZ can not be an abstract class' or 'Serialization XYZ error') in the build log. This does not harm. But if you recognize any problems or limitations deactivate it.", MessageType.Warning);

            Row.Gui("Obfuscate generic classes:", ref this.settings.ObfuscateClassGeneric);
            Row.Gui("Obfuscate abstract classes:", ref this.settings.ObfuscateClassAbstract);

            if (OPS.Obfuscator.Setting.Settings.ObfuscatorType != OPS.Obfuscator.Setting.EObfuscatorType.Pro)
            {
                GUI.enabled = false;
            }
            EditorGUILayout.HelpBox("Define here if classes that inherite from MonoBehaviour/NetworkBehaviour or ScriptableObject should get Obfuscated. For more security, they will automatically be placed in the same namespace.\nWhile obfuscation and after, 'Reloading Assets' appears. Thats normal, do not worry.", MessageType.Info);
            Row.GuiGold("Obfuscate unity class names:", ref this.settings.ObfuscateUnityClasses);
            GUI.enabled = this.settings.ObfuscateGlobally;

            Text.GuiBold("Field");

            Table.Gui("Obfuscate:", "Internal", "Private", "Protected", "Public", ref this.settings.ObfuscateFieldInternal, ref this.settings.ObfuscateFieldPrivate, ref this.settings.ObfuscateFieldProtected, ref this.settings.ObfuscateFieldPublic);

            EditorGUILayout.HelpBox("Obfuscation of serializeable classes and members will generate problems if you are trying to deserialize/load data that got obfuscated with different names! So use the Save Mapping setting. You have to activate obfuscation of serializable classes too.", MessageType.Warning);

            Row.Gui("Obfuscate serializeable fields:", ref this.settings.ObfuscateFieldSerializeAble);

            EditorGUILayout.HelpBox("Define here if values of enums shall get Obfuscated. Deactivate it, if you use 'ToString()' with enums.", MessageType.Warning);
            Row.Gui("Obfuscate enum values:", ref this.settings.ObfuscateEnumValues);

            if (OPS.Obfuscator.Setting.Settings.ObfuscatorType != OPS.Obfuscator.Setting.EObfuscatorType.Pro)
            {
                GUI.enabled = false;
            }
            EditorGUILayout.HelpBox("If you use GameObjects in your Scene, which use Editor editable fields, deactivate this.", MessageType.Warning);
            Row.GuiGold("Obfuscate unity public fields:", ref this.settings.ObfuscateUnityPublicFields);
            GUI.enabled = this.settings.ObfuscateGlobally;

            Text.GuiBold("Method");

            Table.Gui("Obfuscate:", "Internal", "Private", "Protected", "Public", ref this.settings.ObfuscateMethodInternal, ref this.settings.ObfuscateMethodPrivate, ref this.settings.ObfuscateMethodProtected, ref this.settings.ObfuscateMethodPublic);

            EditorGUILayout.HelpBox("Try to auto find used Gui methods. Mostly it will not find all methods. So add to methods, that appear not to get called in game, the 'Obfuscator.DoNotRenameAttribute'.", MessageType.Info);
            Row.Gui("Auto find Gui methods: ", ref this.settings.TryFindGuiMethods);

            EditorGUILayout.HelpBox("Try to auto find used Animation methods. Mostly it will not find all methods. So add to methods, that appear not to get called in game, the 'Obfuscator.DoNotRenameAttribute'.", MessageType.Info);
            Row.Gui("Auto find Animation methods: ", ref this.settings.TryFindAnimationMethods);

            if (OPS.Obfuscator.Setting.Settings.ObfuscatorType != OPS.Obfuscator.Setting.EObfuscatorType.Pro)
            {
                GUI.enabled = false;
            }
            EditorGUILayout.HelpBox("BETA: Obfuscate Unity methods like: Awake, Start, Update, ...", MessageType.Info);
            Row.GuiGold("Obfuscate Unity methods: ", ref this.settings.ObfuscateUnityMethod);
            GUI.enabled = this.settings.ObfuscateGlobally;

            Text.GuiBold("Attribute");
            EditorGUILayout.HelpBox("Add here Attributes you want to behave like the Attribute DoNotRename. These Attribute must entered with their fullname. For example, if you want the Unity NetworkBehaviour Attribute ClientRpc to behave like DoNotRename you have to enter ClientRpcAttribute. So don't forget the 'Attribute' at the ending if there is one.", MessageType.Info);

            //
            ScriptableObject var_Target = this;
            SerializedObject var_SerializedObject = new SerializedObject(var_Target);
            SerializedProperty var_StringsProperty = var_SerializedObject.FindProperty("AttributeArray");

            EditorGUILayout.PropertyField(var_StringsProperty, new GUIContent("Attributes"), true);
            var_SerializedObject.ApplyModifiedProperties();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Add Line"))
            {
                List<String> var_TempList = new List<string>(this.AttributeArray);
                var_TempList.Add("");
                this.AttributeArray = var_TempList.ToArray();
            }
            if (GUILayout.Button("Remove Line"))
            {
                List<String> var_TempList = new List<string>(this.AttributeArray);
                if (var_TempList.Count > 0)
                {
                    var_TempList.RemoveAt(var_TempList.Count - 1);
                }
                this.AttributeArray = var_TempList.ToArray();
            }
            GUILayout.EndHorizontal();

            this.settings.AttributesBehaveLikeDoNotRenameList = new List<string>(this.AttributeArray);

            //

            Text.GuiBold("Reflection and Coroutine");
            EditorGUILayout.HelpBox("When you activate this option, the obfuscator searchs for strings, matching classes/methods/fields/... names and will not obfuscate those. If you use Reflection (like Type.GetField([Name])) or Coroutines (like StartCoroutine([Name]) activate this option. Else deactivate this setting to enhance the obfuscation.", MessageType.Info);
            Row.Gui("Search matching members: ", ref this.settings.CheckForReflectionAndCoroutine);

            //

            Text.GuiBold("Mapping");
            EditorGUILayout.HelpBox("Manage the renaming mapping used in the obfuscator process. Save it at some location and reuse it to obfuscate with the same pattern again. (If you enable obfuscate serializeables for example, the names for serializeable classes/fields/... have to be the exact same in the different builds.)", MessageType.Info);
            Row.Gui("Save Renaming Mapping: ", ref this.settings.SaveRenamingMappingToPathFile);
            GUILayout.BeginHorizontal();
            Text.Gui("File Path: ");
            this.settings.RenamingMappingPathFile = EditorGUILayout.TextField(this.settings.RenamingMappingPathFile);
            GUILayout.EndHorizontal();
            Row.Gui("Load Renaming Mapping: ", ref this.settings.LoadRenamingMappingFromPathFile);
        
			//

            Text.GuiBold("Logging");
            EditorGUILayout.HelpBox("If you want to log in a custom file instead of the default file 'Log/[BuildTarget].txt', please enter here a full file path. Otherwise leave it blank.", MessageType.Info);
            GUILayout.BeginHorizontal();
            Text.Gui("File Path: ");
            this.settings.CustomLogPathFile = EditorGUILayout.TextField(this.settings.CustomLogPathFile);
            GUILayout.EndHorizontal();
		}

        private void SecurityTab()
        {
            if (OPS.Obfuscator.Setting.Settings.ObfuscatorType != OPS.Obfuscator.Setting.EObfuscatorType.Pro)
            {
                GUI.enabled = false;
            }
            Text.GuiBold("String");
            EditorGUILayout.HelpBox("Enable RSA String Encryption. The Obfuscation of Strings is good to have. But even strong String Obfuscation can get broken. So do never store sensitive data in your Game. But keep in mind, String Obfuscation costs performance while the Game is running.", MessageType.Warning);

            Row.GuiGold("Obfuscate Strings: ", ref this.settings.ObfuscateString);

            EditorGUILayout.HelpBox("To increase the runtime performance while using String Obfuscation activate: 'Store Obfuscated Strings'. 'Store Obfuscated Strings' will make the Obfuscator store the Obfuscated String and its decrypted in the RAM. Gives greate Performance boost. But the decrypted String is stored in the RAM, so you have weigh it by yourself if you want to use it. (I personally recommend to activate it.)", MessageType.Warning);
            Row.GuiGold("Store Obfuscated Strings: ", ref this.settings.StoreObfuscatedStrings);

            EditorGUILayout.HelpBox("When you want to use String obfuscation in an Windows Metro Project. Please switch the build platform at first to a standalone platform, close and reopen Window->Obfuscator Pro Settings and press here in the Obfuscator settings the \"Generate\" button, then switch back to our old build project.", MessageType.Warning);
#if UNITY_WINRT
            GUI.enabled = false;
#endif
            GUILayout.BeginHorizontal();
            Text.Gui("Generate new RSA keys: ");
            if (GUILayout.Button("Generate"))
            {
            }

            GUILayout.EndHorizontal();
#if UNITY_WINRT
            if (OPS.Obfuscator.Setting.Settings.ObfuscatorType == OPS.Obfuscator.Setting.EObfuscatorType.Pro)
            {
                GUI.enabled = this.settings.ObfuscateGlobally;
            }
#endif
            Text.GuiBold("Assembly");

            EditorGUILayout.HelpBox("Add Random Source Code based on existing Methods and Classes.", MessageType.Info);
            Row.GuiGold("Add Random Code: ", ref this.settings.AddRandomCode);

            GUI.enabled = this.settings.ObfuscateGlobally;
        }

        private void AddonTab()
        {
            Text.GuiBold("Addon");
            EditorGUILayout.HelpBox("Manage here the activation of included addons. (Custom Addons can be included in the future.)", MessageType.Info);
            Row.Gui("Use Google Addon: ", ref this.settings.UseGoogleAddon);
            Row.Gui("Use Facebook Addon: ", ref this.settings.UseFacebookAddon);
            Row.Gui("Use Photon Addon: ", ref this.settings.UsePhotonAddon);
            Row.Gui("Use PlayMaker Addon: ", ref this.settings.UsePlayMakerAddon);
        }
    }
}
#endif