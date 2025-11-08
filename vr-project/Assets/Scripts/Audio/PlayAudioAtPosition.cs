using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

/// <summary>
/// Spawns a temporary GameObject with an AudioSource at a chosen world position
/// and plays the configured AudioClip with 2D/3D options.
/// Attach to any GameObject. Use the Inspector fields or call Play() from code.
/// </summary>
public class PlayAudioAtPosition : MonoBehaviour
{
    [Header("What to play")]
    [SerializeField] private AudioClip clip;
    [Range(0f, 1f)]
    [SerializeField] private float volume = 1f;
    [Range(-3f, 3f)]
    [SerializeField] private float pitch = 1f;
    [SerializeField] private bool loop = false;

    [Tooltip("Optional routing to a mixer group.")]
    [SerializeField] private AudioMixerGroup outputMixer;

    [Header("Where to play")]
    [Tooltip("If true, uses this GameObject's transform.position; otherwise uses 'worldPosition'.")]
    [SerializeField] private bool useTransformPosition = true;

    [Tooltip("Used when 'useTransformPosition' is false.")]
    [SerializeField] private Vector3 worldPosition = Vector3.zero;

    [Header("3D/Spatial Settings")]
    [Tooltip("0 = 2D (UI/FX), 1 = fully 3D (positional).")]
    [Range(0f, 1f)]
    [SerializeField] private float spatialBlend = 1f;

    [SerializeField] private AudioRolloffMode rolloffMode = AudioRolloffMode.Linear;
    [Min(0.01f)]
    [SerializeField] private float minDistance = 1f;
    [Min(0.01f)]
    [SerializeField] private float maxDistance = 25f;
    [Tooltip("If true, destroys the spawned AudioSource object automatically when done (non-looping only).")]
    [SerializeField] private bool destroyWhenDone = true;

    [Header("Advanced (optional)")]
    [Tooltip("Optional parent for the spawned AudioSource object (helps keep Hierarchy tidy).")]
    [SerializeField] private Transform spawnedParent;

    /// <summary>Play the configured clip once at the configured position.</summary>
    [ContextMenu("Play")]
    public void Play()
    {
        if (clip == null)
        {
            Debug.LogWarning($"[{nameof(PlayAudioAtPosition)}] No AudioClip assigned.", this);
            return;
        }

        Vector3 pos = useTransformPosition ? transform.position : worldPosition;

        // Create a lightweight temp object to host the AudioSource
        string goName = $"OneShotAudio_{clip.name}";
        GameObject host = new GameObject(goName);
        host.transform.position = pos;
        if (spawnedParent != null) host.transform.SetParent(spawnedParent, worldPositionStays: true);

        var source = host.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = Mathf.Clamp01(volume);
        source.pitch = Mathf.Clamp(pitch, -3f, 3f);
        source.loop = loop;
        source.spatialBlend = Mathf.Clamp01(spatialBlend);
        source.rolloffMode = rolloffMode;
        source.minDistance = Mathf.Max(0.01f, minDistance);
        source.maxDistance = Mathf.Max(source.minDistance, maxDistance);
        source.playOnAwake = false;
        source.dopplerLevel = 0f; // usually better for discrete SFX
        if (outputMixer != null) source.outputAudioMixerGroup = outputMixer;

        source.Play();

        // Auto-cleanup for non-looping sounds
        if (!loop && destroyWhenDone)
        {
            float life = SafeLifetimeSeconds(clip, source.pitch);
            host.AddComponent<AutoDestroyAfter>().Init(life);
        }
    }

    /// <summary>
    /// Stop all spawned non-looping sounds parented under 'spawnedParent' (if set).
    /// Looping sounds should be stopped by tracking references or disabling the component that called Play().
    /// </summary>
    [ContextMenu("Stop & Cleanup Spawned (under Parent)")]
    public void StopAndCleanupUnderParent()
    {
        if (spawnedParent == null) return;
        var toDestroy = new System.Collections.Generic.List<GameObject>();
        foreach (Transform child in spawnedParent)
        {
            if (child.GetComponent<AudioSource>() != null)
                toDestroy.Add(child.gameObject);
        }
        foreach (var go in toDestroy) Destroy(go);
    }

    private static float SafeLifetimeSeconds(AudioClip c, float actualPitch)
    {
        if (c == null || c.length <= 0f) return 1f;
        float p = Mathf.Max(0.01f, Mathf.Abs(actualPitch));
        return c.length / p;
    }

    // Nice editor gizmos to see where the sound will spawn & its range
    private void OnDrawGizmosSelected()
    {
        Vector3 pos = useTransformPosition ? transform.position : worldPosition;

        Gizmos.color = new Color(0.2f, 0.8f, 1f, 0.6f);
        Gizmos.DrawWireSphere(pos, Mathf.Clamp(minDistance, 0.01f, maxDistance));
        Gizmos.color = new Color(0.2f, 0.8f, 1f, 0.25f);
        Gizmos.DrawWireSphere(pos, Mathf.Max(minDistance, maxDistance));

        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(pos, 0.1f);
    }

    /// <summary>
    /// Small helper component to auto-destroy a GameObject after a delay.
    /// </summary>
    private class AutoDestroyAfter : MonoBehaviour
    {
        public void Init(float seconds) => StartCoroutine(DestroyAfter(seconds));

        private IEnumerator DestroyAfter(float s)
        {
            yield return new WaitForSeconds(s);
            if (this != null && gameObject != null)
                Destroy(gameObject);
        }
    }
}
