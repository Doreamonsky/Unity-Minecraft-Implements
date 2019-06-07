using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace ShanghaiWindy.Core
{

    public class ScriptableModManager : MonoBehaviour
    {
        public static System.Action<int> OnVehicleLoaded;

        public List<string> dllDirs = new List<string>();

        private System.Action OnUpdated;

        private System.Action OnFixedUpdate;

        private System.Action OnUpdateGUI;

        private System.Action<string> OnNewSceneLoaded;



        public void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
        public void Mount()
        {
#if ModSupport
            SceneManager.sceneLoaded += (scene, mod) => { OnNewSceneLoaded?.Invoke(scene.name); };

            foreach (var assemblyFile in dllDirs)
            {
                try
                {
                    var assembly = Assembly.LoadFrom(assemblyFile);
                    Module[] modules = assembly.GetModules();

                    foreach (Module module in modules)
                    {
                        var types = module.GetTypes();
                        foreach (var type in types)
                        {
                            //Mount General Interface
                            var generalInterface = type.GetInterface("IGeneralAddOn");

                            if (generalInterface != null)
                            {
                                var instance = Activator.CreateInstance(type);

                                var InitializedMethod = type.GetMethod("OnInitialized");
                                var OnUpdateMethod = type.GetMethod("OnUpdate");
                                var OnFixedUpdateMethod = type.GetMethod("OnFixedUpdate");
                                var OnUpdateGUIMethod = type.GetMethod("OnUpdateGUI");
                                var OnNewSceneLoadedMethod = type.GetMethod("OnNewSceneLoaded");

                                try
                                {
                                    InitializedMethod.Invoke(instance, null);
                                }
                                catch { }

                                OnUpdated += () =>
                                {
                                    try
                                    {
                                        OnUpdateMethod.Invoke(instance, null);
                                    }
                                    catch { }
                                };

                                OnFixedUpdate += () =>
                                {
                                    try
                                    {
                                        OnFixedUpdateMethod.Invoke(instance, null);
                                    }
                                    catch { }
                                };

                                OnUpdateGUI += () =>
                                {
                                    try
                                    {
                                        OnUpdateGUIMethod.Invoke(instance, null);
                                    }
                                    catch { }
                                };
                                OnNewSceneLoaded += (sceneName) =>
                                {
                                    try
                                    {
                                        OnNewSceneLoadedMethod.Invoke(instance, new object[] { sceneName });
                                    }
                                    catch { }
                                };
                            }


                            //Mount vehiclePath Interface
                            var vehicleInterface = type.GetInterface("IVehicleAddOn");
                            if (vehicleInterface != null)
                            {
                                var instance = Activator.CreateInstance(type);
                                var OnVehicleLoadedMethod = type.GetMethod("OnVehicleLoaded");

                                OnVehicleLoaded += (vehicle) =>
                                {
                                    try
                                    {
                                        OnVehicleLoadedMethod.Invoke(instance, new object[] { vehicle });
                                    }
                                    catch (Exception exception)
                                    {
                                        Debug.LogError(exception.Message);
                                        Debug.LogError(exception.StackTrace);
                                    }
                                };
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    Debug.Log(exception.Message);
                }


            }

#endif


        }

        private void Update()
        {
            OnUpdated?.Invoke();
        }

        private void FixedUpdate()
        {
            OnFixedUpdate?.Invoke();
        }

        private void OnGUI()
        {
            OnUpdateGUI?.Invoke();
        }




        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }
    }
}