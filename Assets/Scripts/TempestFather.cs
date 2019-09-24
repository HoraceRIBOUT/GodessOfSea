using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempestFather : MonoBehaviour
{
    public static TempestFather instance;

    public void Awake()
    {
        if (TempestFather.instance == null)
            TempestFather.instance = this;
        else
            Destroy(this.gameObject);
    }


    public float secousse = 0;
    private float real_secousse = 0;
    public float reduceSecoussePerSecond = 0.33f;
    public AnimationCurve secoussePower = AnimationCurve.Linear(0, 0, 1, 1);
    public float asymptotic_value = 0.90f;


    public void AddSecousse(float add)
    {
        real_secousse += add;
        if (real_secousse > 1)
            real_secousse = 1;

    }


    public void Update()
    {
        if (real_secousse > 0)
            real_secousse -= reduceSecoussePerSecond * Time.deltaTime;

        CalculSecousse();

        if (Input.GetKeyDown(KeyCode.UpArrow))
            AddSecousse(0.12f);
    }


    void CalculSecousse()
    {
        float target_secousse = secoussePower.Evaluate(real_secousse);
        secousse = (asymptotic_value * secousse) + ((1 - asymptotic_value) * target_secousse);
    }
}
