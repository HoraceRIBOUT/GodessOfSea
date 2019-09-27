using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorWithLayer : MonoBehaviour
{
    public Animator animatorTargeted;

    public float timeToTransitionne = 1f;
    private int palier;

    public enum ValueLook
    {
        intensite,
        pluviometre,
        palier,
    }

    public ValueLook valueToSeek = ValueLook.pluviometre;

    public void Update()
    {
        switch (valueToSeek)
        {
            case ValueLook.intensite:
                animatorTargeted.SetLayerWeight(0, 1 - TempestFather.instance.intensite);
                animatorTargeted.SetLayerWeight(1, TempestFather.instance.intensite);
                break;
            case ValueLook.pluviometre:
                animatorTargeted.SetLayerWeight(0, 1 - (TempestFather.instance.pluviometre/100f));
                animatorTargeted.SetLayerWeight(1, TempestFather.instance.intensite/100f);
                break;
            case ValueLook.palier:
                if(palier != TempestFather.instance.currentPalier)
                {
                    palier = TempestFather.instance.currentPalier;
                    StartCoroutine(transitionLayer());
                }
                break;
            default:
                break;
        }
    }


    IEnumerator transitionLayer()
    {
        Debug.Log("End coroutine");
        if (palier != 0)
        {
            float lerp = 0;
            while (lerp < 1)
            {
                animatorTargeted.SetLayerWeight(palier-1, 1f-lerp);
                animatorTargeted.SetLayerWeight(palier, lerp);

                lerp += Time.deltaTime/timeToTransitionne;

                yield return new WaitForSeconds(1f / 60f);
            }
        }
        Debug.Log("Start coroutine");
        animatorTargeted.SetLayerWeight(palier - 1, 0);
        animatorTargeted.SetLayerWeight(palier, 1);

    }
}
