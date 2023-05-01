using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class DialogueManager : MonoBehaviour
{
    private static DialogueManager instance;
    public static DialogueManager Instance { get { return instance; } private set { } }

    public GameObject textMeshPros;

    //public Color LerpColorA;
    //public Color LerpColorB;

    [SerializeField] public static bool isPlayingLines;
    //[SerializeField] public static bool isReadingStartPlot;

    //protected Animator textAnimator;
    protected AudioSource audioSource;
    protected PlotManager plotManager;
    protected Dialogue[] allDialogues;
    protected Dialogue currentLine;

    public bool blockInput;


    private void Awake()
    {
        if (instance == null)
            instance = this;

        isPlayingLines = false;

        audioSource = GetComponent<AudioSource>();

        plotManager = Resources.Load<PlotManager>("DataAssets/Dialogues");
        allDialogues = plotManager.dialogues;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void DisplayLine(int plotID, int index)
    {
        if (isPlayingLines && index == 0) return;

        if (index == 0)
        {
            StopAllCoroutines();
            isPlayingLines = true;
        }

        for (int i = 0; i < allDialogues.Length; i++)
        {
            if (allDialogues[i].plotID == plotID && allDialogues[i].index == index)
            {
                currentLine = allDialogues[i];

                textMeshPros.GetComponentInChildren<TextMeshProUGUI>().text = currentLine.text;

                StartCoroutine(SetLineUI(true, 0f));
                StartCoroutine(WaitSoundEndToNextLine(currentLine));
            }
        }
    }
    IEnumerator WaitSoundEndToNextLine(Dialogue dialogue)
    {

        audioSource.clip = dialogue.audio;
        audioSource.Play();

        yield return new WaitForSeconds(audioSource.clip.length);

        if (dialogue.nextIndex != -1)
        {
            DisplayLine(dialogue.plotID, dialogue.nextIndex);
        }
        else
        {
            //if (textAnimator != null)
            //{
            //    textAnimator.SetBool("FadeIn", false);
            //    textAnimator.SetBool("FadeOut", true);
            //}
            StartCoroutine(SetLineUI(false, 1f));
        }
    }

    IEnumerator SetLineUI(bool active, float delay)
    {
        yield return new WaitForSeconds(delay);

        textMeshPros.gameObject.SetActive(active);
    

        if (!active)
        {
            isPlayingLines = false;
            blockInput = false;
        }
    }
}