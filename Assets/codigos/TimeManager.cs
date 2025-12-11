using UnityEngine;
using UnityEngine.UI; // <--- ESSA LINHA É OBRIGATÓRIA PARA A BARRINHA APARECER

public class TimeManager : MonoBehaviour
{
    [Header("Configuração de Tempo")]
    [Range(0.1f, 1f)]
    public float slowMotionFactor = 0.5f;

    [Header("Configuração de Energia")]
    public Slider timeBar;      // <--- O CAMPO QUE VAI APARECER
    public float maxTime = 5f;  
    public float regenRate = 1f; 

    private float currentTime;
    private bool isSlowMotion = false;
    private float defaultFixedDeltaTime;

    void Start()
    {
        defaultFixedDeltaTime = Time.fixedDeltaTime;
        currentTime = maxTime; 

        if (timeBar != null)
        {
            timeBar.maxValue = maxTime;
            timeBar.value = currentTime;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (isSlowMotion)
            {
                StopSlowMotion();
            }
            else if (currentTime > 0) 
            {
                StartSlowMotion();
            }
        }

        if (isSlowMotion)
        {
            currentTime -= Time.unscaledDeltaTime;

            if (currentTime <= 0)
            {
                currentTime = 0;
                StopSlowMotion(); 
            }
        }
        else
        {
            if (currentTime < maxTime)
            {
                currentTime += regenRate * Time.unscaledDeltaTime;
            }
        }

        if (timeBar != null)
        {
            timeBar.value = currentTime;
        }
    }

    void StartSlowMotion()
    {
        isSlowMotion = true;
        Time.timeScale = slowMotionFactor;
        Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;
    }

    void StopSlowMotion()
    {
        isSlowMotion = false;
        Time.timeScale = 1f;
        Time.fixedDeltaTime = defaultFixedDeltaTime;
    }
}