using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Audio Events/Simple")]
public class SimpleAudioEvent : ScriptableObject, IAudioEvent
{
    public AudioClip[] clips;

    public MinMaxFloat Volume = new MinMaxFloat(1, 1);

    [MinMaxSlider(0, 2)]
    public MinMaxFloat Pitch = new MinMaxFloat(1, 1);

    public void Play(AudioSource source)
    {
        if (clips.Length == 0) return;

        source.clip = clips[Random.Range(0, clips.Length)];
        source.volume = Volume.RandomRange;
        source.pitch = Pitch.RandomRange;
        source.Play();
    }

    public void PlayOneShot(AudioSource source)
    {
        if (clips.Length == 0) return;

        var clip = clips[Random.Range(0, clips.Length)];
        source.volume = Volume.RandomRange;
        source.pitch = Pitch.RandomRange;
        source.PlayOneShot(clip);
    }

    public void PlayAtPoint(AudioSource source, Vector3 position)
    {
        if (clips.Length == 0) return;

        var clip = clips[Random.Range(0, clips.Length)];
        source.volume = Volume.RandomRange;
        source.pitch = Pitch.RandomRange;
        AudioSource.PlayClipAtPoint(clip, position);
    }

}

public interface IAudioEvent
{
    void Play(AudioSource source);
}

#if UNITY_EDITOR
[CustomEditor(typeof(SimpleAudioEvent), true)]
public class AudioEventEditor : Editor
{
    [SerializeField]
    private AudioSource source;

    public void OnEnable()
    {
        source = EditorUtility.CreateGameObjectWithHideFlags("Audio preview", HideFlags.HideAndDontSave, typeof(AudioSource)).GetComponent<AudioSource>();
    }

    public void OnDisable()
    {
        DestroyImmediate(source.gameObject);
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUI.BeginDisabledGroup(serializedObject.isEditingMultipleObjects);
        if (GUILayout.Button("Preview"))
        {
            ((SimpleAudioEvent) target).Play(source);
        }
        EditorGUI.EndDisabledGroup();
    }
}
#endif
