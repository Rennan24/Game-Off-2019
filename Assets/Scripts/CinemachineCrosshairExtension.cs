using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CinemachineCrosshairExtension : CinemachineExtension
{
    public Transform Crosshair;

    [Range(0, 1)]
    public float Blend = 0.1f;

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (stage != CinemachineCore.Stage.Aim)
            return;

        var pos = vcam.Follow.position;
        var newOffset = Vector3.Lerp(pos, Crosshair.position, Blend);

        state.PositionCorrection += newOffset - pos;
    }
}