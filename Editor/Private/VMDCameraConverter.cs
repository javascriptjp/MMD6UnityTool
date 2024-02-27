using System;
using System.Collections;
using System.Collections.Generic;
using MMD.VMD;
using UnityEditor;
using UnityEngine;

public class VMDCameraConverter
{
	public static AnimationClip CreateAnimationClip(VMDFormat format)
	{
		VMDCameraConverter converter = new VMDCameraConverter();
		return converter.CreateAnimationClip_(format);
	}

	private AnimationClip CreateAnimationClip_(VMDFormat format)
	{

		AnimationClip clip = new AnimationClip() { frameRate = Menu.GetChecked("MMD6UnityTool/(beta) use 60fps") ? 60 : 30 };
		clip.name = format.name;

		CreateKeysForCamera(format, clip);

		return clip;
	}

	void CreateKeysForCamera(VMDFormat format, AnimationClip clip)
	{
		const float tick_time = 1f / 30f;
		const float mmd4unity_unit = 0.08f;

		Keyframe[] posX_keyframes = new Keyframe[format.camera_list.camera_count];
		Keyframe[] posY_keyframes = new Keyframe[format.camera_list.camera_count];
		Keyframe[] posZ_keyframes = new Keyframe[format.camera_list.camera_count];

		Keyframe[] rotX_keyframes = new Keyframe[format.camera_list.camera_count];
		Keyframe[] rotY_keyframes = new Keyframe[format.camera_list.camera_count];
		Keyframe[] rotZ_keyframes = new Keyframe[format.camera_list.camera_count];
		Keyframe[] rotW_keyframes = new Keyframe[format.camera_list.camera_count];

		Keyframe[] fov_keyframes = new Keyframe[format.camera_list.camera_count];

		Keyframe[] dis_keyframes = new Keyframe[format.camera_list.camera_count];
		for (int i = 0; i < format.camera_list.camera_count; i++)
		{
			VMDFormat.CameraData cameraData = format.camera_list.camera[i];
			Quaternion Quaternion = Quaternion.Euler(new Vector3(
									cameraData.rotation.x * Mathf.Rad2Deg,
									-cameraData.rotation.y * Mathf.Rad2Deg,
									cameraData.rotation.z * Mathf.Rad2Deg));
			float frameTime = cameraData.frame_no * tick_time;
			posX_keyframes[i] = new Keyframe(frameTime, -cameraData.location.x * mmd4unity_unit);
			posY_keyframes[i] = new Keyframe(frameTime, cameraData.location.y * mmd4unity_unit);
			posZ_keyframes[i] = new Keyframe(frameTime, -cameraData.location.z * mmd4unity_unit);

			rotX_keyframes[i] = new Keyframe(frameTime, Quaternion.x);
			rotY_keyframes[i] = new Keyframe(frameTime, Quaternion.y);
			rotZ_keyframes[i] = new Keyframe(frameTime, Quaternion.z);
			rotW_keyframes[i] = new Keyframe(frameTime, Quaternion.w);

			fov_keyframes[i] = new Keyframe(frameTime, cameraData.viewing_angle);

			dis_keyframes[i] = new Keyframe(frameTime, -cameraData.length * mmd4unity_unit);
		}

		AnimationCurve posX_curve = ToAnimationCurveWithTangentMode(1, AnimationUtility.TangentMode.Free, posX_keyframes, format.camera_list);
		AnimationCurve posY_curve = ToAnimationCurveWithTangentMode(2, AnimationUtility.TangentMode.Free, posY_keyframes, format.camera_list);
		AnimationCurve posZ_curve = ToAnimationCurveWithTangentMode(3, AnimationUtility.TangentMode.Free, posZ_keyframes, format.camera_list);
		AnimationCurve rotX_curve = ToAnimationCurveWithTangentMode(4, AnimationUtility.TangentMode.Free, rotX_keyframes, format.camera_list);
		AnimationCurve rotY_curve = ToAnimationCurveWithTangentMode(4, AnimationUtility.TangentMode.Free, rotY_keyframes, format.camera_list);
		AnimationCurve rotZ_curve = ToAnimationCurveWithTangentMode(4, AnimationUtility.TangentMode.Free, rotZ_keyframes, format.camera_list);
		AnimationCurve rotW_curve = ToAnimationCurveWithTangentMode(4, AnimationUtility.TangentMode.Free, rotW_keyframes, format.camera_list);
		AnimationCurve dis_curve = ToAnimationCurveWithTangentMode(5, AnimationUtility.TangentMode.Free, dis_keyframes, format.camera_list);
		AnimationCurve fov_curve = ToAnimationCurveWithTangentMode(6, AnimationUtility.TangentMode.Free, fov_keyframes, format.camera_list);

		clip.SetCurve("", typeof(Transform), "m_LocalPosition.x", posX_curve);
		clip.SetCurve("", typeof(Transform), "m_LocalPosition.y", posY_curve);
		clip.SetCurve("", typeof(Transform), "m_LocalPosition.z", posZ_curve);

		clip.SetCurve("", typeof(Transform), "m_LocalRotation.x", rotX_curve);
		clip.SetCurve("", typeof(Transform), "m_LocalRotation.y", rotY_curve);
		clip.SetCurve("", typeof(Transform), "m_LocalRotation.z", rotZ_curve);
		clip.SetCurve("", typeof(Transform), "m_LocalRotation.w", rotW_curve);

		clip.SetCurve("Distance", typeof(Transform), "m_LocalPosition.z", dis_curve);
		clip.SetCurve("Distance/Camera", typeof(Camera), "field of view", fov_curve);
	}

