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


    [Header("Object needed to be accessible")]
    public Light theLight;

    [Header("Value for everybody")]
    public float score = 0;
    public bool canShake = false;

    public int currentLevel = 0;
    public float intensity = 0;
    public float[] seuilForEachLevel = new float[3];

    [Header("Secousse management")]
    public float secousse = 0;
    private float real_secousse = 0;
    public float reduceSecoussePerSecond = 0.33f;
    public AnimationCurve secoussePower = AnimationCurve.Linear(0, 0, 1, 1);
    public float asymptotic_value = 0.90f;


    [Header("UI slider")]
    public UnityEngine.UI.Slider sliderSecousse;
    public UnityEngine.UI.Image sliderBG;
    public Gradient secousseGradient;


    public void AddSecousse(float add)
    {
        real_secousse += add;
        if (real_secousse > 1)
            real_secousse = 1;

    }


    public void Update()
    {
        if (gameOver)
        {
            if (Input.GetKey(KeyCode.UpArrow))
                AddSecousse(0.33f * Time.deltaTime);
            else if(real_secousse > 0)
                real_secousse -= reduceSecoussePerSecond * Time.deltaTime;
            else
                real_secousse = 0;

            textDecompte.text = (real_secousse < (1f/ 3f)) ? "3" : ((real_secousse < (2f / 3f)) ? "2": ((real_secousse < (3f/3f)) ? "1" : "0"));

            if (real_secousse >= 1)
                Restart();
        }
        else
        {
            if (real_secousse > 0)
                real_secousse -= reduceSecoussePerSecond * Time.deltaTime * (canShake ? 1 : 0.5f);
            else
                real_secousse = 0;

            CalculSecousse();

            if (Input.GetKeyDown(KeyCode.UpArrow) && canShake)
                AddSecousse(0.12f);
        }

        
    }


    void CalculSecousse()
    {
        float target_secousse = secoussePower.Evaluate(real_secousse);
        secousse = (asymptotic_value * secousse) + ((1 - asymptotic_value) * target_secousse);

        sliderSecousse.value = secousse;
        sliderBG .color = secousseGradient.Evaluate(secousse);
    }



    #region GAMEOVER PART
    [Header("GameOver")]
    public bool gameOver = false;

    public GameObject gameOverUI;
    public UnityEngine.UI.Text textDecompte;

    public void DisplayGameOver()
    {
        gameOverUI.SetActive(true);

        real_secousse = 0;
        //pas de calcul de secousse

        gameOver = true;
    }

    public void Restart()
    {
        //do a smooth restart ! 
        //not on my watch !
        //YOLOOOOOO
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }


    #endregion
}
