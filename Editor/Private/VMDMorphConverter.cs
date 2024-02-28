using System;
using MMDExtensions.Tools;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
public class VMDMorphConverter
{
    public static AnimationClip CreateAnimationClip(FileStream stream, int fps)
    {
        var vmd = VMDParser.ParseVMD(stream);
        var animationClip = new AnimationClip() { frameRate = fps };
        var keyframes = vmd.Morphs.GroupBy(k => k.MorphName).Select(g => new
        {
            Name = g.Key,
            Keyframes = g.Select(v => new Keyframe(v.FrameIndex * (1 / animationClip.frameRate), v.Weight * 100))
        });

        foreach (var package in keyframes)
        {
            var name = package.Name;
            var curve = new AnimationCurve(package.Keyframes.ToArray());
            if (MorphAnimations.Contains(name))
            {
                if (name == MorphAnimationNames.Blink || name == MorphAnimationNames.Smile)
                {
                    animationClip.SetCurve("Body", typeof(SkinnedMeshRenderer), $"blendShape.{name}", curve);
                }
                if (
                    name == MorphAnimationNames.Wink2 ||
                    name == MorphAnimationNames.Wink2Right ||
                    name == MorphAnimationNames.Wink ||
                    name == MorphAnimationNames.WinkRight)
                {
                    var floatCurve = EditorCurveBinding.FloatCurve("Body", typeof(SkinnedMeshRenderer), $"blendShape.{name}");
                    var existingCurve = AnimationUtility.GetEditorCurve(animationClip, floatCurve);
                    if (existingCurve != null)
                    {
                        var existingKeyframes = existingCurve.keys.ToList();
                        existingKeyframes.AddRange(curve.keys);
                        curve = new AnimationCurve(existingKeyframes.ToArray());
                    }
                }
                animationClip.SetCurve("Body", typeof(SkinnedMeshRenderer), $"blendShape.{name}", curve);
            }
        }

        return animationClip;
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
}