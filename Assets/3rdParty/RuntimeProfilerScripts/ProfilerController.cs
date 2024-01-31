using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using Unity.Profiling;
using Unity.Profiling.LowLevel.Unsafe;

public class ProfilerController : MonoBehaviour
{
    /************************************************************************************************************
    * Source: https://docs.unity3d.com/2020.2/Documentation/ScriptReference/Unity.Profiling.ProfilerRecorder.html
    *************************************************************************************************************/
    
    //public static ProfilerMarker UpdatePlayerProfilerMarker = new ProfilerMarker("Player.Update");

    string statsText;
    ProfilerRecorder systemMemoryRecorder;
    ProfilerRecorder gcMemoryRecorder;
    ProfilerRecorder mainThreadTimeRecorder;
    ProfilerRecorder drawCallsCountRecorder;
    private ProfilerRecorder batchesCountRecorder;
    private ProfilerRecorder setpassCallsRecorder;
    private ProfilerRecorder verticesCountRecorder;

    private long drawCall;
    private long setPassCalls;
    private long batchesCount;
    private long verticesCount;

    private Coroutine _coroutine;

    static double GetRecorderFrameAverage(ProfilerRecorder recorder)
    {
        var samplesCount = recorder.Capacity;
        if (samplesCount == 0)
            return 0;

        double r = 0;
        var samples = new List<ProfilerRecorderSample>(samplesCount);
        recorder.CopyTo(samples);
        for (var i = 0; i < samples.Count; ++i)
            r += samples[i].Value;
        r /= samplesCount;

        return r;
    }

    void OnEnable()
    {
        systemMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "System Used Memory");
        gcMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC Reserved Memory");
        mainThreadTimeRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Internal, "Main Thread", 15);
        drawCallsCountRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Draw Calls Count");
        batchesCountRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Batches Count");
        setpassCallsRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "SetPass Calls Count");
        verticesCountRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Vertices Count");

        GetAvailableProfilerStats.EnumerateProfilerStats();

        _coroutine = StartCoroutine(RepeatUpdateStats());
    }
    
    IEnumerator RepeatUpdateStats()
    {
        while (true)
        {
            // 1초 대기 후 UpdateStats 함수 호출
            yield return new WaitForSeconds(1f);
            UpdateStats();
        }
    }

    void OnDisable()
    {
        StopCoroutine(_coroutine);
        systemMemoryRecorder.Dispose();
        gcMemoryRecorder.Dispose();
        mainThreadTimeRecorder.Dispose();
        drawCallsCountRecorder.Dispose();
    }

    // private void Update()
    // {
    //     UpdateStats();
    // }

    void UpdateStats()
    {
        var sb = new StringBuilder(500);
        sb.AppendLine($"Frame Time: {GetRecorderFrameAverage(mainThreadTimeRecorder) * (1e-6f):F1} ms");
        sb.AppendLine($"GC Memory: {gcMemoryRecorder.LastValue / (1024 * 1024)} MB");
        sb.AppendLine($"System Memory: {systemMemoryRecorder.LastValue / (1024 * 1024)} MB");
        sb.AppendLine("====[Graphics]====");
        if (drawCallsCountRecorder.LastValue > 0)
        {
            drawCall = drawCallsCountRecorder.LastValue;
            sb.AppendLine($"Draw Calls: {drawCall}");
        }
        else
        {
            sb.AppendLine($"Draw Calls: {drawCall} (released)");
        }
        
        if (batchesCountRecorder.LastValue > 0)
        {
            batchesCount = batchesCountRecorder.LastValue;
            sb.AppendLine($"Batches Count: {batchesCount}");
        }
        else
        {
            sb.AppendLine($"Batches Count: {batchesCount} (released)");
        }
        
        if (setpassCallsRecorder.LastValue > 0)
        {
            setPassCalls = setpassCallsRecorder.LastValue;
            sb.AppendLine($"SetPass Calls Count: {setPassCalls}");
        }
        else
        {
            sb.AppendLine($"SetPass Calls Count: {setPassCalls} (released)");
        }
        
        if (verticesCountRecorder.LastValue > 0)
        {
            verticesCount = verticesCountRecorder.LastValue;
            sb.AppendLine($"Vertices Count: {verticesCount}");
        }
        else
        {
            sb.AppendLine($"Vertices Count: {verticesCount} (released)");
        }


        statsText = sb.ToString();
    }

    void OnGUI()
    {
        GUI.TextArea(new Rect(10, 30, 250, 200), statsText);
    }
}
