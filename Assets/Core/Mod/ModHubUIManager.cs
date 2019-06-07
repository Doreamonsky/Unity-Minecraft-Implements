using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
namespace ShanghaiWindy.Core
{
    public class ModHubUIManager : MonoBehaviour
    {
        [System.Serializable]
        public class GUI
        {
            public GameObject Template;

            public RectTransform ModList;

            public GameObject modDetail;

            public Text modName, description, author, supportURL, meta;
        }

        public GUI gui;

        private void Start()
        {
            gui.Template.SetActive(false);

            AssetBundleManager.MonoActiveObject = this;
            StartCoroutine(AssetBundleManager.AssetBundleLoop());
            AssetBundleManager.Init();

            var modDirs = new List<string>();

            modDirs.AddRange(AssetBundleManager.modDirs);
            modDirs.AddRange(AssetBundleManager.activationData.ModsIgnoreList);

            gui.ModList.sizeDelta = new Vector2(0, gui.ModList.rect.height * modDirs.Count);

            foreach (var modDir in modDirs)
            {
                var dir = new DirectoryInfo(modDir);

                if (dir.Name == "Installs" || !dir.Exists)
                {
                    continue;
                }

                var instance = Instantiate(gui.Template, gui.Template.transform.parent, true);
                instance.SetActive(true);

                var toggle = instance.transform.Find("Toggle").GetComponent<Toggle>();
                var label = instance.transform.Find("Toggle/Label").GetComponent<Text>();

                label.text = $"Name:{dir.Name}";

                var dateLabel = instance.transform.Find("Date").GetComponent<Text>();
                dateLabel.text = $"Install Date:{dir.LastWriteTime.ToString()}";

                var detailBtn = instance.transform.Find("View").GetComponent<Button>();

                detailBtn.onClick.AddListener(() =>
                {
                    UpdateModDescription(dir.Name);
                });

                var isIgnored = AssetBundleManager.activationData.ModsIgnoreList.Contains(modDir);
                toggle.isOn = !isIgnored;

                var modDirInstance = modDir;

                toggle.onValueChanged.AddListener(val =>
                {
                    if (val)
                    {
                        if (AssetBundleManager.activationData.ModsIgnoreList.Contains(modDirInstance))
                        {
                            AssetBundleManager.activationData.ModsIgnoreList.Remove(modDirInstance);
                        }
                    }
                    else
                    {
                        AssetBundleManager.activationData.ModsIgnoreList.Add(modDirInstance);
                    }

                    AssetBundleManager.activationData.OnSave();
                });
            }
        }

        private void UpdateModDescription(string modInstallDir)
        {
            var modPackage = ModActivationData.GetModPackage(modInstallDir);

            if (modPackage != null)
            {
                gui.modDetail.SetActive(true);

                gui.modName.text = modPackage.modName;
                gui.description.text = modPackage.description;
                gui.author.text = modPackage.author;
                gui.supportURL.text = modPackage.supportURL;
                gui.meta.text = $"Version : {modPackage.modVersion} Tested: {modPackage.testedGameVersion} Build: {modPackage.buildTarget}";
            }
            else
            {
                gui.modDetail.SetActive(false);

                Debug.Log("Illegal Mod");
            }
        }
    }
}
