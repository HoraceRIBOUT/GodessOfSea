using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicClickingCube : MonoBehaviour
{
    [Tooltip("First value : how long OFF | Second value : how long ON")]
    public Vector2 clickingTiming = new Vector2(1, 2);
    public Light aLight;
    private float intensityAtStart = 1;

    // Start is called before the first frame update
    void Start()
    {
        Sound_Intro();

        intensityAtStart = aLight.intensity;
        aLight.intensity = 0;

        

        StartCoroutine(ClickingOnAndOff());
    }

    IEnumerator ClickingOnAndOff()
    {
        while (true)
        {
            Sound_LightOff();
            aLight.intensity = 0;
            yield return new WaitForSeconds(clickingTiming.x);
            Sound_LightOn();
            aLight.intensity = intensityAtStart;
            yield return new WaitForSeconds(clickingTiming.y);
        }
    }

    void Sound_Intro()
    {

    }

    void Sound_LightOn()
    {

    }

    void Sound_LightOff()
    {

    }


}
