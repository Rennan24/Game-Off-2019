using UnityEngine;

[CreateAssetMenu(menuName = "Reborn/PrefabCollection")]
public class PrefabCollectionAsset : ScriptableObject
{
    public GameObject[] Prefabs;

    public GameObject GetRandomPrefab()
    {
        var i = Random.Range(0, Prefabs.Length);
        return Prefabs[i];
    }

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
