using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnWaveController : MonoBehaviour
{    
    [SerializeField] private int difficulty, startingBudget, curBudget, waveNum, wavesPerDifficulty;
    [SerializeField] private Spawnable[] spawnables;
    [SerializeField] private SpawnPredictorController spawnPredictor;
    [SerializeField]private List<SpawnPos> totalSpawnPositions, validSpawns;
    private List<Spawnable> validSpawnables;
    private SpawnPredictorController[] spawnPredictors;
    [SerializeField]private int livingEnemies;

    private void OnEnable()
    {
        SpawnLocationController.OnSpawnLocationReady += HandleSpawnLocation;
        totalSpawnPositions = new List<SpawnPos>();
        validSpawns = new List<SpawnPos>();
        validSpawnables = new List<Spawnable>();
        EnemyHP.OnEnemyDeath += HandleEnemyDeath;
        
    }



    private void OnDisable()
    {
        SpawnLocationController.OnSpawnLocationReady -= HandleSpawnLocation;
        EnemyHP.OnEnemyDeath -= HandleEnemyDeath;
    }

    private void HandleEnemyDeath(Vector3 pos)
    {
        livingEnemies--;
        if(livingEnemies <= 0)
        {
            EndCurrentWave();
        }
    }

    private void Start()
    {
        CreateSpawnPredictors();
        GetBudget();
        StartCoroutine(WaitAndSpawnWave(.5f, difficulty, curBudget));
    }

    private void EndCurrentWave()
    {
        Debug.Log("ending wave");
        waveNum++;
        if(waveNum >= wavesPerDifficulty)
        {
            waveNum = 0;
            difficulty++;            
        }
        GetBudget();
        StartCoroutine(WaitAndSpawnWave(.5f, difficulty, curBudget));
    }

    private void HandleSpawnLocation(Vector3 pos, Vector3 offset)
    {
        SpawnPos s = new SpawnPos(pos, offset);
        totalSpawnPositions.Add(s);
    }

    private IEnumerator WaitAndSpawnWave(float waitTime, int difficulty, int budget)
    {
        WaitForSeconds w = new WaitForSeconds(waitTime);
        yield return w;
        validSpawnables.Clear();
        for(int i = 0; i < spawnables.Length; i++)
        {
            if(spawnables[i].difficulty <= difficulty)
            {
                validSpawnables.Add(spawnables[i]);
            }
        }
        GetValidSpawns(totalSpawnPositions);
        while (budget > 0)
        {
           // Debug.Log("size before getting random number: " + validSpawns.Count);
            int ranNum = Random.Range(0, validSpawns.Count);
            budget -= SpawnEnemy(difficulty, budget, validSpawns[ranNum]);            
            validSpawns.RemoveAt(ranNum);
           // Debug.Log("new size of valid spawns: " + validSpawns.Count);
            //Debug.Break();
            if (validSpawns.Count < 1)
            {
               // Debug.Log("resetting spawns");
               // ResetSpawns(totalSpawnPositions);
                GetValidSpawns(totalSpawnPositions);
               // Debug.Log("new spawn list size: " + validSpawns.Count);
            }
        }
            
    }



    private void GetBudget()
    {
        float newBudget = startingBudget * (difficulty + 1) ;
        Debug.Log("newbudget: " + newBudget);
        newBudget += (waveNum-1 * .1f);
        curBudget = (int)newBudget;
        Debug.Log("final budget: " + curBudget);
    }

    private int SpawnEnemy(int difficulty, int budget, SpawnPos pos) // and spawns valid enemy at spawnPos with random offset, returns cost of the spawn
    {
        int ranSpawnable = Random.Range(0, validSpawnables.Count);
        for(int i = 0; i < validSpawnables[ranSpawnable].enemies.Length; i++)
        {
            SpawnPredictorController g = GetSpawnPredictor();
            float ranX = Random.Range(-pos.offset.x, pos.offset.x);
            float ranZ = Random.Range(-pos.offset.z, pos.offset.z);
            g.transform.position = pos.pos;
            g.transform.position += Vector3.right * ranX;
            g.transform.position += Vector3.forward * ranZ;
            g.SetSpawnedObject(validSpawnables[ranSpawnable].enemies[i]);
            g.gameObject.SetActive(true);
            livingEnemies++;
        }
        return validSpawnables[ranSpawnable].tokenCost;
    }

    private void GetValidSpawns(List<SpawnPos> spawns)
    {
       // validSpawns.Clear();
        for (int i = 0; i < spawns.Count; i++)
        {
            validSpawns.Add(spawns[i]);
            /*  if (!spawns[i].spawned)
              {
                  validSpawns.Add(spawns[i]);
              }*/
        }
       /* if (validSpawns.Count < 1)
        {
            ResetSpawns(spawns);
            for (int i = 0; i < spawns.Count; i++)
            {
                if (!spawns[i].spawned)
                {
                    validSpawns.Add(spawns[i]);
                }
            }
        }*/
        
    }

   /* private void ResetSpawns(List<SpawnPos> s)
    {
        for(int i = 0; i < s.Count; i++)
        {
            s[i].Reset();
        }
    }*/

    private struct SpawnPos
    {
        public Vector3 pos;
        public Vector3 offset;
        public bool spawned;
        
        public SpawnPos(Vector3 newPos, Vector3 newOffset)
        {
            this.pos = newPos;
            this.offset = newOffset;
            this.spawned = false;
        }

      /*  public void Reset()
        {
            this.spawned = false;
        }*/
    }

    private void CreateSpawnPredictors()
    {
        spawnPredictors = new SpawnPredictorController[GameController.instance.maxEnemies];
        for(int i = 0; i < spawnPredictors.Length; i++)
        {
            GameObject g = Instantiate(spawnPredictor.gameObject, this.transform);
            spawnPredictors[i] = g.GetComponent<SpawnPredictorController>();
        }
    }

    private SpawnPredictorController GetSpawnPredictor()
    {
        for(int i = 0; i < spawnPredictors.Length; i++)
        {
            if(!spawnPredictors[i].isActiveAndEnabled)
            {
                return spawnPredictors[i];
            }
        }
        return null;
    }


}
