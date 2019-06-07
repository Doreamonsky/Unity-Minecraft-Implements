using UnityEngine;
using UnityMod;

namespace MC.Mods
{
    public class HelloWorld : IGeneralAddOn
    {
        public void OnFixedUpdate()
        {
        }

        public void OnInitialized()
        {
        }

        public void OnNewSceneLoaded(string name)
        {
        }

        public void OnUpdate()
        {
        }

        public void OnUpdateGUI()
        {
            GUILayout.Label("Hello World - Mod");
        }
    }
}
