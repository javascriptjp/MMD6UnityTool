using System;
using MMDExtensions.Tools;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace MMD6UnityTool
{
    internal class AnimationHelpers
    {
        [MenuItem("MMD6UnityTool/(beta) use 60fps")]
        private static void Change60fpsState()
        {
            string menuPath = "MMD6UnityTool/(beta) use 60fps";
            bool isChecked = Menu.GetChecked("MMD6UnityTool/(beta) use 60fps");
            Menu.SetChecked(menuPath, !isChecked);
        }

        [MenuItem("Assets/VMD Morph To Anim", priority = 50051)]
        internal static void CreateMorphAnimation()
        {
            string path = AssetDatabase.GetAssetPath(Selection.GetFiltered<DefaultAsset>(SelectionMode.Assets).FirstOrDefault());
            using (var stream = File.Open(path, FileMode.Open))
            {
                var vmd = VMDParser.ParseVMD(stream);
                var animationClip = new AnimationClip() { frameRate = Menu.GetChecked("MMD6UnityTool/(beta) use 60fps") ? 60 : 30 };
                var keyframes = vmd.Morphs.GroupBy(k => k.MorphName).Select(g => new
                {
                    Name = g.Key,
                    Keyframes = g.Select(v => new Keyframe(v.FrameIndex * (1 / animationClip.frameRate), v.Weight * 100))
                });
                foreach (var package in keyframes)
                {
                    var name = package.Name;
                    var curve = new AnimationCurve(package.Keyframes.ToArray());
                    if (name == MorphAnimationNames.Blink || name == MorphAnimationNames.Smile) AddCurveToAnimationClip(animationClip, name, curve);
                    if (name == MorphAnimationNames.Wink2 || name == MorphAnimationNames.Wink2Right || name == MorphAnimationNames.Wink || name == MorphAnimationNames.WinkRight)
                    {
                        var existingCurve = AnimationUtility.GetEditorCurve(animationClip, EditorCurveBinding.FloatCurve("Body", typeof(SkinnedMeshRenderer), $"blendShape.{name}"));
                        if (existingCurve != null) curve = MergeAnimationCurves(existingCurve, curve);
                    }
                    AddCurveToAnimationClip(animationClip, name, curve);
                }
                string filepath = path.Replace(".vmd", "_morph.anim");
                AssetDatabase.CreateAsset(animationClip, $"{filepath}");
                AssetDatabase.Refresh();
                Debug.LogFormat("[{0}]:変換処理が正常に実行されました。", DateTime.Now);
                GC.Collect();
            }
        }

        [MenuItem("Assets/VMD Camera To Anim", priority = 50050)]
        internal static void ExportCameraVmdToAnim()
        {
            var selected = Selection.activeObject;
            string selectPath = AssetDatabase.GetAssetPath(selected);
            if (!string.IsNullOrEmpty(selectPath))
            {
                CameraVmdAgent camera_agent = new CameraVmdAgent(selectPath);
                camera_agent.CreateAnimationClip();
                Debug.LogFormat("[{0}]:カメラアニメーションの作成に成功しました。", DateTime.Now);
            }
            else Debug.LogError("ファイルまたはフォルダを選択してください");
        }

        [MenuItem("Assets/VMD Morph To Anim", validate = true)]
        [MenuItem("Assets/VMD Camera To Anim", validate = true)]
        private static bool ShowLogValidation()
        {
            if (Application.isPlaying) return false;
            string path = AssetDatabase.GetAssetPath(Selection.GetFiltered<DefaultAsset>(SelectionMode.Assets).FirstOrDefault());
            if (!isExtension(path, "vmd")) return false;
            return true;
        }

        [MenuItem("GameObject/MMD/Generate MMDCamera")]
        internal static void GenerateMMDCamera()
        {
            var mmdCamera = new GameObject("MMDCamera");
            var distance = new GameObject("Distance");
            var camera = new GameObject("Camera");
            distance.transform.parent = mmdCamera.transform;
            camera.transform.parent = distance.transform;
            distance.transform.rotation = Quaternion.Euler(0, 180, 0);
            mmdCamera.AddComponent<Animator>();
            camera.AddComponent<Camera>();
            camera.AddComponent<AudioListener>();
        }

        #region Utils

        private static bool isExtension(string path, string extension)
        {
            return Path.GetExtension(path).ToUpper().Contains(extension.ToUpper());
        }

        private static void AddCurveToAnimationClip(AnimationClip animationClip, string registerName, AnimationCurve curve)
        {
            if (MorphAnimations.Contains(registerName))
                animationClip.SetCurve("Body", typeof(SkinnedMeshRenderer), $"blendShape.{registerName}", curve);
        }
        private static AnimationCurve MergeAnimationCurves(AnimationCurve existingCurve, AnimationCurve newCurve)
        {
            var existingKeyframes = existingCurve.keys.ToList();
            existingKeyframes.AddRange(newCurve.keys);
            return new AnimationCurve(existingKeyframes.ToArray());
        }

        private static class MorphAnimationNames
        {
            public const string Blink = "まばたき";
            public const string Wink2 = "ウィンク２";
            public const string Wink2Right = "ｳｨﾝｸ２右";
            public const string Smile = "笑い";
            public const string Wink = "ウィンク";
            public const string WinkRight = "ウィンク右";
        }

        private static string[] MorphAnimations = new string[] {
            "真面目",
            "困る",
            "にこり",
            "怒り",
            "上",
            "下",
            "まばたき",
            "笑い",
            "ウィンク",
            "ウィンク２",
            "ウィンク右",
            "ｳｨﾝｸ２右",
            "はぅ",
            "なごみ",
            "びっくり",
            "じと目",
            "なぬ！",
            "あ",
            "い",
            "う",
            "お",
            "▲",
            "∧",
            "ω",
            "ω□",
            "はんっ！",
            "えー",
            "にやり",
            "瞳小",
            "ぺろっ",
            "白目",
            "瞳大",
            "え",
            "口角上げ",
            "口角下げ",
            "頬染め",
            "青ざめ"
        };

        #endregion
    }
}