	Tuple<Vector2, Vector2> GetInterpolationPoints(byte[] interpolation, int type)
	{
		int row = (type - 1) * 4;
		Vector2 p1 = new Vector2(interpolation[row + 0], interpolation[row + 2]);
		Vector2 p2 = new Vector2(interpolation[row + 1], interpolation[row + 3]);

		return new Tuple<Vector2, Vector2>(p1, p2);
	}
	Tuple<Vector2, Vector2> ConvertToFramekeyControllerPoint(Vector2 p1, Vector2 p2, Keyframe outKeyframe, Keyframe inKeyframe)
	{
		var dX = inKeyframe.time - outKeyframe.time;
		var dY = inKeyframe.value - outKeyframe.value;

		var newP1 = new Vector2(outKeyframe.time + p1.x / 127 * dX, outKeyframe.value + p1.y / 127 * dY);
		var newP2 = new Vector2(outKeyframe.time + p2.x / 127 * dX, outKeyframe.value + p2.y / 127 * dY);

		if (Mathf.Approximately(outKeyframe.time, newP1.x)) newP1.x += 0.1f;
		if (Mathf.Approximately(inKeyframe.time, newP2.x)) newP2.x -= 0.1f;

		return new Tuple<Vector2, Vector2>(newP1, newP2);
	}

	private static float Tangent(in Vector2 from, in Vector2 to)
	{
		Vector2 vec = to - from;
		return vec.y / vec.x;
	}

	private static float Weight(in Vector2 from, in Vector2 to, float length)
	{
		return (to.x - from.x) / length;
	}

