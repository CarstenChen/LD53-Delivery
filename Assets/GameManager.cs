using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject bagImage;
    public GameObject lineUI;

    private bool bagClicked;
    private bool hasStartQuitGame;
    private bool hasShownBag;
    // Start is called before the first frame update
    void Start()
    {
        DialogueManager.Instance.DisplayLine(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!DialogueManager.isPlayingLines && !hasShownBag)
        {
            hasShownBag = true;
            bagImage.SetActive(true);
        }

        if (bagClicked&&!hasStartQuitGame)
        {
            bagImage.SetActive(false);
            lineUI.SetActive(true);

            hasStartQuitGame = true;
            StartCoroutine(QuitGame(5f));
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void ClickBag()
    {
        bagClicked = true;
    }

    IEnumerator QuitGame(float delay)
    {
        yield return new WaitForSeconds(delay);
        Application.Quit();
    }
}
