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
        [HideInInspector] public bool current = false;
    }

    public List<Waves> wavesPattern = new List<Waves>();

    public void Start()
    {
        StartCoroutine(WavesByWavesOnWavesList());
    }

    private IEnumerator WavesByWavesOnWavesList()
    {
        foreach (Waves w in wavesPattern)
        {
            w.current = true;
            //
            DoWaves(w);
            //
            yield return new WaitForSeconds(w.duration);
            w.current = false;
        }

        TempestFather.instance.DisplayGameOver();

    }

    public void DoWaves(Waves wavesData)
    {
        TempestFather.instance.canShake = wavesData.light_on; // to make it in a way that when we change it, it change the light, could be way better
        //So, this one
        TempestFather.instance.theLight.enabled = wavesData.light_on;
        //that's it
    }


}
