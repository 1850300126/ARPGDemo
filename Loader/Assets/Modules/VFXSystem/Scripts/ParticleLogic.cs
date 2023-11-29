using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ParticleLogic : MonoBehaviour
{   
    public  ParticleSystem particle;
    private ParticleSystem.MainModule mainModule;
 
    private void Start()
    {   
        particle = GetComponent<ParticleSystem>();
        mainModule = particle.main;   //获取MainModule
        mainModule.stopAction = ParticleSystemStopAction.Callback;  //设置结束时调用回调
    }
 
    void OnParticleSystemStopped()
    {
        Destroy(particle.gameObject);
    }
}
