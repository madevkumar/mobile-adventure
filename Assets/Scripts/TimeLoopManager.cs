using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// TimeLoopManager handles time manipulation mechanics.
/// Records and replays player actions during time loops.
/// </summary>
public class TimeLoopManager : MonoBehaviour
{
    [Header("Time Loop Settings")]
    [SerializeField] private float loopDuration = 30f;
    [SerializeField] private float rewindSpeed = 2f;

    private float elapsedTime;
    private bool isRewinding;
    private List<FrameData> recordedFrames = new List<FrameData>();
    private PlayerController playerController;

    [System.Serializable]
    public struct FrameData
    {
        public float time;
        public Vector3 playerPosition;
        public Vector3 playerVelocity;
    }

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartRewind();
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            StopRewind();
        }

        if (!isRewinding)
        {
            RecordFrame();
        }
        else
        {
            Rewind();
        }

        if (elapsedTime >= loopDuration)
        {
            ResetLoop();
        }
    }

    private void RecordFrame()
    {
        recordedFrames.Add(new FrameData
        {
            time = elapsedTime,
            playerPosition = transform.position,
            playerVelocity = GetComponent<Rigidbody2D>().velocity
        });
    }

    public void StartRewind()
    {
        isRewinding = true;
    }

    public void StopRewind()
    {
        isRewinding = false;
    }

    private void Rewind()
    {
        if (recordedFrames.Count == 0) return;

        elapsedTime -= Time.deltaTime * rewindSpeed;
        elapsedTime = Mathf.Max(0, elapsedTime);

        // Find the frame closest to current elapsed time
        FrameData targetFrame = recordedFrames[0];
        foreach (var frame in recordedFrames)
        {
            if (frame.time <= elapsedTime)
            {
                targetFrame = frame;
            }
        }

        transform.position = targetFrame.playerPosition;
        GetComponent<Rigidbody2D>().velocity = targetFrame.playerVelocity;
    }

    private void ResetLoop()
    {
        elapsedTime = 0;
        isRewinding = false;
        recordedFrames.Clear();
        playerController.ResetPosition(transform.position);
    }

    public float GetRemainingTime()
    {
        return loopDuration - elapsedTime;
    }
}