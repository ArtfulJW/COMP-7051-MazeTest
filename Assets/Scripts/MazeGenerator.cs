using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazeAssignment
{
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

        // Serialized Two Dimensional Map all the Points
        List<List<Point>> map = new List<List<Point>>();

        // Serialized Two Dimensional Map of the Floors
        List<List<Point>> floor = new List<List<Point>>();

        // Start is called before the first frame update
        void Start()
        {
            generateMap();

            // Map is Successfully Serialized
            //changeColorTest();

            allocateFloors();
            changeColorFloorTest();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void generateMap()
        {
            Debug.Log("Generating Map.");
            for (int x = 0; x < (2 * length) + 1; x++)
            {
                List<Point> tempList = new List<Point>();

                for (int z = 0; z < (2 * length) + 1; z++)
                {
                    // Instantiate a Two-Dimensional Map of the Prefab.
                    //GameObject thing = Instantiate(testPrefab, new Vector3(this.transform.position.x + x, this.transform.position.y, this.transform.position.z + z), Quaternion.identity);
                    //tempList.Add(thing);

                    // "Instatiate" Point Class
                    Point point = new Point(this.transform.position.x + x, this.transform.position.y, this.transform.position.z + z);

                    // Associate a prefab to the Point class, by Instantiating it at the current Position of this Point, modified by the map index.
                    point.testPrefab = Instantiate(testPrefab, new Vector3(point.pos.x + x, point.pos.y, point.pos.z + z), Quaternion.identity);

                    // Add to tempList
                    tempList.Add(point);
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
                    map[x][z].testPrefab.GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

                    // This check only gathers the points that I want. (Tested)
                    if (x % 2 != 0 && z % 2 != 0)
                    {
                        map[x][z].testPrefab.GetComponent<Renderer>().material.color = Color.black;
                    }
                }
            }
        }

        // Testing Function.. Works
        void changeColorFloorTest()
        {
            foreach(List<Point> a in floor)
            {
                foreach(Point b in a)
                {
                    b.testPrefab.GetComponent<Renderer>().material.color = Color.black;
                }
            }
        }

        // Testing Function - detect all the Points that're "Floors"
        public void allocateFloors()
        {
            int count = 0;
            for (int x = 0; x < (2 * length) + 1; x++)
            {
                List<Point> tempList = new List<Point>();

                for (int z = 0; z < (2 * length) + 1; z++)
                {
                    if (x % 2 != 0 && z % 2 != 0)
                    { 
                        tempList.Add((Point)map[x][z]);
                        count++;
                    }
                }

                floor.Add(tempList);

            }
            Debug.Log("Number of Floors:" + count);
        }


        // Helper Function - Set the all the Walls
        public void setWalls()
        {

        }

    }
}
