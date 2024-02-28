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
        [MenuItem("Assets/VMD Morph To Anim", priority = 50051)]
        internal static void CreateMorphAnimation()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            using (var stream = File.Open(path, FileMode.Open))
            {
                var animationClip = VMDMorphConverter.CreateAnimationClip(stream, getAnimFPS());
                string filepath = path.Replace(".vmd", "_morph.anim");
                AssetDatabase.CreateAsset(animationClip, $"{filepath}");
                AssetDatabase.Refresh();
                Debug.LogFormat("[{0}]:変換処理が正常に実行されました。", DateTime.Now);
                GC.Collect();
            }
        }

        [MenuItem("Assets/VMD Motion To Anim", priority = 50050)]
        internal static void ExportMotionVmdToAnim()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            MMD.MotionAgent motionAgent = new MMD.MotionAgent(path);
            //motionAgent.CreateAnimationClip();
        }


        [MenuItem("Assets/VMD Camera To Anim", priority = 50049)]
        internal static void ExportCameraVmdToAnim()
        {
            string selectPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (!string.IsNullOrEmpty(selectPath))
            {
                CameraVmdAgent camera_agent = new CameraVmdAgent(selectPath);
                camera_agent.CreateAnimationClip();
                Debug.LogFormat("[{0}]:カメラアニメーションの作成に成功しました。", DateTime.Now);
            }
            else Debug.LogError("ファイルまたはフォルダを選択してください");
        }

        [MenuItem("MMD6UnityTool/(beta) use 60fps")]
        private static void Change60fpsState()
        {
            string menuPath = "MMD6UnityTool/(beta) use 60fps";
            bool isChecked = Menu.GetChecked("MMD6UnityTool/(beta) use 60fps");
            Menu.SetChecked(menuPath, !isChecked);
        }

        [MenuItem("Assets/VMD Morph To Anim", validate = true)]
        [MenuItem("Assets/VMD Camera To Anim", validate = true)]
        private static bool ShowLogValidation()
        {
            if (Application.isPlaying) return false;
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
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


        //Utils
        public static int getAnimFPS()
        {
            return Menu.GetChecked("MMD6UnityTool/(beta) use 60fps") ? 60 : 30;
        }

        private static bool isExtension(string path, string extension)
        {
            return Path.GetExtension(path).ToUpper().Contains(extension.ToUpper());
        }
    }
}