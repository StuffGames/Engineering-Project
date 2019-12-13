using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private AudioSource source;
    BoxCollider2D player;
    BoxCollider2D killPlane;
    BoxCollider2D nextLevel;
    BoxCollider2D box;
    CameraMovementScript cameraMove;
    bool gameEnded = false;
    public GameObject gameIsDone;

    // Start is called before the first frame update
    void Start()
    {

        if (gameObject.GetComponent<AudioSource>() != null)
        {
            source = gameObject.GetComponent<AudioSource>();
        }

        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider2D>();
        }
        if (GameObject.FindGameObjectWithTag("KillPlane") != null)
        {
            killPlane = GameObject.FindGameObjectWithTag("KillPlane").GetComponent<BoxCollider2D>();
        }
        if (GameObject.FindGameObjectWithTag("NextLevel") != null)
        {
            nextLevel = GameObject.FindGameObjectWithTag("NextLevel").GetComponent<BoxCollider2D>();
        }
        if(GameObject.FindGameObjectWithTag("EndGame") != null)
        {
            box = GameObject.FindGameObjectWithTag("EndGame").GetComponent<BoxCollider2D>();
        }
        else
        {
            Debug.Log("null");
            Debug.Log(GameObject.FindGameObjectWithTag("EndGame"));
        }
        if(GameObject.FindGameObjectWithTag("MainCamera") != null)
        {
            cameraMove = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovementScript>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (cameraMove != null && cameraMove.isPlayerDead == true)
        {
            gameEnded = true;
        }
        else
        {
            gameEnded = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (!source.isPlaying)
            {
                source.Play();
            }
            PlayGame();
        }
        if (gameEnded && Physics2D.IsTouching(player, box))
        {
            gameIsDone.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Space) && gameIsDone.activeSelf == true && SceneManager.GetActiveScene().buildIndex == 3)
        {
            BackToMenu();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().buildIndex != 0)
        {
            BackToMenu();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().buildIndex == 0)
        {
            QuitGame();
        }

    }

    void FixedUpdate()
    {
        if (player != null && killPlane != null && nextLevel != null)
        {
            /*if (Physics2D.IsTouching(player, killPlane))
            {
                CharacterContollerScript controllerScript = player.gameObject.GetComponent<CharacterContollerScript>();
                controllerScript.lives = 0;
            }*/
            if (Physics2D.IsTouching(player, nextLevel))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }

        if (box != null)
        {
            if (Physics2D.IsTouching(player, box))
            {
                CameraMovementScript cameraM = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovementScript>();
                cameraM.isPlayerDead = true;
            }
        }

    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PlayerIsDead()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
