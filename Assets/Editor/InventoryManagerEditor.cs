using MC.Core;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace MC.CoreEditor
{
    [CustomEditor(typeof(InventoryManager))]
    public class InventoryManagerEditor : Editor
    {
        private InventoryManager inventoryManager;

        private string InvHelper(Inventory inv)
        {
            return inv == null ? "无" : inv.inventoryName;
        }

        private void OnEnable()
        {
            inventoryManager = target as InventoryManager;
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();


            if (GUILayout.Button("Update Inventories"))
            {
                inventoryManager.inventories = new List<Inventory>();

                foreach (var guid in AssetDatabase.FindAssets("t:Inventory"))
                {
                    var path = AssetDatabase.GUIDToAssetPath(guid);

                    inventoryManager.inventories.Add(AssetDatabase.LoadAssetAtPath<Inventory>(path));
                }
            }

            if (GUILayout.Button("Update Recipe"))
            {
                inventoryManager.recipeList = new List<RecipeData>();

                foreach (var guid in AssetDatabase.FindAssets("t:RecipeData"))
                {
                    var path = AssetDatabase.GUIDToAssetPath(guid);

                    inventoryManager.recipeList.Add(AssetDatabase.LoadAssetAtPath<RecipeData>(path));
                }
            }

            if (GUILayout.Button("Update Recipe Document"))
            {
                var dic = new DirectoryInfo("Doc/Recipe/");

                if (!dic.Exists)
                {
                    dic.Create();
                }
                else
                {
                    dic.Delete(true);
                    dic.Create();
                }

                var mainStringBuilder = new StringBuilder();
                mainStringBuilder.AppendLine("# 合成文档");
                mainStringBuilder.AppendLine();

                foreach (var category in inventoryManager.recipeList.GroupBy(x => x.CraftedInventory))
                {
                    var stringBuilder = new StringBuilder();
                    var invName = category.ToList()[0].CraftedInventory.inventoryName.Replace(" ", "-");

                    stringBuilder.AppendLine($"# {invName} 合成方法");
                    stringBuilder.AppendLine();

                    var iconUrl = AssetDatabase.GetAssetPath(category.ToList()[0].CraftedInventory.inventoryIcon);

                    if (!string.IsNullOrEmpty(iconUrl))
                    {
                        var guid = AssetDatabase.AssetPathToGUID(iconUrl);
                        var origin = new FileInfo(iconUrl);
                        var targetUrl = $"Doc/Recipe/{guid}{Path.GetExtension(origin.Name)}";

                        File.Copy(origin.FullName, targetUrl);
                        stringBuilder.AppendLine($"![Icon]({guid}{Path.GetExtension(origin.Name)})");
                    }

                    foreach (var recipe in category)
                    {
                        stringBuilder.AppendLine();

                        stringBuilder.AppendLine($"|第一列|第二列|第三列|\n" +
                            $"|----|-----|-----|\n" +
                            $"|{InvHelper(recipe.Recipe[0])}|{InvHelper(recipe.Recipe[1])}|{InvHelper(recipe.Recipe[2])}|\n" +
                            $"|{InvHelper(recipe.Recipe[3])}|{InvHelper(recipe.Recipe[4])}|{InvHelper(recipe.Recipe[5])}|\n" +
                            $"|{InvHelper(recipe.Recipe[6])}|{InvHelper(recipe.Recipe[7])}|{InvHelper(recipe.Recipe[8])}|");

                        stringBuilder.AppendLine();
                        stringBuilder.AppendLine($"是否需要加热: {(recipe.requireHeating ? '是' : '否')}");
                        stringBuilder.AppendLine();
                        stringBuilder.AppendLine($"生成 {recipe.CraftedInventory.inventoryName} \\* 数量 {recipe.CraftedCount}");

                        stringBuilder.AppendLine("<br/> <br/> <br/> ");
                    }

                    var fs = new FileStream($"Doc/Recipe/{invName}.md", FileMode.Create);
                    var sw = new StreamWriter(fs);
                    sw.WriteLine(stringBuilder.ToString());
                    sw.Close();
                    fs.Close();

                    mainStringBuilder.AppendLine($"{invName}的合成方法:[点击查看](Recipe/{invName}.md)");
                    mainStringBuilder.AppendLine();
                }

                var recipeFs = new FileStream($"Doc/Recipe.md", FileMode.Create);
                var recipeSw = new StreamWriter(recipeFs);
                recipeSw.WriteLine(mainStringBuilder.ToString());
                recipeSw.Close();
                recipeFs.Close();

                EditorUtility.RevealInFinder($"Doc/");
            }
            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }

}
