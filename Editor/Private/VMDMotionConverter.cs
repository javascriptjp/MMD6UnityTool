public class VMDMotionConverter
{
    public enum BoneNames
    {
        全ての親, センター, グルーブ, 左足ＩＫ, 左つま先ＩＫ, 右足ＩＫ, 右つま先ＩＫ, 下半身, 上半身, 上半身2,
        首, 頭, 左目, 右目, 両目, 左肩, 左腕, 左腕捩れ, 左ひじ, 左手首,
        右肩, 右腕, 右腕捩れ, 右ひじ, 右手首, 左足, 左ひざ, 左足首, 左つま先, 右足,
        右ひざ, 右足首, 右つま先, 左親指１, 左親指２, 左人指１, 左人指２, 左人指３, 左中指１, 左中指２,
        左中指３, 左薬指１, 左薬指２, 左薬指３, 左小指１, 左小指２, 左小指３, 右親指１, 右親指２, 右人指１,
        右人指２, 右人指３, 右中指１, 右中指２, 右中指３, 右薬指１, 右薬指２, 右薬指３, 右小指１, 右小指２,
        右小指３, None
    }

    //private static Dictionary<string, string> MotionAnimationNames = new Dictionary<string, string>()
    //    { BoneNames.センター, HumanBodyBones.Hips},
    //    { BoneNames.全ての親, HumanBodyBones.Hips},
    //    { BoneNames.上半身,   HumanBodyBones.Spine},
    //    { BoneNames.上半身2,  HumanBodyBones.Chest},
    //    { BoneNames.頭,       HumanBodyBones.Head},
    //    { BoneNames.首,       HumanBodyBones.Neck},
    //    { BoneNames.左肩,     HumanBodyBones.LeftShoulder},
    //    { BoneNames.右肩,     HumanBodyBones.RightShoulder},
    //    { BoneNames.左腕,     HumanBodyBones.LeftUpperArm},
    //    { BoneNames.右腕,     HumanBodyBones.RightUpperArm},
    //    { BoneNames.左ひじ,   HumanBodyBones.LeftLowerArm},
    //    { BoneNames.右ひじ,   HumanBodyBones.RightLowerArm},
    //    { BoneNames.左手首,   HumanBodyBones.LeftHand},
    //    { BoneNames.右手首,   HumanBodyBones.RightHand},
    //    { BoneNames.左親指１, HumanBodyBones.LeftThumbProximal},
    //    { BoneNames.右親指１, HumanBodyBones.RightThumbProximal},
    //    { BoneNames.左親指２, HumanBodyBones.LeftThumbIntermediate},
    //    { BoneNames.右親指２, HumanBodyBones.RightThumbIntermediate},
    //    { BoneNames.左人指１, HumanBodyBones.LeftIndexProximal},
    //    { BoneNames.右人指１, HumanBodyBones.RightIndexProximal},
    //    { BoneNames.左人指２, HumanBodyBones.LeftIndexIntermediate},
    //    { BoneNames.右人指２, HumanBodyBones.RightIndexIntermediate},
    //    { BoneNames.左人指３, HumanBodyBones.LeftIndexDistal},
    //    { BoneNames.右人指３, HumanBodyBones.RightIndexDistal},
    //    { BoneNames.左中指１, HumanBodyBones.LeftMiddleProximal},
    //    { BoneNames.右中指１, HumanBodyBones.RightMiddleProximal},
    //    { BoneNames.左中指２, HumanBodyBones.LeftMiddleIntermediate},
    //    { BoneNames.右中指２, HumanBodyBones.RightMiddleIntermediate},
    //    { BoneNames.左中指３, HumanBodyBones.LeftMiddleDistal},
    //    { BoneNames.右中指３, HumanBodyBones.RightMiddleDistal},
    //    { BoneNames.左薬指１, HumanBodyBones.LeftRingProximal},
    //    { BoneNames.右薬指１, HumanBodyBones.RightRingProximal},
    //    { BoneNames.左薬指２, HumanBodyBones.LeftRingIntermediate},
    //    { BoneNames.右薬指２, HumanBodyBones.RightRingIntermediate},
    //    { BoneNames.左薬指３, HumanBodyBones.LeftRingDistal},
    //    { BoneNames.右薬指３, HumanBodyBones.RightRingDistal},
    //    { BoneNames.左小指１, HumanBodyBones.LeftLittleProximal},
    //    { BoneNames.右小指１, HumanBodyBones.RightLittleProximal},
    //    { BoneNames.左小指２, HumanBodyBones.LeftLittleIntermediate},
    //    { BoneNames.右小指２, HumanBodyBones.RightLittleIntermediate},
    //    { BoneNames.左小指３, HumanBodyBones.LeftLittleDistal},
    //    { BoneNames.右小指３, HumanBodyBones.RightLittleDistal},
    //    { BoneNames.左足ＩＫ, HumanBodyBones.LeftFoot},
    //    { BoneNames.右足ＩＫ, HumanBodyBones.RightFoot},
    //    { BoneNames.左足,     HumanBodyBones.LeftUpperLeg},
    //    { BoneNames.右足,     HumanBodyBones.RightUpperLeg},
    //    { BoneNames.左ひざ,   HumanBodyBones.LeftLowerLeg},
    //    { BoneNames.右ひざ,   HumanBodyBones.RightLowerLeg},
    //    { BoneNames.左足首,   HumanBodyBones.LeftFoot},
    //    { BoneNames.右足首,   HumanBodyBones.RightFoot}
    //};

}