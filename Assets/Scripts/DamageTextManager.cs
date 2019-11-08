using System.Collections;
using TMPro;
using UnityEngine;

public class DamageTextManager : MonoBehaviourSingleton<DamageTextManager>
{
    public TextMeshProUGUI damageText;

    public void SpawnText(Vector3 position, float lifetime)
    {
        var text = Instantiate(damageText, position, Quaternion.identity, transform);
        text.CrossFadeAlpha(0, lifetime, false);
        StartCoroutine(DestroyText(text.gameObject, lifetime));
    }

    private IEnumerator DestroyText(GameObject text, float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(text);
    }
}
