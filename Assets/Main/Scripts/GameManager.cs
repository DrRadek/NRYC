using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    WaveStarter waveStarter;
    [SerializeField] List<Wave> waves = new();
    public GameObject player;
    public GameObject computer;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }
            
        instance = this;

        waveStarter = GetComponent<WaveStarter>();
    }

    private void Start()
    {
        waveStarter.SetWave(waves[0]);
        waveStarter.StartWave();
    }
}
