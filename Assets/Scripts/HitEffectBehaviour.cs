using UnityEngine;

public class HitEffectBehaviour : MonoBehaviour
{
    public float HitEffectSpeed = 4f;

    [SerializeField]
    private HealthBehaviour health;

    [SerializeField]
    private SpriteRenderer renderer;
    private MaterialPropertyBlock block;

    private float hitAmount;

    private static readonly int hitTime = Shader.PropertyToID("_HitTime");

    private void Reset()
    {
        health = GetComponent<HealthBehaviour>();
        renderer = GetComponent<SpriteRenderer>();
    }

    private void Awake()
    {
        block = new MaterialPropertyBlock();
    }

    private void Start()
    {
        health.Damaged += Hit;
    }

    private void Update()
    {
        renderer.GetPropertyBlock(block);
        block.SetFloat(hitTime, hitAmount);
        renderer.SetPropertyBlock(block);

        var decrement = HitEffectSpeed * Time.deltaTime;
        hitAmount = Mathf.Max(hitAmount - decrement, 0);
    }

    private void Hit(Vector3 hitPos, Vector2 hitDir, int amount, int curHealth)
    {
        Vector3 offset = new Vector3(0, 1);
        Vector3 random = Random.insideUnitCircle * 0.5f;
        var newPos = transform.position + offset + random;

        DamageTextManager.Inst.SpawnText(newPos, 1.0f);
        hitAmount = 1.0f;
    }
}
