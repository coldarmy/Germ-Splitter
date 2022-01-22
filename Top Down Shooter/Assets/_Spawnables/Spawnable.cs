using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Spawnable", menuName = "Shooter/Spawnables", order = 1)]
public class Spawnable : ScriptableObject
{
    public int difficulty, tokenCost;
    public GameObject[] enemies;

}
