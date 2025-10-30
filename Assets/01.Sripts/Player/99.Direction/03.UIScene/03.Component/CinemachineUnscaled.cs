using System;
using System.Reflection;
using Unity.Cinemachine;
using UnityEngine;

[AddComponentMenu("Cinemachine/Cinemachine Sequencer Camera (Unscaled)")]
[DisallowMultipleComponent]
public class CinemachineSequencerCameraUnscaled : CinemachineSequencerCamera
{
    [Tooltip("If enabled, the sequencer will ignore Time.timeScale.")]
    public bool ignoreTimeScale = true;

    private float unscaledActivationTime = -1;
    private int unscaledCurrentInstruction = 0;

    protected override CinemachineVirtualCameraBase ChooseCurrentCamera(Vector3 worldUp, float deltaTime)
    {
        if (!ignoreTimeScale)
            return base.ChooseCurrentCamera(worldUp, deltaTime);

        // Unscaled deltaTime 처리
        AdvanceCurrentInstruction(Time.unscaledDeltaTime);
        if (Instructions == null || Instructions.Count == 0)
            return null;

        if (unscaledCurrentInstruction < 0 || unscaledCurrentInstruction >= Instructions.Count)
            return null;

        return Instructions[unscaledCurrentInstruction].Camera;
    }

    private void AdvanceCurrentInstruction(float deltaTime)
    {
        if (Instructions == null || Instructions.Count == 0)
        {
            unscaledActivationTime = -1;
            unscaledCurrentInstruction = -1;
            return;
        }

        float now = Time.unscaledTime;

        if (unscaledActivationTime < 0)
        {
            unscaledActivationTime = now;
            unscaledCurrentInstruction = 0;
        }

        if (unscaledCurrentInstruction > Instructions.Count - 1)
        {
            unscaledActivationTime = now;
            unscaledCurrentInstruction = Instructions.Count - 1;
        }

        var instr = Instructions[unscaledCurrentInstruction];
        float holdTime = instr.Hold + instr.Blend.BlendTime;
        float minHold = (unscaledCurrentInstruction < Instructions.Count - 1 || Loop) ? 0f : float.MaxValue;

        if (now - unscaledActivationTime > Mathf.Max(minHold, holdTime))
        {
            unscaledActivationTime = now;
            unscaledCurrentInstruction++;
            if (Loop && unscaledCurrentInstruction == Instructions.Count)
                unscaledCurrentInstruction = 0;
        }
    }
}