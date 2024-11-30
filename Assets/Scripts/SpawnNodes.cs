using UnityEngine;

public class SpawnNodes : MonoBehaviour
{
    int numToSpawn = 28;
    public float currentSpawnOffSet;
    public float spawnOffSet = 0.3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        /*if (gameObject.name == "Node")
        {
            currentSpawnOffSet = spawnOffSet;
            for (int i = 0; i < numToSpawn; i++)
            {
                //Clone a new node
                GameObject clone = Instantiate(gameObject, new Vector3(transform.position.x, transform.position.y + currentSpawnOffSet, 0), Quaternion.identity);

                currentSpawnOffSet += spawnOffSet;
            }
        }*/

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
