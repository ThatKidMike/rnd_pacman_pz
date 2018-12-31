using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class RandomBoxSpawner : MonoBehaviour {

    public GameObject blackBox;
    private GameObject tmpValidatedPlaces;
    private int[,] spawnCoordinates = new int[100, 100];
    private int[,] borderCoordinates = new int[100, 100];

    public GameObject one_closing;
    public GameObject one_left_right;
    public GameObject single;
    public GameObject single_border;
    public GameObject single_corner;
    public GameObject filler;

    // Use this for initialization
    void Start() {


        generateBoxes();
        generateGraphics();

    }
	
	// Update is called once per frame
	void Update () {
		


	}



    void generateBoxes() {

        for (int i = 0; i < 26; i++) {
            for(int j = 0; j < 31; j++) {

                if(!Physics2D.OverlapBox(new Vector2(-11.5f + i, 19.5f - j ),(blackBox.GetComponent<Renderer>().bounds.size), 0)) {

                    if(!(-11.5f + i == 1.5f && 19.5f - j == 4.5f))
                    spawnCoordinates[i,j] = 1;

                }

            }
        }

        for (int i = 0; i < 26; i++) {
            for (int j = 0; j < 31; j++) {

                    if (spawnCoordinates[i, j] == 1) {

                        Instantiate(blackBox, new Vector2(-11.5f + i, 19.5f - j), Quaternion.identity);

                    }

            }
        }


    }

    void generateGraphics() {

        for (int i = 0; i < 26; i++) {
            for (int j = 0; j < 31; j++) {

                if (spawnCoordinates[i, j] == 1) {
                    //One_Closers
                    if (spawnCoordinates[i - 1, j] == 0 && spawnCoordinates[i + 1, j] == 1
                        && spawnCoordinates[i, j + 1] == 0 && spawnCoordinates[i, j - 1] == 0) {

                        Instantiate(one_closing, new Vector3(-11.5f + i, 19.5f - j, -1), Quaternion.identity);
                        borderCoordinates[i, j] = 1;

                    } else if (spawnCoordinates[i + 1, j] == 0 && spawnCoordinates[i - 1, j] == 1
                        && spawnCoordinates[i, j + 1] == 0 && spawnCoordinates[i, j - 1] == 0) {

                        Instantiate(one_closing, new Vector3(-11.5f + i, 19.5f - j, -1), Quaternion.Euler(0, 0, 180));
                        borderCoordinates[i, j] = 1;

                    } else if (spawnCoordinates[i, j + 1] == 0 && spawnCoordinates[i, j - 1] == 1
                        && spawnCoordinates[i + 1, j] == 0 && spawnCoordinates[i - 1, j] == 0) {

                        Instantiate(one_closing, new Vector3(-11.5f + i, 19.5f - j, -1), Quaternion.Euler(0, 0, 90));
                        borderCoordinates[i, j] = 1;

                    } else if (spawnCoordinates[i, j + 1] == 1 && spawnCoordinates[i, j - 1] == 0
                        && spawnCoordinates[i + 1, j] == 0 && spawnCoordinates[i - 1, j] == 0) {

                        Instantiate(one_closing, new Vector3(-11.5f + i, 19.5f - j, -1), Quaternion.Euler(0, 0, -90));
                        borderCoordinates[i, j] = 1;

                    }

                    //One_Borders

                    else if (spawnCoordinates[i - 1, j] == 1 && spawnCoordinates[i + 1, j] == 1
                        && spawnCoordinates[i, j + 1] == 0 && spawnCoordinates[i, j - 1] == 0) {

                        Instantiate(one_left_right, new Vector3(-11.5f + i, 19.5f - j, -1), Quaternion.identity);
                        borderCoordinates[i, j] = 1;

                    } else if (spawnCoordinates[i, j + 1] == 1 && spawnCoordinates[i, j - 1] == 1
                       && spawnCoordinates[i + 1, j] == 0 && spawnCoordinates[i - 1, j] == 0) {

                        Instantiate(one_left_right, new Vector3(-11.5f + i, 19.5f - j, -1), Quaternion.Euler(0, 0, 90));
                        borderCoordinates[i, j] = 1;

                    }

                       //Single

                       else if (spawnCoordinates[i - 1, j] == 0 && spawnCoordinates[i + 1, j] == 0
                       && spawnCoordinates[i, j + 1] == 0 && spawnCoordinates[i, j - 1] == 0) {

                        Instantiate(single, new Vector3(-11.5f + i, 19.5f - j, -1), Quaternion.Euler(0, 0, 0));
                        borderCoordinates[i, j] = 1;

                    }

                    //Single_Borders

                    else if (spawnCoordinates[i - 1, j] == 0 && spawnCoordinates[i + 1, j] == 1
                       && spawnCoordinates[i, j + 1] == 1 && spawnCoordinates[i, j - 1] == 1) {

                        Instantiate(single_border, new Vector3(-11.5f + i, 19.5f - j, -1), Quaternion.Euler(0, 0, 0));
                        borderCoordinates[i, j] = 1;

                    } else if (spawnCoordinates[i - 1, j] == 1 && spawnCoordinates[i + 1, j] == 0
                       && spawnCoordinates[i, j + 1] == 1 && spawnCoordinates[i, j - 1] == 1) {

                        Instantiate(single_border, new Vector3(-11.5f + i, 19.5f - j, -1), Quaternion.Euler(0, 180, 0));
                        borderCoordinates[i, j] = 1;

                    } else if (spawnCoordinates[i, j + 1] == 1 && spawnCoordinates[i, j - 1] == 0
                       && spawnCoordinates[i + 1, j] == 1 && spawnCoordinates[i - 1, j] == 1) {

                        Instantiate(single_border, new Vector3(-11.5f + i, 19.5f - j, -1), Quaternion.Euler(0, 0, -90));
                        borderCoordinates[i, j] = 1;

                    } else if (spawnCoordinates[i, j + 1] == 0 && spawnCoordinates[i, j - 1] == 1
                       && spawnCoordinates[i + 1, j] == 1 && spawnCoordinates[i - 1, j] == 1) {

                        Instantiate(single_border, new Vector3(-11.5f + i, 19.5f - j, -1), Quaternion.Euler(0, 0, 90));
                        borderCoordinates[i, j] = 1;

                    }

                    //Single_corners

                    else if (spawnCoordinates[i - 1, j] == 0 && spawnCoordinates[i + 1, j] == 1
                        && spawnCoordinates[i, j + 1] == 1 && spawnCoordinates[i, j - 1] == 0) {

                        Instantiate(single_corner, new Vector3(-11.5f + i, 19.5f - j, -1), Quaternion.Euler(0, 0, 0));
                        borderCoordinates[i, j] = 1;

                    } else if (spawnCoordinates[i - 1, j] == 1 && spawnCoordinates[i + 1, j] == 0
                       && spawnCoordinates[i, j + 1] == 1 && spawnCoordinates[i, j - 1] == 0) {

                        Instantiate(single_corner, new Vector3(-11.5f + i, 19.5f - j, -1), Quaternion.Euler(0, 0, -90));
                        borderCoordinates[i, j] = 1;

                    } else if (spawnCoordinates[i, j + 1] == 0 && spawnCoordinates[i, j - 1] == 1
                       && spawnCoordinates[i + 1, j] == 1 && spawnCoordinates[i - 1, j] == 0) {

                        Instantiate(single_corner, new Vector3(-11.5f + i, 19.5f - j, -1), Quaternion.Euler(0, 0, 90));
                        borderCoordinates[i, j] = 1;

                    } else if (spawnCoordinates[i, j + 1] == 0 && spawnCoordinates[i, j - 1] == 1
                       && spawnCoordinates[i + 1, j] == 0 && spawnCoordinates[i - 1, j] == 1) {

                        Instantiate(single_corner, new Vector3(-11.5f + i, 19.5f - j, -1), Quaternion.Euler(0, 0, 180));
                        borderCoordinates[i, j] = 1;

                    }

                }

            }
        }




    }
    

   
}
