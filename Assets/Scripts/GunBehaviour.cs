using Cinemachine;
using UnityEngine;

public class GunBehaviour : MonoBehaviour
{
    public float ShakeAmplitude = 1f;
    public float ShakeFrequency = 2f;
    public float ShakeTime = 0.25f;

    [SerializeField] private VelocityBehaviour Projectile;
    [SerializeField] private float FireDelay;
    [SerializeField] private float FireSpeed;
    [SerializeField] private ParticleSystem MuzzleFlashRef;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] gunsounds;

    private float fireTimer;

    public void Fire(Vector3 dir, Quaternion rot)
    {
        if (fireTimer > 0f)
            return;

        MuzzleFlashRef.Play();
        var num = Random.Range(0, gunsounds.Length);
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.PlayOneShot(gunsounds[num]);

        CameraController.Inst.Shake(ShakeAmplitude, ShakeFrequency);
        var projectileVel = Instantiate(Projectile, transform.position, rot);
        projectileVel.Value = transform.right * FireSpeed;
        fireTimer = FireDelay;
    }

    private void Update()
    {
        fireTimer -= Time.deltaTime;
    }
}
