using UnityEngine;
using Unity.Cinemachine;

public class CinemachineShake : MonoBehaviour
{
    public static CinemachineShake Instance {get; private set;}
    public CinemachineCamera cinemachineCam;
    public float amplitude_botao = 1f;
    public float frequency_botao = 7f;
    public float time_botao = 0f;
    private float shakeTimer;
    private float startingIntensity;
    private float shakeTimeTotal;
    private GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake() {
        cinemachineCam = GetComponent<CinemachineCamera>();
    }
    public void ShakeCamera(float amplitude, float frequency, float time)
    {
        CinemachineBasicMultiChannelPerlin multiChannelPerlin = cinemachineCam.GetComponent<CinemachineBasicMultiChannelPerlin>();
        multiChannelPerlin.AmplitudeGain = amplitude;
        multiChannelPerlin.FrequencyGain = frequency;
        shakeTimer = time;
        startingIntensity = amplitude;
        shakeTimeTotal = time;
    }
    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.I))
        {
            amplitude_botao += .1f;
        } else if (Input.GetKeyDown(KeyCode.U))
        {
            frequency_botao += .1f;
        } else if (Input.GetKeyDown(KeyCode.K))
        {
            amplitude_botao -= .1f;
        } else if (Input.GetKeyDown(KeyCode.J))
        {
            frequency_botao -= .1f;
        }
        ShakeCamera(amplitude_botao, frequency_botao, .07f);
        */
        if (shakeTimer > 0)
        {    
            shakeTimer -= Time.deltaTime;
            // Timer over!
            CinemachineBasicMultiChannelPerlin multiChannelPerlin = cinemachineCam.GetComponent<CinemachineBasicMultiChannelPerlin>();
            multiChannelPerlin.AmplitudeGain = Mathf.Lerp(startingIntensity, 0f, 1-(shakeTimer/shakeTimeTotal));
        }
    }
}
