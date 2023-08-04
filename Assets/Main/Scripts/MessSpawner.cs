using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessSpawner : MonoBehaviour
{
    [SerializeField] float messToSpawn = 50;
    [SerializeField] GameObject messStorage;
    [SerializeField] List<GameObject> spawnList = new List<GameObject>();
    [SerializeField] List<BoxCollider> spawnAreas;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < messToSpawn; i++)
        {
            var instance = Instantiate(GameManager.GetRandomObjectFromList(spawnList), messStorage.transform);
            do
            {
                instance.transform.position = GameManager.GetRandomPointInBoxList(spawnAreas);
            } while (instance.transform.position.magnitude < 18);
            

        }
    }
}
