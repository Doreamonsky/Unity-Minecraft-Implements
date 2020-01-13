#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEditor.Callbacks;

namespace OPS.Obfuscator
{
    public class BuildPostProcessor
    {
        //Defines if an Obfuscation Process took place.
        private static bool hasObfuscated = false;

        //The Main Obfuscation Program
        private static OPS.Obfuscator.Obfuscator obfuscator;

        //Check if extern assemblys got obfuscated.
        private static bool assemblysNeedReverting = false;

        //Revert Unity Assets and external Assemblies, if postprocess got not called or update got cleared!
        [InitializeOnLoad]
        public static class OnInitializeOnLoad
        {
            static OnInitializeOnLoad()
            {
                EditorApplication.update += RestoreAssemblies;
            }
        }

        //Obfuscate Assemblies after first scene build.
        [PostProcessScene(1)]
        public static void OnPostProcessScene()
        {
            if (!hasObfuscated)
            {
                if (BuildPipeline.isBuildingPlayer && !EditorApplication.isPlayingOrWillChangePlaymode)
                {
                    try
                    { 
                        //Load Settings
                        var var_Settings = new Setting.Settings();
                        var_Settings.LoadSettings();

#if UNITY_2018_2_OR_NEWER
                        var_Settings.IsLaterUnity_2018_2 = true;
#endif

                        //Init
                        obfuscator = new Obfuscator(var_Settings);

                        //Obfuscate Assemblies
                        bool var_NoError = obfuscator.Obfuscate(UnityEditor.EditorUserBuildSettings.activeBuildTarget, new System.Collections.Generic.List<string>());
                        if (var_NoError)
                        {

                            //
                            EditorApplication.update += RestoreAssemblies;
                            assemblysNeedReverting = true;

                            //Save Assemblies
                            obfuscator.Save();
                        }
                        hasObfuscated = true;
                    }
                    catch (Exception e)
                    {
                        UnityEngine.Debug.LogError("[OPS] Error: " + e.ToString());
                    }
                    finally
                    {
                        UnityEditor.EditorApplication.UnlockReloadAssemblies();
                    }
                }
            }
        }

        //Revert external Assemblies. 
        [PostProcessBuildAttribute(1)]
        public static void OnPostprocessBuild(BuildTarget _Target, string _PathToBuiltProject)
        {
            if (hasObfuscated)
            {
                if (assemblysNeedReverting)
                {
                    RestoreAssemblies();
                }
            }

            RefreshAll();
        }
        
        private static void RestoreAssemblies()
        {
            if (BuildPipeline.isBuildingPlayer == false)
            {
                try
                {
                    assemblysNeedReverting = false;

                    if (obfuscator == null)
                    {
                        return;
                    }

                    obfuscator.RestoreTemporaryAssemblies();
                    EditorApplication.update -= RestoreAssemblies;
                }
                catch (Exception e)
                {
                    assemblysNeedReverting = true;
                    UnityEngine.Debug.LogWarning("[OPS.OBF] " + e.ToString());
                }
            }
        }

        public static void ManualRestore()
        {
            RestoreAssemblies();
        }

        private static void RefreshAll()
        {
            hasObfuscated = false;
        }
    }
}
#endif