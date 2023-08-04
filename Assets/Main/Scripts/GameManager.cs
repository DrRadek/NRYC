using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, IPrintable
{
    readonly UnityEvent<GameObject, int, string> onTextChanged = new();
    public UnityEvent<GameObject, int, string> OnTextChanged { get { return onTextChanged; } }

    public int EnemiesLeft {
        get => enemiesLeft;
        set {
            enemiesLeft = value;
            onTextChanged.Invoke(gameObject, 0, value.ToString());
            if (value == 0)
                OnWaveEnd();
        }
    }
    public int EnemiesAlive { get => enemiesAlive; set { enemiesAlive = value; onTextChanged.Invoke(gameObject, 1, value.ToString()); } }

    public int CurrentWave { get => currentWave; private set { currentWave = value; onTextChanged.Invoke(gameObject, 2, value.ToString()); } }

    public bool IsActivePopup { get => isActivePopup; set => isActivePopup = value; }

    private int currentWave = 0;
    [SerializeField] private int targetWave = 11;

    public void ManualPrintUpdate()
    {
        onTextChanged.Invoke(gameObject, 0, EnemiesLeft.ToString());
        onTextChanged.Invoke(gameObject, 1, enemiesAlive.ToString());
        onTextChanged.Invoke(gameObject, 2, currentWave.ToString());
        onTextChanged.Invoke(gameObject, 3, (targetWave - 1).ToString());
    }

    public static GameManager instance;

    WaveStarter waveStarter;
    [SerializeField] List<Wave> waves = new();
    [SerializeField] GameObject nextWaveButton;

    public PlayerController player;
    public GameObject computer;
    public Shotgun shotgun;

    public bool waveInProgress = false;

    private bool isActivePopup = false;
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

        if (currentWave == 1)
            AudioManager.instance.PlayWave();

        waveStarter.SetWave(waves[0]);
        waveStarter.StartWave();
        waveInProgress = true;
        nextWaveButton.SetActive(false);
    }

    int lastEnemiesToSpawn = 0;
    bool useLastEnemiesToSpawn = false;

    public void OnWaveEnd()
    {
        waveInProgress = false;
        CurrentWave += 1;

        waves[0].spawnRate = Mathf.Max(waves[0].spawnRate * 0.75f, 0.0001f);

        

        if (CurrentWave == targetWave && StoryManager.instance.StoryProgression <= StoryManager.Story.wave)
        {
            AudioManager.instance.StopWave();
            StoryManager.instance.ActivateEndSequence();
            
        }
        else
        {
            nextWaveButton.SetActive(true);
        }

        if (CurrentWave == targetWave - 1)
        {
            lastEnemiesToSpawn = (int)(2 + CurrentWave * CurrentWave * 1.2f);
            waves[0].enemiesToSpawn[0] = 0;
            waves[0].enemiesToSpawn[1] = 1;
            useLastEnemiesToSpawn = true;
        }
        else
        {
            if (useLastEnemiesToSpawn)
            {
                useLastEnemiesToSpawn = false;
                waves[0].enemiesToSpawn[0] = lastEnemiesToSpawn;
                waves[0].enemiesToSpawn[1] = 1;
            }
            else
            {
                waves[0].enemiesToSpawn[0] = (int)(2 + CurrentWave * CurrentWave * 1.2f);
            }
        }
    }

    public void OnGameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

    private void Start()
    {
        waves[0] = Instantiate(waves[0]);
        OnWaveEnd();
    }

    public static void EndGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#elif UNITY_WEBGL
    SceneManager.LoadScene("WebGL");
#else
    Application.Quit();
#endif
    }

    public static void NewGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public static Vector3 GetRandomPointInBounds(BoxCollider collider)
    {
        var bounds = collider.bounds;
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }

    public static Vector3 GetRandomPointInBoxList(List<BoxCollider> colliders)
    {
        return GetRandomPointInBounds(colliders[Random.Range(0, colliders.Count)]);
    }


    public static T GetRandomObjectFromList<T>(List<T> list){
        return list[Random.Range(0,list.Count)];
    }
}
