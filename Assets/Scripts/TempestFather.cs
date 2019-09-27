using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempestFather : MonoBehaviour
{
    public static TempestFather instance;

    public bool DEBUG_MODE = false;

    public void Awake()
    {
#if !UNITY_EDITOR
        DEBUG_MODE = false;
#endif
        dbg_info_WiiInput.enabled = DEBUG_MODE;

        if (TempestFather.instance == null)
        {
            TempestFather.instance = this;
            AkSoundEngine.PostEvent(startEvent.Id, gameObject);

            startEvent.Post(this.gameObject, (uint)AkCallbackType.AK_MusicSyncBeat, CallBackFunction);
        }
        else
            Destroy(this.gameObject);
    }


    void CallBackFunction(object baseObject, AkCallbackType type, object info)
    {
        if (canShake)
            if (intensite != 0)
                AkSoundEngine.PostEvent(snd_shakeBeat.Id, this.gameObject);
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
    public int currentPalier = 1;
    public float[] seuilForEachPalier = new float[3];

    public bool ignoringMovement = false;
    public float delayForIgnoring = 1f;

    [Header("Intensite management")]
    public float intensite = 0;
    public float badIntensite = 0;
    private float real_intensite = 0;
    public float reduceIntensitePerSecond = 0.33f;
    public AnimationCurve intensitePower = AnimationCurve.Linear(0, 0, 1, 1);
    public float asymptotic_value = 0.90f;


    [Header("UI slider")]
    public UnityEngine.UI.Slider sliderIntensite;
    public UnityEngine.UI.Image sliderBG;
    public Gradient intensiteGradient;

    public Transform rotationAiguille;
    public List<SpriteRenderer> spriteCompteurPression;
    
    public UnityEngine.UI.Slider sliderPluviometre;
    public UnityEngine.UI.Image sliderPlBG;
    public Gradient pluviometreGradient;

    public Transform scaleTube;

    [Header("Sound")]
    public AK.Wwise.Event startEvent;
    public AK.Wwise.Event musicEvent;

    public AK.Wwise.Event snd_light_on;
    public AK.Wwise.Event snd_light_off;
    public AK.Wwise.Event snd_eclair;

    public AK.Wwise.Event snd_menu;
    public AK.Wwise.Event snd_screenFin;
    public AK.Wwise.Event snd_restart;

    public AK.Wwise.Event snd_shakeBeat;

    public AK.Wwise.RTPC snd_intensiteRTPC;

    public AK.Wwise.RTPC snd_palierRTPC;
    

    [Header("Wiimote gestion")]
    public float seuilForWiimoteDetection = 1f;
    public AnimationCurve wiimoteInputCurve;
    public UnityEngine.UI.Text dbg_info_WiiInput;
    
    public void Update()
    {
        if (menu)
        {
            float magnit = InputManager.wiimoteInput.magnitude;
            if (Input.GetKey(KeyCode.UpArrow) || magnit > seuilForWiimoteDetection)
            {
                if (Input.GetKey(KeyCode.UpArrow))
                    AddIntensite(0.33f * Time.deltaTime);
                //WII
                if (magnit > seuilForWiimoteDetection)
                    AddIntensite(0.33f * Time.deltaTime);
                //END WII
            }
            else if (real_intensite > 0)
                real_intensite -= 0.33f * Time.deltaTime;
            else
                real_intensite = 0;

            textDecompte.text = (real_intensite < (1f / 3f)) ? "3" : ((real_intensite < (2f / 3f)) ? "2" : ((real_intensite < (3f / 3f)) ? "1" : "0"));

            if (real_intensite >= 1)
            {
                menu = false;
                textDecompte.enabled = false;
                AkSoundEngine.PostEvent(musicEvent.Id, gameObject);
            }
            return;
        }


        if (!gameOver)
        {
            if (real_intensite > 0)
            {
                real_intensite -= reduceIntensitePerSecond * Time.deltaTime * (canShake ? 1 : 0);
            }
            else
                real_intensite = 0;

            Calculntensite();

            if (Input.GetKey(KeyCode.UpArrow))
                if (canShake)
                    AddIntensite(0.12f);
            else if(!ignoringMovement)
                    badIntensite += 0.12f;
            
            //WII
            float magnit = InputManager.wiimoteInput.magnitude;
            if ((magnit > seuilForWiimoteDetection) && canShake)
                if (canShake)
                    AddIntensite(wiimoteInputCurve.Evaluate(magnit) * Time.deltaTime);
                else if (!ignoringMovement)
                    badIntensite += wiimoteInputCurve.Evaluate(magnit) * Time.deltaTime;
            //END WII

            CalculPluviometre();
        }
        else
        {
            float magnit = InputManager.wiimoteInput.magnitude;
            if (Input.GetKey(KeyCode.UpArrow) || magnit > seuilForWiimoteDetection)
            {
                if (Input.GetKey(KeyCode.UpArrow))
                    AddIntensite(0.33f * Time.deltaTime);
                //WII
                if (magnit > seuilForWiimoteDetection)
                    AddIntensite(0.33f * Time.deltaTime);
                //END WII
            }
            else if (real_intensite > 0)
                real_intensite -= reduceIntensitePerSecond * Time.deltaTime;
            else
                real_intensite = 0;

            textDecompte.text = (real_intensite < (1f / 3f)) ? "3" : ((real_intensite < (2f / 3f)) ? "2" : ((real_intensite < (3f / 3f)) ? "1" : "0"));

            if (real_intensite >= 1)
                Restart();
        }


        if (DEBUG_MODE)
        {
            Vector3 vec = InputManager.wiimoteInput;
            dbg_info_WiiInput.text = vec.x+"=X\n"+ vec.y + "=Y\n"+ vec.z + "=Z";
            if (Input.GetKey(KeyCode.F))
                print("WiiInput = " + vec + " at " + Time.time);
        }


        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

    }



    public void AddIntensite(float add)
    {
        real_intensite += add;
        if (real_intensite > 1)
            real_intensite = 1;
    }

    void Calculntensite()
    {
        float target_intensite = intensitePower.Evaluate(real_intensite);
        intensite = (asymptotic_value * intensite) + ((1 - asymptotic_value) * target_intensite);

        sliderIntensite.value = intensite;
        sliderBG.color = intensiteGradient.Evaluate(intensite);

        rotationAiguille.rotation = Quaternion.Euler(Vector3.forward * -180 * intensite);

        AkSoundEngine.SetRTPCValue(snd_intensiteRTPC.Id, (int)(intensite * 4));
    }
    
    void CalculPluviometre()
    {
        if (canShake)
        {
            pluviometre += changePluviometrePerSecond.Evaluate(intensite) * Time.deltaTime * (DEBUG_MODE ? 4 : 1) * 1.7f;
        }
        else
        {
            if(badIntensite != 0 && !ignoringMovement)
            {
                //Debug.Log("pluviometre go down : " + pluviometre);
                pluviometre += badIntensite * reducePluviometreWhenLightOff * Time.deltaTime;
                badIntensite = 0;
                //Debug.Log("pluviometre is down : " + pluviometre);
            }
        }
        pluviometre = Mathf.Max(0, pluviometre);
        pluviometre = Mathf.Min(100, pluviometre);

        sliderPluviometre.value = pluviometre / 100f;
        sliderPlBG.color = pluviometreGradient.Evaluate(pluviometre / 100f);

        scaleTube.localScale = new Vector3(1, 1.05f * (float)pluviometre / 100f, 1);

        //seuil !
        if (currentPalier != 3 && pluviometre > seuilForEachPalier[currentPalier - 1])
        {
            currentPalier++;

            AkSoundEngine.SetRTPCValue(snd_palierRTPC.Id, currentPalier);
        }
    }

    public void LightChange(bool on)
    {
        canShake = on;
        theLight.gameObject.SetActive(on);

        if (on)
            AkSoundEngine.PostEvent(snd_light_on.Id, this.gameObject);
        else
            AkSoundEngine.PostEvent(snd_light_off.Id, this.gameObject);

        //sliderIntensite.gameObject.SetActive(on);
        foreach(SpriteRenderer sR in spriteCompteurPression)
        {
            sR.color = new Color(1, 1, 1, on ? 1 : 0.2f);
        }
        real_intensite = 0f;
        Calculntensite();

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

    public bool menu = true;

    public GameObject gameOverUI;
    public UnityEngine.UI.Text textDecompte;
    public UnityEngine.UI.Text textScore;

    public void DisplayGameOver()
    {
        gameOverUI.SetActive(true);

        real_intensite = 0;
        //pas de calcul d'intensité

        gameOver = true;

        textScore.text = "Score : " + pluviometre;
        textDecompte.enabled = true;

        AkSoundEngine.PostEvent(snd_screenFin.Id, this.gameObject);
    }

    public void Restart()
    {
        //do a smooth restart ! 
        //not on my watch !
        //YOLOOOOOO
        AkSoundEngine.PostEvent(snd_restart.Id, this.gameObject);
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }


#endregion




    public void SoundEclair()
    {
        AkSoundEngine.PostEvent(snd_eclair.Id, this.gameObject);
    }
}
