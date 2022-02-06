using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPredictorController : MonoBehaviour
{
    [SerializeField] private Transform displayMesh;
    private GameObject spawnedObject;
    private float spawnSpeed = .5f;

    private void OnEnable()
    {
        StartCoroutine(StartSpawnAnim());
    }

    private IEnumerator StartSpawnAnim()
    {
        float t = 1;
        Vector3 startSize = displayMesh.localScale;       
        while (t > 0)
        {
            displayMesh.localScale = Vector3.Lerp(  startSize, Vector3.zero, t);
            t -= Time.deltaTime * spawnSpeed ;
            yield return null;
        }
        if(spawnedObject != null)
        {
            GameObject newObject = Instantiate(spawnedObject);
            newObject.transform.position = transform.position;
            
        }
        this.gameObject.SetActive(false);
    }

    public void SetSpawnedObject(GameObject g)
    {
        spawnedObject = g;
    }


}
