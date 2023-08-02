using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Wave : MonoBehaviour
{
    public float spawnRate = 0.25f;
    public List<int> enemiesToSpawn = new List<int> { 10 };
    public List<GameObject> enemies = new();
}
