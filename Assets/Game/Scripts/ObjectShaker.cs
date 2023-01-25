using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ObjectShaker : MonoBehaviour
{
    public float shakeAmount = 0.1f;
    public float shakeForce = 0.5f;
    public float shakeDuration = 0.5f;

    private Vector3 initialPosition;
    private float shakeTimer;
    public Animator loliUnityChanAnim;
    public ParticleSystem[] particles;

    private void Awake()
    {
        foreach (var particle in particles)
        {
            particle.Stop();
        }
    }

    void Start()
    {
        initialPosition = transform.position;
      
    }

    void Update()
    {
        if (Input.acceleration.magnitude > shakeAmount)
        {
            shakeTimer = shakeDuration;
        }

        if (shakeTimer > 0 && UIManager.instance.IsLoadScreen)
        {
            Vector3 shakeOffset = Random.insideUnitSphere * shakeForce;
            transform.position = initialPosition + shakeOffset;
            shakeTimer -= Time.deltaTime;
            UIManager.instance.loadChargeSlide.value += 0.1f * Time.deltaTime;
            loliUnityChanAnim.SetBool("IsRun", true);
            if (!UIManager.instance.allAudioSource[3].isPlaying)
            {
            UIManager.instance.allAudioSource[3].Play();

            }
            ParticleSystemStart();
            if (UIManager.instance.loadChargeSlide.value == 1)
            {
                ParticlesStop();
                UIManager.instance.allAudioSource[3].Stop();

                UIManager.instance.LoadNextResult();
                UIManager.instance.loadChargeSlide.value = 0;
                UIManager.instance.IsLoadScreen = false;
            }



        }
        else
        {
            if (UIManager.instance.IsLoadScreen)
            {
                UIManager.instance.allAudioSource[3].Stop();

                loliUnityChanAnim.SetBool("IsRun", false);
                  ParticlesStop();
                transform.position = initialPosition;
            }
            
        }
    }

    public void ParticleSystemStart()
    {
        
            foreach (var particle in particles)
            {
            if (!particle.isPlaying)
            {
                particle.Play();

            }
            }
        
        
    }

    public void ParticlesStop()
    {
        foreach (var particle in particles)
        {
            if (particle.isPlaying)
            {
                particle.Stop();

            }
        }
    }
}
