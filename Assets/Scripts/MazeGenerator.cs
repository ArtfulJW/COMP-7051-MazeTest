using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;

namespace MazeAssignment
{
    public class MazeGenerator : MonoBehaviour
    {

        [SerializeField]
        public int length;

        [SerializeField]
        GameObject testPrefab;

        // Serialized Two Dimensional Map all the Points
        List<List<Point>> map = new List<List<Point>>();

        // Serialized Two Dimensional Map of the Floors
        List<List<Point>> floor = new List<List<Point>>();

        enum CardinalDirection { North = 0, South = 1, East = 2, West = 3};

        List<Point> MST = new List<Point>();
 
        // Start is called before the first frame update
        void Start()
        {
            generateMap();

            // Map is Successfully Serialized
            //changeColorTest();

            allocateFloors();
            changeColorFloorTest();

            // Test Reset Edge weights
            resetWeightsDirectedAtEdge();

            // Initial Test
            PrimAlgo(floor[0][0]);
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void OnDrawGizmos()
        {
            //debugFloor();
        }

        void generateMap()
        {
            UnityEngine.Debug.Log("Generating Map.");
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
                    point.testPrefab = Instantiate(testPrefab, new Vector3(point.pos.x, point.pos.y, point.pos.z), Quaternion.identity);

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
                    map[x][z].testPrefab.GetComponent<Renderer>().material.color = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

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
            
            //for (int x = 0; x < floor.Count; x++)
            //{
            //    Debug.Log("Sizeof List in List: " + x + " = " + floor[x].Count);
            //}

            floor[0][0].testPrefab.GetComponent<Renderer>().material.color = Color.blue;
            floor[length-1][length-1].testPrefab.GetComponent<Renderer>().material.color = Color.red;
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
                        map[x][z].initMapPointer(x, z);
                        tempList.Add((Point)map[x][z]);
                        count++;
                    }
                }

                if (tempList.Count > 0)
                {
                    floor.Add(tempList);
                    for (int i = 0; i < floor.Count; i++)
                    {
                        for (int j = 0; j < floor[i].Count; j++)
                        {
                            //Debug.Log("X: " + x + " Z: " + i);
                            floor[i][j].initFloorPointer(i, j);
                        }
                    }
                }

            }
            UnityEngine.Debug.Log("Number of Floors:" + count);
        }

        /* Prim Algo
         * Setup: Initialize a MST List<Point>.
         * 1. Get Point.MapPointer coordinates
         * 2. Find the smallest VALID weight.
         *      - Check if weight is greater than 0
         * 3. Destroy the intermediary Wall between the two points
         *      - Add THAT point to MST.
         * 4. Add Adjacent Point (in the direction of the Cardinal Direction) to MST
        */
        public void PrimAlgo(Point inputPoint)
        {
            foreach (List<Point> a in floor)
            {
                foreach (Point b in a)
                {
                    Destroy(getIntermediatePoint(b, getLowestWeightDirection(b)).testPrefab);
                }
            }

            //MST.Add(inputPoint);

            //string direction = getLowestWeightDirection(inputPoint);
            //Destroy(getIntermediatePoint(inputPoint, direction).testPrefab);

            //Point a = getAdjacentPoint(inputPoint, direction);

            //while (MST.Contains(a))
            //{
            //    a = getAdjacentPoint(inputPoint, direction);
            //}

            //if (map[map.Count - 1][map[0].Count - 1] != a)
            //{
            //    PrimAlgo(a);
            //}

        }

        /*Helper Functions for access the Points in the Cardinal Directions
         * 1. Get Point.MapPointer coordinates
         * 2. Return Intermediary Point
        */
        public Point getIntermediatePoint(Point inputPoint, string Direction)
        {
            switch (Direction)
            {
                case "North":
                    return map[inputPoint.getMapPointerX()][inputPoint.getMapPointerZ()+1];
                case "South":
                    return map[inputPoint.getMapPointerX()][inputPoint.getMapPointerZ()-1];
                case "East":
                    return map[inputPoint.getMapPointerX() + 1][inputPoint.getMapPointerZ()];
                case "West":
                    return map[inputPoint.getMapPointerX() - 1][inputPoint.getMapPointerZ()];
            }

            // Something went wrong
            return null;
        }

        /*Helper Functions for access the Points in the Cardinal Directions
         * 1. Get Point.MapPointer coordinates
         * 2. Return Adjacent Point
        */
        public Point getAdjacentPoint(Point inputPoint, string Direction)
        {
            switch (Direction)
            {
                case "North":
                    return floor[inputPoint.getFloorPointerX()][inputPoint.getFloorPointerZ() + 1];
                case "South":
                    return floor[inputPoint.getFloorPointerX()][inputPoint.getFloorPointerZ() - 1];
                case "East":
                    return floor[inputPoint.getFloorPointerX() + 1][inputPoint.getFloorPointerZ()];
                case "West":
                    return floor[inputPoint.getFloorPointerX() - 1][inputPoint.getFloorPointerZ()];
            }

            // Something went wrong
            return null;
        }

        public string getLowestWeightDirection(Point inputPoint)
        {
            int lowest = 10000;
            int indexLowest = 0;
            for (int x = 0; x < inputPoint.cardinalDirection.Count; x++)
            {
                if (inputPoint.cardinalDirection[x] < lowest && inputPoint.cardinalDirection[x] >= 1 )
                {
                    indexLowest = x;
                    lowest = inputPoint.cardinalDirection[x];
                }
            }
            UnityEngine.Debug.Log("LOWEST: " + lowest);
            switch (indexLowest)
            {
                case 0:
                    return "North";
                case 1:
                    return "South";
                case 2:
                    return "East";
                case 3:
                    return "West";
            }

            // Something went Wrong
            return null;
        }

        // Helper Function - reset all weights directed towards the edges
        public void resetWeightsDirectedAtEdge()
        {
            for (int x = 0; x < floor.Count; x++)
            {
                for (int z = 0; z < floor[x].Count; z++)
                {
                    if (x == 0)
                    {
                        floor[x][z].setWest(-1);
                    }
                    if (z == 0)
                    {
                        floor[x][z].setSouth(-1);
                    }
                    if (x == floor[x].Count - 1)
                    {
                        floor[x][z].setEast(-1);
                    }
                    if (z == floor[z].Count - 1)
                    {
                        //floor[x][z].testPrefab.GetComponent<Renderer>().material.color = Color.yellow;
                        floor[x][z].setNorth(-1);
                    }
                }
            }
        }

        public void debugFloor()
        {
            foreach(List<Point> a in floor) 
            {
                foreach(Point b in a)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawSphere(b.pos, 1);
                }
            }
        }

        // Helper Function - Set the all the Walls
        public void setWalls()
        {

        }

    }
}
