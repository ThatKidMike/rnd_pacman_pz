using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillsSpawn : MonoBehaviour {

    public GameObject pellet;
    public GameObject bigPill;
    public List<KeyValuePair<int, int>> coordinates = new List<KeyValuePair<int, int>>();
    public GameObject node;
    public int[,] xy = new int[100, 100];
    public int amount = 0;

    private GameObject tmpValidatedPlaces;
    public GameObject pills;

    private int[,] validCoordinates = new int[100, 100];
    private int[,] tmpCoordinates = new int[100, 100];
    private System.Random gen = new System.Random();
    private int rndCount = 0;
    private GameObject tmpBorder;
    private int[,] tmpPillsAtBorder = new int[100, 100];


    // Use this for initialization
    void Start () {

        tmpValidatedPlaces = GameObject.Find("nodes");
        Transform[] allChildren = tmpValidatedPlaces.GetComponentsInChildren<Transform>();

        foreach (Transform child in allChildren) {
            if (!(child.localPosition.x == 0 && child.localPosition.y == 0))
                validCoordinates[(int)(child.localPosition.x + 30), (int)(child.localPosition.y + 30)] = 1;

            //if(child.localPosition.x == 8 && child.localPosition.y == 10)
            //Debug.Log(child.localPosition.x + " :: " + child.localPosition.y);


        }

        tmpBorder = GameObject.Find("InitialPills");
        Transform[] Children = tmpBorder.GetComponentsInChildren<Transform>();

        foreach (Transform child in Children) {
            if (!(child.localPosition.x == 0 && child.localPosition.y == 0))
                tmpPillsAtBorder[(int)(child.localPosition.x + 30), (int)(child.localPosition.y + 30)] = 1;

           //Debug.Log(child.localPosition.x + " :: " + child.localPosition.y);
        }

        rndGen();
        spawnInitalPills();
        instantiateRnd();
        spawnNodes();
       		
	}

    enum directions {
        UP, DOWN, LEFT, RIGHT
    };

    private List<E> ShuffleList<E>(List<E> inputList) {

        List<E> randomList = new List<E>();

        System.Random r = new System.Random();
        int randomIndex = 0;
        while (inputList.Count > 0) {
            randomIndex = r.Next(0, inputList.Count);
            randomList.Add(inputList[randomIndex]);
            inputList.RemoveAt(randomIndex);
        }

        return randomList;
    }

    public int[] generateRndDirs() {

        List<int> list = new List<int>();

        for (int i = 0; i < 4; i++) {
            list.Add(i);
        }

        list = ShuffleList(list);

        return list.ToArray();

    }

    void generateMaze(int x, int y) {

        int[] rndDir = generateRndDirs();

        if (amount < 75) {

            for (int i = 0; i < 4; i++) {

                switch ((directions)Convert.ToInt32(i)) {

                    case directions.UP:
                        if (validCoordinates[x, y + 2] == 0)
                            continue;
                        if (validCoordinates[x, y + 2] == 1) {
                            for (int j = 1; j <= 2; j++) {
                                tmpCoordinates[x, y + j] = 1;
                                validCoordinates[x, y + j] = 0;                               
                            }
                            generateMaze(x, y + 2);
                        }
                        break;

                    case directions.DOWN:
                        if (validCoordinates[x, y - 2] == 0)
                            continue;
                        if (validCoordinates[x, y - 2] == 1) {
                            for (int h = 1; h <= 2; h++) {
                                tmpCoordinates[x, y - h] = 1;
                                validCoordinates[x, y - h] = 0;
                            }
                            generateMaze(x, y - 2);
                        }
                        break;

                    case directions.LEFT:
                        if (validCoordinates[x - 2, y] == 0)
                            continue;
                        if (validCoordinates[x - 2, y] == 1) {
                            for (int g = 1; g <= 2; g++) {
                                tmpCoordinates[x - g, y] = 1;
                                validCoordinates[x - g, y] = 0;
                            }
                            generateMaze(x - 2, y);
                        }
                        break;

                    case directions.RIGHT:
                        if (validCoordinates[x + 2, y] == 0) {
                            continue;
                        }
                        if (validCoordinates[x + 2, y] == 1) {
                            for (int f = 1; f <= 2; f++) {
                                tmpCoordinates[x + f, y] = 1;
                                validCoordinates[x + f, y] = 0;
                            }

                            generateMaze(x + 2, y);
                        }
                        break;
                        
                }


            }

             rndGen();

        }

        

    }

    void instantiateRnd() {

        for (int i = 0; i < 100; i++) {
            for (int j = 0; j < 100; j++) {

                if(tmpPillsAtBorder[i, j] == 1 && tmpCoordinates[i, j] == 0) {
                    tmpCoordinates[i, j] = 1;
                }

                if (tmpCoordinates[i, j] == 1) {

                    if ((i == 19 && j == 21) || (i == 44 && j == 21) || (i == 44 && j == 47) || (i == 19 && j == 47)) {

                        Instantiate(bigPill, new Vector2((i - 30), (j - 30)), Quaternion.identity);
                        xy[i, j] = 1;
                        amount++;

                    } else {

                        Instantiate(pellet, new Vector2((i - 30), (j - 30)), Quaternion.identity);
                        xy[i, j] = 1;
                        amount++;

                        }

                    }

                }

            }
    }

    void rndGen() {

        int tmpX = gen.Next(0, 99);
        int tmpY = gen.Next(0, 99);

        if (validCoordinates[tmpX, tmpY] == 1) {
            tmpCoordinates[tmpX, tmpY] = 1;
            validCoordinates[tmpX, tmpY] = 0;
            generateMaze(tmpX, tmpY);
        } else if (rndCount < 50) {
            rndCount++;
            rndGen();
        }


    }

    void spawnInitalPills() {

        for (int i = 0; i < 100; i++) {
            for (int j = 0; j < 100; j++) {

                if(validCoordinates[i, j] == 1 && tmpCoordinates[i, j] == 0) {

                    if (tmpCoordinates[i + 1, j] == 1 && tmpCoordinates[i - 1, j] == 1) {

                        tmpCoordinates[i, j] = 1;
                        validCoordinates[i, j] = 0;

                    }

                    if(tmpCoordinates[i, j + 1] == 1 && tmpCoordinates[i, j - 1] == 1) {

                        tmpCoordinates[i, j] = 1;
                        validCoordinates[i, j] = 0;

                    }

                }

            }
        }

    }

    // Update is called once per frame
    void Update () { 

       

    }

    void spawnPills() {

        coordinates.Add(new KeyValuePair<int, int>(15, 4));
        coordinates.Add(new KeyValuePair<int, int>(16, 4));
        coordinates.Add(new KeyValuePair<int, int>(17, 4));
        coordinates.Add(new KeyValuePair<int, int>(-12, 4));
        coordinates.Add(new KeyValuePair<int, int>(-13, 4));
        coordinates.Add(new KeyValuePair<int, int>(-14, 4));
        coordinates.Add(new KeyValuePair<int, int>(-1, -5));
        coordinates.Add(new KeyValuePair<int, int>(0, -5));
        coordinates.Add(new KeyValuePair<int, int>(1, -5));
        coordinates.Add(new KeyValuePair<int, int>(2, -5));

        for (int j = 0; j < 10; j++) {
            for (int i = 0; i < 26; i++) {
                if (!Physics2D.OverlapCircle(new Vector2(-11 + i, -11 + j), (float)0.2)) {
                    if ((i == 0) && (j == 6)) {
                        Instantiate(bigPill, new Vector2(-11 + i, -11 + j), Quaternion.identity);
                        amount++;
                        coordinates.Add(new KeyValuePair<int, int>(-11 + i, -11 + j));
                    } else if ((i == 25) && (j == 6)) {
                        Instantiate(bigPill, new Vector2(-11 + i, -11 + j), Quaternion.identity);
                        amount++;
                        coordinates.Add(new KeyValuePair<int, int>(-11 + i, -11 + j));
                    } else {
                        Instantiate(pellet, new Vector2(-11 + i, -11 + j), Quaternion.identity);
                        amount++;
                        coordinates.Add(new KeyValuePair<int, int>(-11 + i, -11 + j));
                    }
                }
            }
        }

        for(int j = 10; j < 15; j++) {
            for (int i = 5; i < 21; i++) {
                if (!Physics2D.OverlapCircle(new Vector2(-11 + i, -11 + j), (float)0.2)) {
                    Instantiate(pellet, new Vector2(-11 + i, -11 + j), Quaternion.identity);
                    amount++;
                    coordinates.Add(new KeyValuePair<int, int>(-11 + i, -11 + j));
                }
            }
        }

        for(int i = 0; i < 9; i++) {
            if (!Physics2D.OverlapCircle(new Vector2(-11 + i, 4), (float)0.2)) {
                Instantiate(pellet, new Vector2(-11 + i, 4), Quaternion.identity);
                amount++;
                coordinates.Add(new KeyValuePair<int, int>(-11 + i, 4));
            }
        }

        for (int i = 17; i < 26; i++) {
            if (!Physics2D.OverlapCircle(new Vector2(-11 + i, 4), (float)0.2)) {
                Instantiate(pellet, new Vector2(-11 + i, 4), Quaternion.identity);
                amount++;
                coordinates.Add(new KeyValuePair<int, int>(-11 + i, 4));
            }
        }

        for (int j = 16; j < 20; j++) {
            for (int i = 5; i < 21; i++) {
                if (!Physics2D.OverlapCircle(new Vector2(-11 + i, -11 + j), (float)0.2)) {
                    Instantiate(pellet, new Vector2(-11 + i, -11 + j), Quaternion.identity);
                    amount++;
                    coordinates.Add(new KeyValuePair<int, int>(-11 + i, -11 + j));
                }
            }
        }

        for (int j = 20; j < 31; j++) {
            for (int i = 0; i < 26; i++) {
                if (!Physics2D.OverlapCircle(new Vector2(-11 + i, -11 + j), (float)0.2)) {
                    if ((i == 0) && (j == 28)) {
                        Instantiate(bigPill, new Vector2(-11 + i, -11 + j), Quaternion.identity);
                        amount++;
                        coordinates.Add(new KeyValuePair<int, int>(-11 + i, -11 + j));
                    } else if ((i == 25) && (j == 28)) {
                        Instantiate(bigPill, new Vector2(-11 + i, -11 + j), Quaternion.identity);
                        amount++;
                        coordinates.Add(new KeyValuePair<int, int>(-11 + i, -11 + j));
                    } else {
                        Instantiate(pellet, new Vector2(-11 + i, -11 + j), Quaternion.identity);
                        amount++;
                        coordinates.Add(new KeyValuePair<int, int>(-11 + i, -11 + j));
                    }
                }
            }
        }

    }

    void spawnNodes() {

        xy[46, 34] = 1;
        xy[17, 34] = 1;
        xy[47, 34] = 1;
        xy[16, 34] = 1;



    }

}
