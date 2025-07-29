#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;

public class MultiSceneAssetLogger : MonoBehaviour
{
    [Header("Ana Klasör Adı")]
    public string mainFolderName = "MAZE"; // 1) Organize edilecek dosyaların taşınacağı ana klasör ismi.
    
    [Header("Taranacak Sahneler")]
    public List<SceneAsset> scenes = new List<SceneAsset>(); // 2) Kullanıcının belirlediği sahne dosyaları listesi.

    // 3) Seçilen sahnelerdeki tüm script, materyal ve prefab bilgilerini tek bir rapor dosyasına yazar.
    [ContextMenu("Seçili Sahneleri Tara ve Tek Dosyaya Yaz")]
    public void LogAllScenesToSingleFile()
    {
        string path = Application.dataPath + "/SceneAssetsReport.txt"; // 4) Raporun kaydedileceği dosya yolu.
        List<string> lines = new List<string>(); // 5) Rapor satırlarını tutacak liste.
        
        // 6) Eğer rapor dosyası varsa önceki içerikleri koruyarak yeni taramayı ekle.
        if (File.Exists(path))
        {
            lines.AddRange(File.ReadAllLines(path));
            lines.Add(""); 
            lines.Add(new string('-', 80));
            lines.Add($"YENİ TARAMA - {System.DateTime.Now}");
            lines.Add(new string('-', 80));
        }
        else // 7) Yoksa yeni bir rapor dosyası oluştur.
        {
            lines.Add("=== TÜM SAHNELER VARLIK RAPORU ===");
            lines.Add("İlk Oluşturulma: " + System.DateTime.Now);
        }
        
        lines.Add("");

        // 8) Listedeki tüm sahneleri sırayla tara.
        foreach (var sceneAsset in scenes)
        {
            if (sceneAsset == null) continue;

            string scenePath = AssetDatabase.GetAssetPath(sceneAsset); // 9) Sahne yolunu bul.
            Scene scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single); // 10) Sahneyi aç.

            // 11) Rapor dosyasına sahne bilgilerini ekle.
            lines.Add(new string('=', 80));
            lines.Add($"SAHNE: {scene.name}");
            lines.Add($"Dosya Yolu: {scenePath}");
            lines.Add($"Taranma Zamanı: {System.DateTime.Now}");
            lines.Add(new string('=', 80));
            lines.Add("");

            GameObject[] allObjects = scene.GetRootGameObjects(); // 12) Sahnedeki tüm kök objeleri al.

            HashSet<MonoBehaviour> uniqueScripts = new HashSet<MonoBehaviour>(); // 13) Tekrarsız scriptler.
            HashSet<Material> uniqueMaterials = new HashSet<Material>(); // 14) Tekrarsız materyaller.
            HashSet<GameObject> uniquePrefabs = new HashSet<GameObject>(); // 15) Tekrarsız prefablar.

            // 16) Sahnedeki objeleri tara ve script, materyal, prefab bilgilerini topla.
            foreach (GameObject obj in allObjects)
            {
                foreach (MonoBehaviour script in obj.GetComponentsInChildren<MonoBehaviour>(true))
                {
                    if (script != null)
                        uniqueScripts.Add(script);
                }

                foreach (Renderer renderer in obj.GetComponentsInChildren<Renderer>(true))
                {
                    foreach (Material mat in renderer.sharedMaterials)
                    {
                        if (mat != null)
                            uniqueMaterials.Add(mat);
                    }
                }

                foreach (Transform t in obj.GetComponentsInChildren<Transform>(true))
                {
                    GameObject prefab = PrefabUtility.GetCorrespondingObjectFromSource(t.gameObject);
                    if (prefab != null)
                        uniquePrefabs.Add(prefab);
                }
            }

            // 17) Script bilgilerini rapora ekle.
            lines.Add("  --- Scriptler ---");
            if (uniqueScripts.Count == 0) lines.Add("    (Yok)");
            foreach (var script in uniqueScripts)
            {
                MonoScript monoScript = MonoScript.FromMonoBehaviour(script);
                if (monoScript != null)
                {
                    string scriptPath = AssetDatabase.GetAssetPath(monoScript);
                    lines.Add($"    {script.GetType().Name}  -->  {scriptPath}");
                }
            }
            lines.Add("");

            // 18) Materyal bilgilerini rapora ekle.
            lines.Add("  --- Materyaller ---");
            if (uniqueMaterials.Count == 0) lines.Add("    (Yok)");
            foreach (var mat in uniqueMaterials)
            {
                string matPath = AssetDatabase.GetAssetPath(mat);
                lines.Add($"    {mat.name}  -->  {matPath}");
            }
            lines.Add("");

            // 19) Prefab bilgilerini rapora ekle.
            lines.Add("  --- Prefablar ---");
            if (uniquePrefabs.Count == 0) lines.Add("    (Yok)");
            foreach (var prefab in uniquePrefabs)
            {
                string prefabPath = AssetDatabase.GetAssetPath(prefab);
                lines.Add($"    {prefab.name}  -->  {prefabPath}");
            }
            lines.Add("");
            lines.Add("");
        }

        // 20) Dosyaya tüm rapor satırlarını yaz.
        File.WriteAllLines(path, lines.ToArray());
        AssetDatabase.Refresh();

