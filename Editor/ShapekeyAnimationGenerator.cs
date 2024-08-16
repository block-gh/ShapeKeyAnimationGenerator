using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class ShapeKeyAnimationGenerator : EditorWindow
{
    private GameObject targetObject;

    [MenuItem("Tools/Shape Key Animation Generator")]
    public static void ShowWindow()
    {
        GetWindow<ShapeKeyAnimationGenerator>("Shape Key Animation Generator");
    }

    private void OnGUI()
    {
        targetObject = (GameObject)EditorGUILayout.ObjectField("Target Object", targetObject, typeof(GameObject), true);

        if (GUILayout.Button("Generate Animations"))
        {
            if (targetObject != null)
            {
                GenerateAnimations();
            }
            else
            {
                Debug.LogError("Please assign a target object.");
            }
        }
    }

    private void GenerateAnimations()
    {
        List<SkinnedMeshRenderer> skinnedMeshRenderers = new List<SkinnedMeshRenderer>();
        FindSkinnedMeshRenderers(targetObject, skinnedMeshRenderers);

        if (skinnedMeshRenderers.Count == 0)
        {
            Debug.LogError("No SkinnedMeshRenderer components found in the target object or its children.");
            return;
        }

        string folderPath = EditorUtility.SaveFolderPanel("Save Animations", "Assets", "");
        if (string.IsNullOrEmpty(folderPath))
        {
            return;
        }

        foreach (SkinnedMeshRenderer skinnedMeshRenderer in skinnedMeshRenderers)
        {
            GenerateAnimationsForRenderer(skinnedMeshRenderer, folderPath);
        }

        AssetDatabase.Refresh();
        Debug.Log("Animation files generated successfully.");
    }

    private void FindSkinnedMeshRenderers(GameObject obj, List<SkinnedMeshRenderer> results)
    {
        SkinnedMeshRenderer renderer = obj.GetComponent<SkinnedMeshRenderer>();
        if (renderer != null)
        {
            results.Add(renderer);
        }

        foreach (Transform child in obj.transform)
        {
            FindSkinnedMeshRenderers(child.gameObject, results);
        }
    }

    private void GenerateAnimationsForRenderer(SkinnedMeshRenderer skinnedMeshRenderer, string folderPath)
    {
        Mesh mesh = skinnedMeshRenderer.sharedMesh;
        if (mesh == null)
        {
            Debug.LogWarning($"SkinnedMeshRenderer on {skinnedMeshRenderer.gameObject.name} does not have a valid mesh.");
            return;
        }

        for (int i = 0; i < mesh.blendShapeCount; i++)
        {
            string shapeKeyName = mesh.GetBlendShapeName(i);
            string animationName = shapeKeyName + ".anim";
            string fullPath = Path.Combine(folderPath, animationName);

            AnimationClip clip = new AnimationClip();
            clip.frameRate = 60; // フレームレートを設定

            // シェイプキーの値を100に設定するキーフレームを追加
            AnimationCurve curve = new AnimationCurve();
            curve.AddKey(0f, 100f);

            // ルートオブジェクトからの相対パスを取得
            string relativePath = AnimationUtility.CalculateTransformPath(skinnedMeshRenderer.transform, targetObject.transform);
            clip.SetCurve(relativePath, typeof(SkinnedMeshRenderer), "blendShape." + shapeKeyName, curve);

            AssetDatabase.CreateAsset(clip, "Assets" + fullPath.Substring(Application.dataPath.Length));
        }
    }
}