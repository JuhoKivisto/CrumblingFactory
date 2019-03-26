using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{

    public int controllerId;

    public SteamVR_TrackedController svrc;

    [Range(0f, 4f)]
    [SerializeField]
    private float hapticPulse;

    public float HapticPulse
    {
        get
        {
            return hapticPulse;
        }

        set
        {
            hapticPulse = value;
        }
    }
    private bool triggerPressed;

    public bool TriggerPressed
    {
        get
        {
            return triggerPressed;
        }

        set
        {
            triggerPressed = value;
        }
    }

    private void OnEnable()
    {
        InitController();
    }

    private void Update()
    {
        if (svrc == null)
        {
            return;
        }


    }

    public void InitController()
    {
        svrc = GetComponent<SteamVR_TrackedController>();
    }

    public void EnableHapticFeedBack()
    {

        SteamVR_Controller.Input((int)svrc.controllerIndex).TriggerHapticPulse((ushort)(HapticPulse * 1000));

    }

    public void EnableHapticFeedBack(float pulseStrengh)
    {

        SteamVR_Controller.Input((int)svrc.controllerIndex).TriggerHapticPulse((ushort)(pulseStrengh * 1000));

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pulseInterval">Time between pulses</param>
    /// <param name="pulseStrengh">Individual pulses strengh</param>
    /// <param name="pulseTime">How long haptic feedback is active</param>
    public void EnableHapticFeedBackLoop(float pulseInterval, float pulseStrengh, float pulseTime)
    {        
        
        StartCoroutine(HapticFeedBackLoop(pulseInterval, pulseStrengh, pulseTime));        
    }

    public IEnumerator HapticFeedBackLoop(float pulseInterval, float pulseStrengh, float pulseTime)
    {

        float timer = 0;
        float startTime = 0;
        while (timer < pulseTime)
        {                     
            if (timer - startTime > pulseInterval)
            {
                EnableHapticFeedBack(pulseStrengh);
                startTime = timer;
            }

            timer += Time.deltaTime;
            yield return null;
        }        
    }
}