        // 21) Konsola raporun kaydedildiğini yazdır.
        if (File.Exists(path))
        {
            Debug.Log("Rapor güncellendi: " + path);
        }
        else
        {
            Debug.Log("Yeni rapor oluşturuldu: " + path);
        }
    }

    // 22) Rapor dosyasını temizlemek için bir fonksiyon.
    [ContextMenu("Rapor Dosyasını Temizle")]
    public void ClearReportFile()
    {
        string path = Application.dataPath + "/SceneAssetsReport.txt";
        if (File.Exists(path))
        {
            File.Delete(path);
            AssetDatabase.Refresh();
            Debug.Log("Rapor dosyası silindi: " + path);
        }
        else
        {
            Debug.Log("Silinecek rapor dosyası bulunamadı.");
        }
    }

    // 23) Bulunan tüm assetleri türlerine göre organize eder (Script, Material, Prefab, vs.)
    [ContextMenu("Varlıkları Türüne Göre Organize Et")]
    public void OrganizeAssetsByType()
    {
        foreach (var sceneAsset in scenes)
        {
            if (sceneAsset == null) continue;

            string scenePath = AssetDatabase.GetAssetPath(sceneAsset);
            Scene scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
            
            GameObject[] allObjects = scene.GetRootGameObjects();
            HashSet<string> processedAssets = new HashSet<string>(); // 24) Aynı dosyaları tekrar taşımamak için kontrol.

            foreach (GameObject obj in allObjects)
            {
                // 25) Scriptleri organize et.
                foreach (MonoBehaviour script in obj.GetComponentsInChildren<MonoBehaviour>(true))
                {
                    if (script != null)
                    {
                        MonoScript monoScript = MonoScript.FromMonoBehaviour(script);
                        if (monoScript != null)
                        {
                            string scriptPath = AssetDatabase.GetAssetPath(monoScript);
                            MoveAssetToTypeFolder(scriptPath, "Scripts", processedAssets);
                        }
                    }
                }

                // 26) Materyalleri ve kullandıkları texture'ları organize et.
                foreach (Renderer renderer in obj.GetComponentsInChildren<Renderer>(true))
                {
                    foreach (Material mat in renderer.sharedMaterials)
                    {
                        if (mat != null)
                        {
                            string matPath = AssetDatabase.GetAssetPath(mat);
                            MoveAssetToTypeFolder(matPath, "Materials", processedAssets);
                            
                            var shader = mat.shader;
                            for (int i = 0; i < ShaderUtil.GetPropertyCount(shader); i++)
                            {
                                if (ShaderUtil.GetPropertyType(shader, i) == ShaderUtil.ShaderPropertyType.TexEnv)
                                {
                                    string propName = ShaderUtil.GetPropertyName(shader, i);
                                    Texture tex = mat.GetTexture(propName);
                                    if (tex != null)
                                    {
                                        string texPath = AssetDatabase.GetAssetPath(tex);
                                        MoveAssetToTypeFolder(texPath, "Textures", processedAssets);
                                    }
                                }
                            }
                        }
                    }
                }

                // 27) Modelleri organize et.
                foreach (MeshFilter meshFilter in obj.GetComponentsInChildren<MeshFilter>(true))
                {
                    if (meshFilter.sharedMesh != null)
                    {
                        string meshPath = AssetDatabase.GetAssetPath(meshFilter.sharedMesh);
                        MoveAssetToTypeFolder(meshPath, "Models", processedAssets);
                    }
                }

                // 28) Ses dosyalarını organize et.
                foreach (AudioSource audioSource in obj.GetComponentsInChildren<AudioSource>(true))
                {
                    if (audioSource.clip != null)
                    {
                        string audioPath = AssetDatabase.GetAssetPath(audioSource.clip);
                        MoveAssetToTypeFolder(audioPath, "Audio", processedAssets);
                    }
                }

                // 29) Animasyonları organize et.
                foreach (Animator animator in obj.GetComponentsInChildren<Animator>(true))
                {
                    if (animator.runtimeAnimatorController != null)
                    {
                        string animPath = AssetDatabase.GetAssetPath(animator.runtimeAnimatorController);
                        MoveAssetToTypeFolder(animPath, "Animations", processedAssets);
                    }
                }

                // 30) Prefabları organize et.
                foreach (Transform t in obj.GetComponentsInChildren<Transform>(true))
                {
                    GameObject prefab = PrefabUtility.GetCorrespondingObjectFromSource(t.gameObject);
                    if (prefab != null)
                    {
                        string prefabPath = AssetDatabase.GetAssetPath(prefab);
                        MoveAssetToTypeFolder(prefabPath, "Prefabs", processedAssets);
                    }
                }
            }
        }
        
        AssetDatabase.Refresh();
        Debug.Log("Tüm varlıklar türüne göre organize edildi!");
    }

    // 31) Belirli bir asset dosyasını hedef klasöre taşır.
    private void MoveAssetToTypeFolder(string assetPath, string folderType, HashSet<string> processedAssets)
    {
        if (string.IsNullOrEmpty(assetPath) || processedAssets.Contains(assetPath)) return;
        
        processedAssets.Add(assetPath);
        
        string fileName = Path.GetFileName(assetPath);
        string mainFolder = $"Assets/{mainFolderName}";
        string targetFolder = $"{mainFolder}/{folderType}";
        
        if (!AssetDatabase.IsValidFolder(mainFolder))
        {
            AssetDatabase.CreateFolder("Assets", mainFolderName);
        }
        
        if (!AssetDatabase.IsValidFolder(targetFolder))
        {
            AssetDatabase.CreateFolder(mainFolder, folderType);
        }
        
        string newPath = $"{targetFolder}/{fileName}";
        
        if (assetPath != newPath && !File.Exists(newPath))
        {
            AssetDatabase.MoveAsset(assetPath, newPath);
        }
    }
}
#endif
