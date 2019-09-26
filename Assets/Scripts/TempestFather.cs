using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempestFather : MonoBehaviour
{
    public static TempestFather instance;

    public void Awake()
    {
        if (TempestFather.instance == null)
        {
            TempestFather.instance = this;
            AkSoundEngine.PostEvent(startEvent.Id, gameObject);
            AkSoundEngine.PostEvent(musicEvent.Id, gameObject);
        }
        else
            Destroy(this.gameObject);
    }


    [Header("Object needed to be accessible")]
    public Light theLight;

    [Header("Value for everybody")]
    public float score = 0;
    public bool canShake = false;

    [Range(0,100)]
    public float pluviometre = 0;
    public float reducePluviometrePerSecond = 0.33f;
    public AnimationCurve changePluviometrePerSecond;
    public float reducePluviometreWhenLightOff = -0.2f;
    public int currentPalier = 0;
    public float[] seuilForEachPalier = new float[3];

    public bool ignoringMovement = false;
    public float delayForIgnoring = 1f;

    [Header("Intensite management")]
    public float intensite = 0;
    private float real_intensite = 0;
    public float reduceIntensitePerSecond = 0.33f;
    public AnimationCurve intensitePower = AnimationCurve.Linear(0, 0, 1, 1);
    public float asymptotic_value = 0.90f;


    [Header("UI slider")]
    public UnityEngine.UI.Slider sliderIntensite;
    public UnityEngine.UI.Image sliderBG;
    public Gradient intensiteGradient;
    
    public UnityEngine.UI.Slider sliderPluviometre;
    public UnityEngine.UI.Image sliderPlBG;
    public Gradient pluviometreGradient;

    [Header("Sound")]
    public AK.Wwise.Event startEvent;
    public AK.Wwise.Event musicEvent;
    public AK.Wwise.Switch switchForSeuil;



    public void AddIntensite(float add)
    {
        real_intensite += add;
        if (real_intensite > 1)
            real_intensite = 1;

    }


    public float seuil = 1f;

    public void Update()
    {
        if (gameOver)
        {
            //TO DO : change that TOO !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            if (Input.GetKey(KeyCode.UpArrow))
                AddIntensite(0.33f * Time.deltaTime);
            //
            float magnit = InputManager.wiimoteInput.magnitude;
            if (magnit > seuil)
                AddIntensite((magnit / 9.0f)* Time.deltaTime);
            //
            else if(real_intensite > 0)
                real_intensite -= reduceIntensitePerSecond * Time.deltaTime;
            else
                real_intensite = 0;

            textDecompte.text = (real_intensite < (1f/ 3f)) ? "3" : ((real_intensite < (2f / 3f)) ? "2": ((real_intensite < (3f/3f)) ? "1" : "0"));

            if (real_intensite >= 1)
                Restart();
        }
        else
        {
            if (real_intensite > 0 && !ignoringMovement)
                real_intensite -= reduceIntensitePerSecond * Time.deltaTime * (canShake ? 1 : 0.5f);
            else if(!ignoringMovement)
                real_intensite = 0;

            Calculntensite();

            //TO DO : change that here                          !!!!!!!!!!!!!!!!!!!!!!!
            if (Input.GetKeyDown(KeyCode.UpArrow) && canShake)
                AddIntensite(0.12f);
            //
            float magnit = InputManager.wiimoteInput.magnitude;
            if (magnit > seuil && canShake)
                AddIntensite((magnit / 3.0f) * Time.deltaTime);
            //
            CalculPluviometre();
        }

        
    }


    void Calculntensite()
    {
        float target_intense = intensitePower.Evaluate(real_intensite);
        intensite = (asymptotic_value * intensite) + ((1 - asymptotic_value) * target_intense);

        sliderIntensite.value = intensite;
        sliderBG .color = intensiteGradient.Evaluate(intensite);
    }
    
    void CalculPluviometre()
    {
        if (canShake)
        {
            pluviometre += changePluviometrePerSecond.Evaluate(intensite) * Time.deltaTime;
        }
        else if(!ignoringMovement)
        {
            pluviometre += intensite * reducePluviometreWhenLightOff * Time.deltaTime;
        }
        pluviometre = Mathf.Max(0, pluviometre);
        pluviometre = Mathf.Min(100, pluviometre);

        sliderPluviometre.value = pluviometre/100;
        sliderPlBG.color = pluviometreGradient.Evaluate(pluviometre / 100);
    }

    public void LightChange(bool on)
    {
        canShake = on;
        theLight.enabled = on;


        sliderIntensite.gameObject.SetActive(on);
        //intensite = 0.5f;
        //sliderIntensite.value = 0.5f;

        ignoringMovement = true;
        Invoke("StopIgnoreMovement", delayForIgnoring);
    }

    void StopIgnoreMovement()
    {
        ignoringMovement = false;
    }

    #region GAMEOVER PART
    [Header("GameOver")]
    public bool gameOver = false;

    public GameObject gameOverUI;
    public UnityEngine.UI.Text textDecompte;

    public void DisplayGameOver()
    {
        gameOverUI.SetActive(true);

        real_intensite = 0;
        //pas de calcul d'intensité

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
