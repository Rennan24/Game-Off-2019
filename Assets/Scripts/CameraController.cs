using System.Collections;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviourSingleton<CameraController>
{
    public float DecaySpeed;

    [SerializeField]
    private CinemachineBrain brain;

    [SerializeField]
    private CinemachineVirtualCamera vcam;
    private CinemachineBasicMultiChannelPerlin noise;

    private float curAmplitude;
    private float curFrequency;

    private void Start()
    {
        noise = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        noise.m_AmplitudeGain = 0;
        noise.m_FrequencyGain = 0;
    }

    private void Update()
    {
        curAmplitude -= curAmplitude * DecaySpeed * Time.deltaTime;
        curFrequency -= curFrequency * DecaySpeed * Time.deltaTime;

        noise.m_AmplitudeGain = curAmplitude;
        noise.m_FrequencyGain = curFrequency;
    }

    public void Shake(float amplitude, float frequency)
    {
        curAmplitude += amplitude;
        curFrequency += frequency;
    }
}
