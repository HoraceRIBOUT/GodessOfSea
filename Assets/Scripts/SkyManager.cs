using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyManager : MonoBehaviour
{

    public Gradient lightColor;


    public Camera cameraBackGround;//maybe a sprite no  ?




    public void Update()
    {
        cameraBackGround.backgroundColor = lightColor.Evaluate(TempestFather.instance.secousse);
    }

}
