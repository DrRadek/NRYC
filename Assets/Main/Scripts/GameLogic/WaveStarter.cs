using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveStarter : MonoBehaviour
{
    [SerializeField] GameObject enemyStorage;
    [SerializeField] Wave wave;

    public void SetWave(Wave wave)
    {
        this.wave = Instantiate(wave);
    }

    public void StartWave()
    {
        StartCoroutine(Spawner());
    }

    IEnumerator Spawner()
    {
        Debug.Log(wave.enemiesToSpawn[0]);
        Debug.Log(wave.enemiesToSpawn.Sum());
        while (wave.enemiesToSpawn.Sum() > 0)
        {
            yield return new WaitForSeconds(wave.spawnRate);

            int enemyType;
            do
            {
                enemyType = Random.Range(0, wave.enemiesToSpawn.Count);
            } while (wave.enemiesToSpawn[enemyType] == 0);

            wave.enemiesToSpawn[enemyType]--;

            var instance = Instantiate(wave.enemies[enemyType]);
            instance.transform.position = new Vector3(20 * (Random.Range(0,2) * 2 - 1), 0, Random.Range(-10.0f, 10.0f));
            Debug.Log("??");
        }


    }
}
