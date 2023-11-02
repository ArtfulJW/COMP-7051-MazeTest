using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{

    /*
     * 
     * Objective(s)
     * 1. Create a 2D array of "Points"
     * - Using the given Length, v, create a map of 2(v) + 1.
     * 
     * 
     */

    [SerializeField]
    public int length;

    [SerializeField]
    GameObject testPrefab;

    // Serialized Two Dimensional Map
    List<List<GameObject>> map = new List<List<GameObject>>();

    // Start is called before the first frame update
    void Start()
    {
        generateMap();

        // Map is Successfully Serialized
        changeColorTest();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void generateMap()
    {
        
        for (int x = 0; x < (2*length) + 1; x++)
        {
            List<GameObject> tempList = new List<GameObject>();

            for (int z = 0; z < (2 * length) + 1; z++)
            {
                // Instantiate a Two-Dimensional Map of the Prefab.
                GameObject thing = Instantiate(testPrefab, new Vector3(this.transform.position.x + x, this.transform.position.y, this.transform.position.z + z),Quaternion.identity);
                tempList.Add(thing);
            }

            // Every Column List, Add to Map
            map.Add(tempList);
        }

    }

    // Testing Function
    void changeColorTest()
    {
        for (int x = 0; x < (2 * length) + 1; x++)
        {
            for (int z = 0; z < (2 * length) + 1; z++)
            {
                map[x][z].GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
                
                // This check only gathers the points that I want. (Tested)
                if (x%2 != 0 && z%2 != 0)
                {
                    map[x][z].GetComponent<Renderer>().material.color = Color.black;
                }
            }
        }
    }

}
