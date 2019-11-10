using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBehaviour : MonoBehaviour
{
    [SerializeField] private VelocityBehaviour Projectile;
    [SerializeField] private Vector2 FireOffset;
    [SerializeField] private float FireDelay;
    [SerializeField] private float FireSpeed;
    [SerializeField] private ParticleSystem MuzzleFlashRef;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] gunsounds;

    private float fireTimer;

    public void Fire(Vector3 dir)
    {
        if (fireTimer > 0f)
            return;

        // Seems to not be necessary depending on the particle systems
        // Render Alignment set to 'Local' towards the bottom...
//        var mainModule = MuzzleFlashRef.main;
//        mainModule.startRotationZ = (transform.eulerAngles.z - 90) * Mathf.Deg2Rad;

        MuzzleFlashRef.Play();
        var num = Random.Range(0, gunsounds.Length);
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.PlayOneShot(gunsounds[num]);


        CameraFollower.Inst.Shake(new Vector3(0.2f, 0.2f), 0.05f);
        var projectileVel = Instantiate(Projectile, transform.position, transform.rotation);
        projectileVel.Value = dir * FireSpeed;
        fireTimer = FireDelay;
    }

    private void Update()
    {
        fireTimer -= Time.deltaTime;
    }
}
