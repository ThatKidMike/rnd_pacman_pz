using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PacmanMovement : MonoBehaviour {

    public float velocity = 4f;
    private Vector2 go;
    public Vector2 direction = Vector2.right;
    public Vector2 orientation;
    private Vector2 startingPos = new Vector2(1.5f, -5);

    public GameObject find;
    public PillsSpawn lookFor;

    public RuntimeAnimatorController deathAnimation;
    public RuntimeAnimatorController eatingAnimation;

    private GameObject background;
    private GameObject backgroundFear;
    private GameObject soundOfDeath;
    private GameObject introSound;
    public AudioSource backgroundSound;
    public AudioSource backgroundFearSound;
    private AudioSource deathAudioSource;
    private AudioSource introMusic;
    private AudioSource _audio;
    private AudioSource deathSound;
    public AudioClip sound1;
    public AudioClip sound2;

    public GameObject red;
    public GameObject blue;
    public GameObject orange;
    public GameObject pink;

    private Pink_movement pScript;
    private Orange_movement oScript;
    private Blue_movement bScript;
    private red_movement rScript;

    private GameObject canvas;
    private Canvas readyCanvas;

    private GameObject checkForPellets;
    private GameObject checkForPellets1;
    private GameObject checkForPellets2;
    private GameObject checkForPellets3;

    public Text scoreText;
    public Text up1;
    public Image lives1;
    public Image lives0;
    public Text highScore;

    public int score = 0;
    public int playerCharLives = 3;

    public bool afterDeathMovement = false;
    private bool playedSound1 = false;
    private bool deathStarted = false;

    // Use this for initialization
    void Start () {

        find = GameObject.Find("PillsSpawn");
        lookFor = find.GetComponent<PillsSpawn>();
        transform.position = new Vector2(1.5f, -5);
        _audio = gameObject.GetComponent<AudioSource>();
        //position = gameObject.transform.position;

        red = GameObject.Find("ghost");
        blue = GameObject.Find("ghost_blue");
        orange = GameObject.Find("ghost_orange");
        pink = GameObject.Find("ghost_pink");

        pScript = pink.GetComponent<Pink_movement>();
        oScript = orange.GetComponent<Orange_movement>();
        bScript = blue.GetComponent<Blue_movement>();
        rScript = red.GetComponent<red_movement>();

        background = GameObject.Find("background_sound");
        backgroundFear = GameObject.Find("fearModeSound");
        soundOfDeath = GameObject.Find("DeathSound");
        backgroundSound = background.GetComponent<AudioSource>();
        backgroundFearSound = backgroundFear.GetComponent<AudioSource>();
        deathSound = soundOfDeath.GetComponent<AudioSource>();

        introSound = GameObject.Find("IntroSound");
        introMusic = background.GetComponent<AudioSource>();

        canvas = GameObject.Find("Canvas");
        readyCanvas = canvas.GetComponent<Canvas>();
        highScore.text = StaticStas.Points.ToString();

        StartGame();


    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKey("escape")) {
            Application.Quit();
        }

        if (!afterDeathMovement) {

            CheckInput();
            //Move();
            moveToNxtPoint(direction);
            UpdateUI();
          

            if(lookFor.amount <= 0) {

                
                rScript.afterDeathMovement = true;
                bScript.afterDeathMovement = true;
                pScript.afterDeathMovement = true;
                oScript.afterDeathMovement = true;

                gameObject.GetComponent<Animator>().enabled = false;
                afterDeathMovement = true;
                backgroundSound.enabled = false;
                backgroundFearSound.enabled = false;

                StaticStas.Points = score;
                //highScore.text = StaticStas.Points.ToString();

                StartCoroutine(ProcessEnd(4));

            } 


        }

	}

    IEnumerator ProcessEnd(float delay) {

        yield return new WaitForSeconds(delay);


        SceneManager.LoadScene("End title screen");


    }

    void UpdateUI() {

        scoreText.text = score.ToString();

        if(playerCharLives == 3) {

            lives0.enabled = true;
            lives1.enabled = true;

        } else if(playerCharLives == 2) {

            lives0.enabled = true;
            lives1.enabled = false;

        } else if(playerCharLives == 1) {

            lives0.enabled = false;
            lives1.enabled = false;

        }

    }

    public void StartGame() {

        red.transform.GetComponent<SpriteRenderer>().enabled = false;
        blue.transform.GetComponent<SpriteRenderer>().enabled = false;
        pink.transform.GetComponent<SpriteRenderer>().enabled = false;
        orange.transform.GetComponent<SpriteRenderer>().enabled = false;
        rScript.afterDeathMovement = true;
        bScript.afterDeathMovement = true;
        pScript.afterDeathMovement = true;
        oScript.afterDeathMovement = true;

        gameObject.transform.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<Animator>().enabled = false;
        afterDeathMovement = true;
        backgroundSound.enabled = false;
        introMusic.Play();

        StartCoroutine(SpawnStart(2.25f));

    }

    IEnumerator SpawnStart(float delay) {

        yield return new WaitForSeconds(delay);

        red.transform.GetComponent<SpriteRenderer>().enabled = true;
        blue.transform.GetComponent<SpriteRenderer>().enabled = true;
        pink.transform.GetComponent<SpriteRenderer>().enabled = true;
        orange.transform.GetComponent<SpriteRenderer>().enabled = true;

        gameObject.transform.GetComponent<SpriteRenderer>().enabled = true;


        introMusic.enabled = false;
        StartCoroutine(StartGameAfter(2));


    }

    IEnumerator StartGameAfter (float delay) {

        yield return new WaitForSeconds(delay);

        backgroundSound.enabled = true;
        rScript.afterDeathMovement = false;
        bScript.afterDeathMovement = false;
        pScript.afterDeathMovement = false;
        oScript.afterDeathMovement = false;
        afterDeathMovement = false;
        readyCanvas.enabled = false;
        gameObject.GetComponent<Animator>().enabled = true;

    }

    IEnumerator AfterDeathSpawning(float delay) {

        readyCanvas.enabled = true;

        _audio.enabled = false;

        pScript.Restart();
        oScript.Restart();
        bScript.Restart();
        rScript.Restart();

        transform.position = startingPos;
        direction = Vector2.right;
        transform.rotation = Quaternion.Euler(0, 0, 0);

        gameObject.transform.GetComponent<Animator>().enabled = false;

        red.transform.GetComponent<SpriteRenderer>().enabled = true;
        blue.transform.GetComponent<SpriteRenderer>().enabled = true;
        pink.transform.GetComponent<SpriteRenderer>().enabled = true;
        orange.transform.GetComponent<SpriteRenderer>().enabled = true;

        gameObject.transform.GetComponent<SpriteRenderer>().enabled = true;
        gameObject.transform.GetComponent<Animator>().runtimeAnimatorController = eatingAnimation;

        yield return new WaitForSeconds(delay);

        Restart();


    }

    public void DeathStart() {

        if(!deathStarted) {

            deathStarted = true;

            rScript.afterDeathMovement = true;
            oScript.afterDeathMovement = true;
            pScript.afterDeathMovement = true;
            bScript.afterDeathMovement = true;
            afterDeathMovement = true;

            gameObject.transform.GetComponent<Animator>().enabled = false;
            backgroundSound.Stop();
            backgroundFearSound.Stop();

            StartCoroutine(ProcessDeathAfter(2));

        }

    }

    IEnumerator ProcessDeathAfter(float delay) {

        yield return new WaitForSeconds(delay);

        red.GetComponent<SpriteRenderer>().enabled = false;
        blue.GetComponent<SpriteRenderer>().enabled = false;
        pink.GetComponent<SpriteRenderer>().enabled = false;
        orange.GetComponent<SpriteRenderer>().enabled = false;

        StartCoroutine(ProcessDeathAnimation(1.9f));

    }

    IEnumerator ProcessDeathAnimation(float delay) {

        gameObject.transform.localScale = new Vector3(1, 1, 1);
        gameObject.transform.localRotation = Quaternion.identity;

        gameObject.transform.GetComponent<Animator>().runtimeAnimatorController = deathAnimation;
        gameObject.GetComponent<Animator>().enabled = true;

        deathSound.Play();

        yield return new WaitForSeconds(delay);

        StartCoroutine(ProcessRestart(2));

    }

    IEnumerator ProcessRestart(float delay) {

        playerCharLives--;

        gameObject.transform.GetComponent<SpriteRenderer>().enabled = false;

        transform.GetComponent<AudioSource>().Stop();

        if(playerCharLives <= 0) {

            StaticStas.Points = score;
            SceneManager.LoadScene("End title screen");

        }

        yield return new WaitForSeconds(delay);

        StartCoroutine(AfterDeathSpawning(2.5f));
    }

    public void Restart() {

        if(playerCharLives > 0) {

            readyCanvas.enabled = false;

            rScript.afterDeathMovement = false;
            oScript.afterDeathMovement = false;
            pScript.afterDeathMovement = false;
            bScript.afterDeathMovement = false;
            afterDeathMovement = false;
            deathStarted = false;

            gameObject.GetComponent<Animator>().enabled = true;
            
            backgroundSound.Play();
            backgroundFearSound.Play();
            _audio.enabled = true;

        }  

    }

    public void PlaySound() {

        if(playedSound1) {

            _audio.PlayOneShot(sound1);
            playedSound1 = false;

        } else {

            _audio.PlayOneShot(sound2);
            playedSound1 = true;

        }
    }

    void CheckInput() {
      
        if(Input.GetKeyDown(KeyCode.LeftArrow) && canMove(Vector2.left)) {

            if(!direction.Equals(Vector2.left)) {
                transform.rotation = Quaternion.identity;
                transform.Rotate(0, 0 - 180, 0);
                direction = Vector2.left;
                moveToNxtPoint(direction);
            }

        } else if(Input.GetKeyDown(KeyCode.RightArrow) && canMove(Vector2.right)) {

            if(!direction.Equals(Vector2.right)) {
                transform.rotation = Quaternion.identity;
                direction = Vector2.right;
                moveToNxtPoint(direction);
            }

        } else if(Input.GetKeyDown(KeyCode.UpArrow) && canMove(Vector2.up)) {

            if(!direction.Equals(Vector2.up)) {
                transform.rotation = Quaternion.identity;
                transform.Rotate(0, 0, 0 + 90);
                direction = Vector2.up;
                moveToNxtPoint(direction);
            }

        } else if(Input.GetKeyDown(KeyCode.DownArrow) && canMove(Vector2.down)) {

            if(!direction.Equals(Vector2.down)) {
                transform.rotation = Quaternion.identity;
                transform.Rotate(0, 0, 0 - 90);
                direction = Vector2.down;
                moveToNxtPoint(direction);
            }

        }
  
    }

    void moveToNxtPoint(Vector2 d) {

        Vector2 currPosition = transform.localPosition;
        Vector2 targetPos = new Vector2((int)Math.Round(currPosition.x + d.x), (int)Math.Round(currPosition.y + d.y));
        transform.localPosition = Vector2.MoveTowards(currPosition, targetPos, velocity * Time.deltaTime * 1.3f);
 
    }

    bool canMove(Vector2 d) {

        Vector2 currPosition = transform.localPosition;

        if(lookFor.xy[(int)Math.Round(currPosition.x + d.x) + 30, (int)Math.Round(currPosition.y + d.y) + 30] == 1) {
            return true;
        } else {
            return false;
        }

    }


}
