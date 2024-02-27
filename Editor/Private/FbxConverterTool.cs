using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class FbxConverterTool
{
    public static void SpliteFbxMotion2Anim(string fbxPath)
    {
        Object[] objects = AssetDatabase.LoadAllAssetsAtPath(fbxPath);
        if (objects != null && objects.Length > 0)
        {
            string targetPath = Application.dataPath + "/AnimationClip";
            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }
            foreach (Object obj in objects)
            {
                AnimationClip fbxClip = obj as AnimationClip;
                if (fbxClip != null)
                {
                    AnimationClip clip = new AnimationClip();
                    EditorUtility.CopySerialized(fbxClip, clip);
                    AssetDatabase.CreateAsset(clip, "Assets/AnimationClip/" + fbxClip.name + ".anim");
                }
                else
                {
                    Debug.Log("当前选择的文件不是带有AnimationClip的FBX文件");
                }
            }
        }
    }
}
