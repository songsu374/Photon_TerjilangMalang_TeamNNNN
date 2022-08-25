using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager instance;
    private void Awake()
    {
        instance = this;
    }

    public AudioSource clickSfx;
    public AudioSource chatSfx;
    public AudioSource winSfx;
    public AudioSource loseSfx;
    public AudioSource DrawSfx;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
