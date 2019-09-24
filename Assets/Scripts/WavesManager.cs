using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavesManager : MonoBehaviour
{
    public List<Transform> waveeees;

    public List<GameObject> randomWaves;

    [System.Serializable]
    public class Waves
    {
        [HideInInspector] public string id;
        public bool light_on = true;
        public float duration = 0;
        public float time = 0;
    }

    public List<Waves> wavesPattern = new List<Waves>();







}
