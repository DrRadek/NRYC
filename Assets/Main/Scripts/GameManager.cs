using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour, IPrintable
{
    UnityEvent<int, string> onTextChanged = new();
    public UnityEvent<int,string> OnTextChanged { get { return onTextChanged; } }

    public int EnemiesLeft { 
        get => enemiesLeft; 
        set { 
            enemiesLeft = value;
            onTextChanged.Invoke(0,value.ToString());
            if (value == 0)
                OnWaveEnd();
        } 
    }
    public int EnemiesAlive { get => enemiesAlive; set { enemiesAlive = value; onTextChanged.Invoke(1, value.ToString()); } }

    public int CurrentWave { get => currentWave; private set { currentWave = value; onTextChanged.Invoke(2, value.ToString()); } }

    private int currentWave = 0;

    public void ManualPrintUpdate(int index)
    {
        switch(index)
        {
            case 0:
                onTextChanged.Invoke(0, EnemiesLeft.ToString());
                return;
            case 1:
                onTextChanged.Invoke(1, enemiesAlive.ToString());
                return;
            case 2:
                onTextChanged.Invoke(2, currentWave.ToString());
                return;
        }
    }

    public static GameManager instance;

    WaveStarter waveStarter;
    [SerializeField] List<Wave> waves = new();
    [SerializeField] GameObject nextWaveButton;

    public GameObject player;
    public GameObject computer;

    public bool waveInProgress = false;
    private int enemiesLeft = 0;
    private int enemiesAlive = 0;

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


    public void StartNextWave()
    {
        if (waveInProgress)
            return;

        waveStarter.SetWave(waves[0]);
        waveStarter.StartWave();
        waveInProgress = true;
        nextWaveButton.SetActive(false);
    }

    public void OnWaveEnd()
    {
        waveInProgress = false;
        CurrentWave += 1;

        waves[0].spawnRate = Mathf.Max(waves[0].spawnRate * 0.95f, 0.01f);

        waves[0].enemiesToSpawn[0] = (int)(5 + CurrentWave * CurrentWave * 1.2f); // waves[0].enemiesToSpawn[0] * 1.1f

        nextWaveButton.SetActive(true);
    }


    private void Start()
    {
        waves[0] = Instantiate(waves[0]);
        OnWaveEnd();
        //waveStarter.SetWave(waves[0]);
        //waveStarter.StartWave();
    }
}
