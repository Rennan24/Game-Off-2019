using UnityEngine;

[CreateAssetMenu(menuName = "Reborn/PrefabCollection")]
public class PrefabCollectionAsset : ScriptableObject
{
    public GameObject[] Prefabs;

    public Vector2 minSize = Vector2.one;
    public Vector2 maxSize = Vector2.one;

    public float minRotation = 0;
    public float maxRotation = 360;

    public GameObject GetRandomPrefab()
    {
        var i = Random.Range(0, Prefabs.Length);
        return Prefabs[i];
    }

    public float GetRandomSize()
    {
        var x = Random.Range(minSize.x, maxSize.x);
        var y = Random.Range(minSize.y, maxSize.y);

        return (x + y) * 0.5f;
    }

    public Vector2 GetRandomSizeXY()
    {
        var x = Random.Range(minSize.x, maxSize.x);
        var y = Random.Range(minSize.y, maxSize.y);

        return new Vector2(x, y);
    }

    public float GetRandomRotation()
        => Random.Range(minRotation, maxRotation);

    public Texture GetPrefabImage()
    {
        var prefab = Prefabs[0];

        if (prefab == null)
            return null;

        if (prefab.TryGetComponent<SpriteRenderer>(out var spriteRenderer))
            return spriteRenderer.sprite.texture;

        if (prefab.TryGetComponent<Renderer>(out var renderer))
            return renderer.sharedMaterial.mainTexture;

        return null;
    }
}
