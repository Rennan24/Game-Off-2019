using Spine.Unity;
using UnityEngine;

public class PlayerWeapon: MonoBehaviour
{
    public float ShakeAmplitude = 1f;
    public float ShakeFrequency = 2f;

    public PlayerWeaponType WeaponType;
    public DelayTimer AttackDelay;
    public int ProjectileAmount = 1;
    public ProjectileController Controller;
    public bool RandomizeAngle;

    public int MaxAmmoCount = 999;
    public int AmmoCount;

    [Header("Sounds:")]
    [SerializeField]
    private SimpleAudioEvent swapSFX;
    [SerializeField]
    private SimpleAudioEvent attackSFX;
    [SerializeField]
    private SimpleAudioEvent cantUseSFX;
    [SerializeField]
    private SimpleAudioEvent pickupSFX;

    [Header("Spine Anims:")]
    public AnimationReferenceAsset IdleAnim;
    public AnimationReferenceAsset RunAnim;
    public AnimationReferenceAsset AttackAnim;

    [Header("References:")]
    public ParticleSystem MuzzleFlashRef;
    public ParticleSystem BulletCasingRightFX;
    public Sprite OutlineSprite;

    [HideInInspector]
    public ParticleSystem BulletCasingLeftFX;

    [SerializeField]
    private AudioSource audioSource;

    public void Start()
    {
        // Spawn a casing effect for the left side by flipping the X
        BulletCasingLeftFX = Instantiate(BulletCasingRightFX, transform);
        BulletCasingLeftFX.transform.FlipX();
    }

    public bool Attack(Vector3 pos, Vector3 dir, float angle)
    {
        if (!AttackDelay.AutoReady())
            return false;
        if (AmmoCount <= 0)
        {
            cantUseSFX.Play(audioSource);
            return false;
        }

        CameraController.Inst.Shake(ShakeAmplitude, ShakeFrequency);
        Controller.SpawnProjectiles(pos, angle, ProjectileAmount, RandomizeAngle);
        AmmoCount--;

        MuzzleFlashRef.transform.SetPositionAndRotation(pos, Quaternion.Euler(0, 0, angle));
        MuzzleFlashRef.Play();

        if (dir.x > 0)
            BulletCasingLeftFX.Play();
        else
            BulletCasingRightFX.Play();

        attackSFX.PlayOneShot(audioSource);
        return true;
    }

    public void SwappedTo()
    {
        swapSFX.Play(audioSource);
    }
}

public enum PlayerWeaponType { Axe, Pistol, Uzi, Shotgun, Bazooka }
