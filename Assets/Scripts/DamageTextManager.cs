﻿using TMPro;
using UnityEngine;

public class DamageTextManager : MonoBehaviourSingleton<DamageTextManager>
{
    public Transform player;
    public TextMeshProUGUI damageText;
    public Vector2 DamageTextVelocity;
    public bool CanSpawnText;

    private void Start()
    {
        player = GameManager.Player.transform;
    }

    public void SpawnText(Vector3 position, string text, float lifetime)
    {
        if (!CanSpawnText)
            return;

        var textMesh = Instantiate(damageText, position, Quaternion.identity, transform);
        var simpleGravity = textMesh.GetComponent<SimpleGravityBehaviour>();
        var dir = position - player.position;

        simpleGravity.Velocity = new Vector2((dir.x > 0 ? 1 : -1) * DamageTextVelocity.x, DamageTextVelocity.y);

        textMesh.text = text;
        textMesh.CrossFadeAlpha(0, lifetime, false);
        Destroy(textMesh.gameObject, lifetime);
    }
}
