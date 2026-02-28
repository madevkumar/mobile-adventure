using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton that manages the time-loop mechanic.
/// Each frame during normal play it asks every registered IRewindable to
/// record its state; when the player activates rewind the states are played
/// back in reverse.
/// </summary>
public class TimeLoopController : MonoBehaviour
{
    public static TimeLoopController Instance { get; private set; }

    [Tooltip("Maximum number of seconds that can be rewound.")]
    public float maxRewindDuration = 5f;

    [Tooltip("Key / button used to activate rewind (keyboard fallback).")]
    public KeyCode rewindKey = KeyCode.R;

    public bool IsRewinding { get; private set; }

    private readonly List<IRewindable> _rewindables = new List<IRewindable>();

    // How many frames we store (derived from maxRewindDuration at runtime).
    private int _maxFrames;

    // ------------------------------------------------------------------ //

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        // Derive the frame budget from the project's actual fixed timestep.
        _maxFrames = Mathf.RoundToInt(maxRewindDuration / Time.fixedDeltaTime);
    }

    void Update()
    {
        // Toggle rewind while the button is held.
        IsRewinding = Input.GetKey(rewindKey);

        // Slow down time while rewinding to give a visual effect.
        // Using a small positive value (not 0) so that FixedUpdate continues
        // to run and can step through the recorded state history.
        Time.timeScale = IsRewinding ? 0.1f : 1f;
    }

    void FixedUpdate()
    {
        if (IsRewinding)
        {
            foreach (var r in _rewindables)
                r.RewindState();
        }
        else
        {
            foreach (var r in _rewindables)
                r.RecordState(_maxFrames);
        }
    }

    // ------------------------------------------------------------------ //

    /// <summary>Register an object so that it participates in time rewind.</summary>
    public void Register(IRewindable rewindable)
    {
        if (!_rewindables.Contains(rewindable))
            _rewindables.Add(rewindable);
    }

    /// <summary>Unregister an object (called on destruction).</summary>
    public void Unregister(IRewindable rewindable)
    {
        _rewindables.Remove(rewindable);
    }
}

/// <summary>
/// Any object that wants to participate in the time-loop rewind system
/// should implement this interface.
/// </summary>
public interface IRewindable
{
    /// <summary>
    /// Called every FixedUpdate during normal play.
    /// Implementations should push the current state onto an internal buffer
    /// and trim the buffer to <paramref name="maxFrames"/>.
    /// </summary>
    void RecordState(int maxFrames);

    /// <summary>
    /// Called every FixedUpdate while rewinding.
    /// Implementations should pop the most recent state and restore it.
    /// </summary>
    void RewindState();
}
