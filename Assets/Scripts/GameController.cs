using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public enum State
    {
        TitleScreen,
        Playing,
        Paused,
        GameOver
    }

    public State currentstate;

    public Text titleScreenText;
    public Text pausedText;
    public Text gameOverText;

    public GameObject player;
    private GameObject playerGameObject;

    public GameObject otherCamera;

    private void Start()
    {
        currentstate = State.TitleScreen;
    }

    private void Update()
    {
        switch (currentstate)
        {
            case State.TitleScreen:
                if (Input.GetButton("Start"))
                {
                    currentstate = State.Playing;
                }

                otherCamera.SetActive(true);

                Destroy(playerGameObject);

                titleScreenText.text = "Title Screen\nPress enter to start";
                pausedText.text = "";
                gameOverText.text = "";

                break;

            case State.Playing:
                if (Input.GetButton("Cancel"))
                {
                    currentstate = State.Paused;
                }

                otherCamera.SetActive(false);

                if (playerGameObject == null)
                {
                    playerGameObject = Instantiate(player);
                }

                titleScreenText.text = "";
                pausedText.text = "";
                gameOverText.text = "";

                break;

            case State.Paused:
                if (Input.GetButton("Start"))
                {
                    currentstate = State.Playing;
                }

                titleScreenText.text = "";
                pausedText.text = "Paused\nPress enter to play";
                gameOverText.text = "";

                break;

            case State.GameOver:
                if (Input.GetButton("Start"))
                {
                    currentstate = State.Playing;
                } else if (Input.GetButton("Cancel"))
                {
                    currentstate = State.TitleScreen;
                }

                otherCamera.SetActive(true);

                Destroy(playerGameObject);

                titleScreenText.text = "";
                pausedText.text = "";
                gameOverText.text = "Game Over\nPress enter to retry\nPress esc to go to title screen";

                break;
        }
    }

    public void GameOver()
    {
        currentstate = State.GameOver;
    }
}
