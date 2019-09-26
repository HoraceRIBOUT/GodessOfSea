using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorWithLayer : MonoBehaviour
{
    public Animator animatorTargeted;

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
                for (int i = 0; i < animatorTargeted.layerCount; i++)
                {
                    animatorTargeted.SetLayerWeight(i, 0);
                }
                animatorTargeted.SetLayerWeight(TempestFather.instance.currentPalier, 1);
                break;
            default:
                break;
        }
    }
}
