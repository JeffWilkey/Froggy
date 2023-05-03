using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject homePrefab;
    // Start is called before the first frame update
    void Start()
    {
        SpawnHomes();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnHomes()
    {
        for (float i = 0f; i < 12f; i+=2f)
        {

            Instantiate(homePrefab, new Vector3(homePrefab.transform.position.x + i, homePrefab.transform.position.y, 0), Quaternion.identity);
        }
    }
}
