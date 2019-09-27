using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyManager : MonoBehaviour
{

    public Gradient lightColor;


    public Camera cameraBackGround;

    public ParticleSystem[] pluieStep = new ParticleSystem[3];

    private int pallier = 0;


    public void Update()
    {
        cameraBackGround.backgroundColor = lightColor.Evaluate(TempestFather.instance.pluviometre/100f);

        if (pallier != TempestFather.instance.currentPalier)
        {
            pallier = TempestFather.instance.currentPalier;
            foreach (ParticleSystem pS in pluieStep)
            {
                pS.Stop();
            }
            if(pallier > 2)
            {
                pluieStep[2].Play();
                TempestFather.instance.SoundEclair();
            }
            if (pallier > 1)
            {
                pluieStep[1].Play();
            }
            if (pallier > 0)
            {
                pluieStep[0].Play();
            }
        }
    }

}
