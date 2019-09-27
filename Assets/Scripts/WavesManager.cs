using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavesManager : MonoBehaviour
{
    //public List<Transform> waveeees;
    public List<Animator> waveeees;

    public List<GameObject> randomWaves;


    public Animator baliseAnimator;

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

        int i = 0;
        foreach (Animator animWav in waveeees)
        {
            float index = i++;
            float value = (((index) / waveeees.Count) * 0.3f);
            animWav.speed = 1f - value;
            if (animWav.name.Contains("4"))
            {
                baliseAnimator.speed = animWav.speed;
            }
        }
    }

    public void Update()
    {
        float pluie = (TempestFather.instance.pluviometre / 100f);

        foreach (Animator animWav in waveeees)
        {
            animWav.SetLayerWeight(0, 1 - pluie);
            animWav.SetLayerWeight(1, pluie);
        }

        float intensite = 0.5f + ( 0.5f * TempestFather.instance.intensite);

        baliseAnimator.SetLayerWeight(0, 1 - (pluie * intensite));
        baliseAnimator.SetLayerWeight(1, pluie * intensite); 
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
        TempestFather.instance.LightChange(wavesData.light_on);
    }


}
