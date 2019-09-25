using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WavesManager))]
public class EditorUpdate : Editor
{

    public override void OnInspectorGUI()
    {
        WavesManager spawn = (WavesManager)target;

        if (DrawDefaultInspector())
        {
            // spawn.RotateLeft();
        }

        int num = 0;
        float time = 0;
        foreach (WavesManager.Waves w in spawn.wavesPattern)
        {
            w.id = "Waves n°" + num++;
            w.time = time;
            time += w.duration;
            //if(w.current)

        }

    }


}
