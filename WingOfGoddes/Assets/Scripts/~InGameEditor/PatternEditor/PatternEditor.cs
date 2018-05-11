using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PatternEditor : EditorBase {
    
    public class Frame
    {
        public static Frame select;

        public PatternDatabase.PatternInfo info;
        public List<DirectionMarker> markers = new List<DirectionMarker>();
    }

    public GameObject markerBase;
    public GameObject spikeMarkerBase;

    private PatternPresetDatabase presetDatabase;

    private DirectionMarker marker;

    public void Awake()
    {
        presetDatabase = AssetDatabase.LoadAssetAtPath("Assets/Database/PatternPresetDatabase.asset", typeof(PatternPresetDatabase)) as PatternPresetDatabase;
        CreateMarker();
    }

    public void Update()
    {
        marker.Progress(null);
    }

    public void CreateMarker()
    {
        marker = Instantiate(markerBase).GetComponent<DirectionMarker>();
        marker.transform.SetParent(this.transform);

        PatternPresetDatabase.PatternPreset preset = presetDatabase.data[0];

        for(int i = 0; i < preset.shots.Count; ++i)
        {
            Transform tp = Instantiate(spikeMarkerBase).transform;

            tp.SetParent(marker.transform);
            tp.rotation = Quaternion.Euler(0f,0f,preset.shots[i].angle);
        }
    }
}