	float[] CalculateBezierCoefficient(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
	{
		float p30Length = (p3.x - p0.x);

		float outTangent = Tangent(p0, p1);
		float outWeight = Weight(p0, p1, p30Length);

		float inTangent = Tangent(p2, p3);
		float inWeight = Weight(p2, p3, p30Length);

		return new float[] { outTangent, outWeight, inTangent, inWeight };
	}

	void SetKeyfreamTweenCurve(int index, int type, Keyframe[] keyframes, VMDFormat.CameraList cameraList)
	{
		if (index <= 0)
			return;

		VMDFormat.CameraData curCameraData = cameraList.camera[index];
		VMDFormat.CameraData lastCameraData = cameraList.camera[index - 1];

		Keyframe outKeyframe = keyframes[index - 1];
		Keyframe inKeyframe = keyframes[index];

		var dX = inKeyframe.time - outKeyframe.time;
		var dY = inKeyframe.value - outKeyframe.value;

		outKeyframe.weightedMode = WeightedMode.Both;
		inKeyframe.weightedMode = WeightedMode.Both;
		if (Mathf.Approximately(dY, 0f) || Mathf.Approximately(dX, 1 / 30f))
		{
			outKeyframe.outTangent = 0;
			outKeyframe.outWeight = 0;

			inKeyframe.inTangent = 0;
			inKeyframe.inWeight = 0;
		}
		else
		{
			Vector2 p0 = new Vector2(outKeyframe.time, outKeyframe.value);
			Vector2 p3 = new Vector2(inKeyframe.time, inKeyframe.value);
			Vector2 p1 = Vector2.zero;
			Vector2 p2 = Vector2.zero;
			var intTuple = GetInterpolationPoints(curCameraData.interpolation, type);
			var ptTuple = ConvertToFramekeyControllerPoint(intTuple.Item1, intTuple.Item2, outKeyframe, inKeyframe);
			p1 = ptTuple.Item1;
			p2 = ptTuple.Item2;

			float[] coeffs = CalculateBezierCoefficient(p0, p1, p2, p3);
			outKeyframe.outTangent = coeffs[0];
			outKeyframe.outWeight = coeffs[1];
			inKeyframe.inTangent = coeffs[2];
			inKeyframe.inWeight = coeffs[3];
		}
		keyframes[index - 1] = outKeyframe;
		keyframes[index] = inKeyframe;
	}

	AnimationCurve ToAnimationCurveWithTangentMode(int type, AnimationUtility.TangentMode mode, Keyframe[] keyframes, VMDFormat.CameraList cameraList)
	{
		if (mode == AnimationUtility.TangentMode.Free)
		{
			for (int i = 0; i < keyframes.Length; i++)
			{
				SetKeyfreamTweenCurve(i, type, keyframes, cameraList);
			}
		}

		var newKeyFrames = OptimizedCurves(type, keyframes, cameraList);

		AnimationCurve curve = new AnimationCurve(newKeyFrames);
		for (int i = 0; i < curve.keys.Length; i++)
		{
			if (mode == AnimationUtility.TangentMode.Free)
				AnimationUtility.SetKeyBroken(curve, i, true);

			AnimationUtility.SetKeyLeftTangentMode(curve, i, mode);
			AnimationUtility.SetKeyRightTangentMode(curve, i, mode);

		}
		for (int j = 0; j < keyframes.Length; j++)
		{
			if (j < keyframes.Length - 1)
			{
				int StartFrame = (int)(keyframes[j].time * 30f);
				int EndFrame = (int)(keyframes[j + 1].time * 30f);

				if (EndFrame == StartFrame + 1)
				{
					AnimationUtility.SetKeyRightTangentMode(curve, j, AnimationUtility.TangentMode.Constant);
				}
			}
		}
		return curve;
	}

	Vector2 EvaluteThire(float t, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
	{
		return Mathf.Pow(1 - t, 3) * p0 + 3 * Mathf.Pow(1 - t, 2) * t * p1 + 3 * (1 - t) * Mathf.Pow(t, 2) * p2 + Mathf.Pow(t, 3) * p3;
	}

	List<Keyframe> CalculateInterpolationKeyframe(int type, byte[] interpolation, Keyframe outKeyframe, Keyframe inKeyframe, float density = 0.05f)
	{
		density = Mathf.Min(1f, density);
		density = Mathf.Max(0f, density);
		var p0 = new Vector2(outKeyframe.time, outKeyframe.value);
		var p3 = new Vector2(inKeyframe.time, inKeyframe.value);
		var intTuple = GetInterpolationPoints(interpolation, type);
		var ptTuple = ConvertToFramekeyControllerPoint(intTuple.Item1, intTuple.Item2, outKeyframe, inKeyframe);
		var p1 = ptTuple.Item1;
		var p2 = ptTuple.Item2;

		var dX = inKeyframe.time - outKeyframe.time;
		var dY = inKeyframe.value - outKeyframe.value;
		float stepTime = (1 / 30f) / density;
		List<Keyframe> interpolationKeyframes = new List<Keyframe>();
		if (!Mathf.Approximately(dY, 0f))
		{
			if (stepTime > 0f && stepTime >= (1 / 30f))
			{
				for (float time = outKeyframe.time + stepTime; time < inKeyframe.time; time += stepTime)
				{
					float t = (time - outKeyframe.time) / dX;
					var pt = EvaluteThire(t, p0, p1, p2, p3);
					var newKeyframe = new Keyframe(pt.x, pt.y);
					interpolationKeyframes.Add(newKeyframe);
				}
			}

		}

		return interpolationKeyframes;
	}

	Keyframe[] OptimizedCurves(int type, Keyframe[] keyframes, VMDFormat.CameraList cameraList)
	{
		List<Keyframe> framesList = new List<Keyframe>();
		for (int i = 0; i < keyframes.Length; i++)
		{
			var keyframe = keyframes[i];
			framesList.Add(keyframe);
		}

		if (framesList.Count == 1)
		{
			Keyframe[] newKeyframes = new Keyframe[2];
			newKeyframes[0] = keyframes[0];
			newKeyframes[1] = keyframes[0];
			newKeyframes[1].time += 0.001f / 60f;
			newKeyframes[0].outTangent = 0f;
			newKeyframes[1].inTangent = 0f;

			framesList.Clear();
			framesList.AddRange(newKeyframes);
		}
		return framesList.ToArray();
	}
}
