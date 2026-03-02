#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;
using System.Collections.Generic;

public class OctopusGameLevelCompiler : EditorWindow
{
    private string sceneName = "NewLevel";
    private string assetBundleName = "level_bundle";
    private string sceneToCompile = "";
    private SceneAsset sceneAsset;
    private Texture2D levelIcon;
    private Vector2 scrollPosition;

    // Logo texture cached
    private Texture2D logoTexture;

    [MenuItem("Tools/Octopus Game/Level Manager")]
    public static void ShowWindow()
    {
        GetWindow<OctopusGameLevelCompiler>("Octopus Game");
    }

    private void OnEnable()
    {
        LoadLogo();
    }

    private void LoadLogo()
    {
        // Search for the logo in the project
        string[] guids = AssetDatabase.FindAssets("Icon logo 0.9 t:Texture2D");
        if (guids.Length > 0)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            logoTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
        }

        // Fallback: try direct path
        if (logoTexture == null)
        {
            logoTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Icon logo 0.9.png");
        }
    }

    private void OnGUI()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        GUILayout.Space(10);
        EditorGUILayout.LabelField("Octopus Game - Level Manager", EditorStyles.boldLabel);
        GUILayout.Space(10);

        // New Level Section
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("Create New Level", EditorStyles.boldLabel);
        GUILayout.Space(5);

        sceneName = EditorGUILayout.TextField("Scene Name:", sceneName);
        assetBundleName = EditorGUILayout.TextField("Asset Bundle Name:", assetBundleName);

        GUILayout.Space(10);

        if (GUILayout.Button("New Level", GUILayout.Height(30)))
        {
            CreateNewLevel();
        }

        EditorGUILayout.EndVertical();

        GUILayout.Space(20);

        // Compile Level Section
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("Compile Level", EditorStyles.boldLabel);
        GUILayout.Space(5);

        sceneAsset = (SceneAsset)EditorGUILayout.ObjectField("Scene to Compile:",
            sceneAsset, typeof(SceneAsset), false);

        if (sceneAsset != null)
        {
            sceneToCompile = sceneAsset.name;
        }

        GUILayout.Space(5);
        levelIcon = (Texture2D)EditorGUILayout.ObjectField("Level Icon:", levelIcon, typeof(Texture2D), false);

        if (levelIcon != null)
        {
            GUILayout.Space(5);
            EditorGUILayout.LabelField("Icon Preview:", EditorStyles.boldLabel);
            Rect iconRect = GUILayoutUtility.GetRect(100, 100, GUILayout.ExpandWidth(false));
            EditorGUI.DrawPreviewTexture(iconRect, levelIcon);
        }

        EditorGUILayout.HelpBox("Select a scene and an icon texture to compile into the asset bundle.", MessageType.Info);

        GUILayout.Space(10);

        if (GUILayout.Button("Compile This Level", GUILayout.Height(30)))
        {
            CompileLevel();
        }

        EditorGUILayout.EndVertical();

        GUILayout.Space(20);

        // Quick Actions
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("Quick Actions", EditorStyles.boldLabel);
        GUILayout.Space(5);

        if (GUILayout.Button("Open Asset Bundle Output Folder"))
        {
            string path = Application.dataPath + "/../AssetBundles";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            EditorUtility.RevealInFinder(path);
        }

        EditorGUILayout.EndVertical();

        GUILayout.Space(20);

        // ──────────────────────────────────────────
        // Logo Section
        // ──────────────────────────────────────────
        if (logoTexture == null)
        {
            // Try to reload if not found yet
            LoadLogo();
        }

        if (logoTexture != null)
        {
            float windowWidth = EditorGUIUtility.currentViewWidth;

            // Logo dimensions: keep aspect ratio, max width = window width - 20px padding
            float maxLogoWidth = Mathf.Min(windowWidth - 20f, 200f);
            float aspect = (float)logoTexture.height / logoTexture.width;
            float logoHeight = maxLogoWidth * aspect;

            // Center the logo horizontally
            float offsetX = (windowWidth - maxLogoWidth) * 0.5f;

            GUILayout.Space(5);
            Rect logoRect = GUILayoutUtility.GetRect(windowWidth, logoHeight);
            logoRect.x = offsetX;
            logoRect.width = maxLogoWidth;

            GUI.DrawTexture(logoRect, logoTexture, ScaleMode.ScaleToFit, true);
            GUILayout.Space(5);
        }
        else
        {
            // Show a placeholder message if the logo is not found
            EditorGUILayout.HelpBox("Logo 'Icon logo 0.9.png' not found.\nMake sure it is imported into the project.", MessageType.Warning);
        }

        EditorGUILayout.EndScrollView();
    }

    private void CreateNewLevel()
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            EditorUtility.DisplayDialog("Error", "Please enter a scene name.", "OK");
            return;
        }

        if (string.IsNullOrEmpty(assetBundleName))
        {
            EditorUtility.DisplayDialog("Error", "Please enter an asset bundle name.", "OK");
            return;
        }

        // Create Scenes folder if it doesn't exist
        string scenesPath = "Assets/Scenes";
        if (!AssetDatabase.IsValidFolder(scenesPath))
        {
            AssetDatabase.CreateFolder("Assets", "Scenes");
        }

        // Create a folder for this specific level
        string levelFolderPath = $"{scenesPath}/{sceneName}";
        if (!AssetDatabase.IsValidFolder(levelFolderPath))
        {
            string guid = AssetDatabase.CreateFolder(scenesPath, sceneName);
            if (string.IsNullOrEmpty(guid))
            {
                EditorUtility.DisplayDialog("Error", $"Failed to create folder: {levelFolderPath}", "OK");
                return;
            }
            Debug.Log($"Created level folder: {levelFolderPath}");
        }
        else
        {
            EditorUtility.DisplayDialog("Warning",
                $"Folder '{sceneName}' already exists. Scene will be created in existing folder.",
                "OK");
        }

        // Create new scene in the level folder
        var newScene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
        string scenePath = $"{levelFolderPath}/{sceneName}.unity";

        // Save the scene
        EditorSceneManager.SaveScene(newScene, scenePath);

        // Set asset bundle name for the scene
        AssetImporter importer = AssetImporter.GetAtPath(scenePath);
        if (importer != null)
        {
            importer.assetBundleName = assetBundleName;
            importer.SaveAndReimport();
        }

        EditorUtility.DisplayDialog("Success",
            $"Level '{sceneName}' created successfully!\n\nFolder: {levelFolderPath}\nAsset Bundle: {assetBundleName}\nScene: {scenePath}",
            "OK");

        Debug.Log($"New level created: {scenePath} with asset bundle: {assetBundleName}");

        // Highlight the created folder in Project window
        Object folderObj = AssetDatabase.LoadAssetAtPath<Object>(levelFolderPath);
        if (folderObj != null)
        {
            EditorGUIUtility.PingObject(folderObj);
            Selection.activeObject = folderObj;
        }
    }

    private void CompileLevel()
    {
        if (string.IsNullOrEmpty(sceneToCompile))
        {
            EditorUtility.DisplayDialog("Error", "Please specify a scene to compile.", "OK");
            return;
        }

        // Try to find the scene in level folders first
        string scenePath = FindScenePath(sceneToCompile);

        if (string.IsNullOrEmpty(scenePath) || !File.Exists(scenePath))
        {
            EditorUtility.DisplayDialog("Error",
                $"Scene '{sceneToCompile}' not found in any level folder.",
                "OK");
            return;
        }

        Debug.Log($"Found scene at: {scenePath}");

        // Get asset bundle name from scene
        AssetImporter sceneImporter = AssetImporter.GetAtPath(scenePath);
        if (sceneImporter == null || string.IsNullOrEmpty(sceneImporter.assetBundleName))
        {
            EditorUtility.DisplayDialog("Error",
                "Scene doesn't have an asset bundle assigned. Please assign one first.",
                "OK");
            return;
        }

        string bundleName = sceneImporter.assetBundleName;
        string iconBundleName = bundleName + "_icon";

        // Create list of asset bundle builds
        List<AssetBundleBuild> builds = new List<AssetBundleBuild>();

        // Build for scene
        AssetBundleBuild sceneBuild = new AssetBundleBuild();
        sceneBuild.assetBundleName = bundleName;
        sceneBuild.assetNames = new string[] { scenePath };
        builds.Add(sceneBuild);

        // Build for icon if provided
        if (levelIcon != null)
        {
            string iconPath = AssetDatabase.GetAssetPath(levelIcon);
            if (!string.IsNullOrEmpty(iconPath))
            {
                AssetBundleBuild iconBuild = new AssetBundleBuild();
                iconBuild.assetBundleName = iconBundleName;
                iconBuild.assetNames = new string[] { iconPath };
                builds.Add(iconBuild);

                Debug.Log($"Level icon '{levelIcon.name}' will be built into: {iconBundleName}");
            }
        }

        // Force save all assets before building
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        // Create output directory
        string outputPath = Path.Combine(Application.dataPath, "..", "AssetBundles");
        outputPath = Path.GetFullPath(outputPath);

        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
            Debug.Log($"Created AssetBundles directory at: {outputPath}");
        }

        Debug.Log($"Building asset bundles to: {outputPath}");
        Debug.Log($"Scene bundle name: {bundleName}");
        if (levelIcon != null)
        {
            Debug.Log($"Icon bundle name: {iconBundleName}");
        }
        Debug.Log($"Target platform: {EditorUserBuildSettings.activeBuildTarget}");
        Debug.Log($"Building {builds.Count} bundle(s)");

        // Build ONLY the specified asset bundles
        AssetBundleManifest manifest = BuildPipeline.BuildAssetBundles(
            outputPath,
            builds.ToArray(),
            BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.ForceRebuildAssetBundle,
            EditorUserBuildSettings.activeBuildTarget);

        if (manifest == null)
        {
            EditorUtility.DisplayDialog("Build Failed",
                "Asset bundle build failed! Check the Console for errors.",
                "OK");
            Debug.LogError("Asset bundle build returned null manifest!");
            return;
        }

        // Verify the files were created
        string[] createdBundles = manifest.GetAllAssetBundles();
        Debug.Log($"Created {createdBundles.Length} asset bundle(s):");
        foreach (string bundle in createdBundles)
        {
            string bundlePath = Path.Combine(outputPath, bundle);
            if (File.Exists(bundlePath))
            {
                FileInfo fileInfo = new FileInfo(bundlePath);
                Debug.Log($"  - {bundle} ({fileInfo.Length} bytes)");
            }
        }

        string successMessage = $"Level '{sceneToCompile}' compiled successfully!\n\nScene Bundle: {bundleName}\nOutput: {outputPath}\n\nFiles created: {createdBundles.Length}";

        if (levelIcon != null)
        {
            successMessage += $"\n\nIcon Bundle: {iconBundleName}\nIcon: {levelIcon.name}";
        }

        EditorUtility.DisplayDialog("Success", successMessage, "OK");

        // Refresh asset database
        AssetDatabase.Refresh();

        // Open the folder
        EditorUtility.RevealInFinder(outputPath);
    }

    private string FindScenePath(string sceneName)
    {
        // First try direct path in level folder
        string directPath = $"Assets/Scenes/{sceneName}/{sceneName}.unity";
        if (File.Exists(directPath))
        {
            return directPath;
        }

        // Search all scene files in Scenes folder
        string[] sceneGuids = AssetDatabase.FindAssets($"t:Scene {sceneName}", new[] { "Assets/Scenes" });

        foreach (string guid in sceneGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            string fileName = Path.GetFileNameWithoutExtension(path);

            if (fileName == sceneName)
            {
                return path;
            }
        }

        return null;
    }
}
#endif