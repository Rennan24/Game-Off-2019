using TMPro;
using UnityEngine;

public class DamageTextManager : MonoBehaviourSingleton<DamageTextManager>
{
    public TextMeshProUGUI damageText;
    public Vector2 DamageTextVelocity;

    public void SpawnText(Vector3 position, float lifetime)
    {
        var text = Instantiate(damageText, position, Quaternion.identity, transform);
        var simpleGravity = text.GetComponent<SimpleGravityBehaviour>();
        simpleGravity.Velocity = new Vector2(Random.Range(-1, 1) * DamageTextVelocity.x, DamageTextVelocity.y);
        
        text.CrossFadeAlpha(0, lifetime, false);
        Destroy(text.gameObject, lifetime);
    }
}
