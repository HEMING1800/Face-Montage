using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text dialogueText;
    [SerializeField] GameObject dialogueUIBox;

    //The choice box
    [SerializeField] Text choice01;
    [SerializeField] Text choice02;
    [SerializeField] Text choice03;
    [SerializeField] GameObject choiceBox;

    [SerializeField] GameObject continueBox;

    [SerializeField] FaceMontage faceMontageController;
    [SerializeField] AudioManager audioController;

    // In End scene
    // Control the original post animation
    [SerializeField] Animator normalPostAnimator;
    [SerializeField] GameObject endDialogueButton;


    public string[] changedFaceComp;
    public Dialogue[] dialogues;

    private int currentScene; // Which task
    private int currentFaceComp; // current relative face component 
    private Queue<string> sentences;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        currentScene = 0;
        currentFaceComp = 0;
        StartDialogue(dialogues[0]);
    }

    void Update()
    {


    }

    public void StartDialogue(Dialogue dialogue)
    {
        // Pass the character's name to the UI 
        nameText.text = dialogue.name;

        // Clear the previous dialogue
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    // Display the next dialogue sentence of the character
    public void DisplayNextSentence()
    {
        // At the end of the task
        if (sentences.Count == 0)
        {
            choiceBox.SetActive(false);
            // No left sentences, then end dialogue
            nextScene();
            return;
        }

        string sentence = sentences.Dequeue();

        // Pass the sentence to the UI 
        StopAllCoroutines(); // The player could skip the current running typing sentence into the next one
        StartCoroutine(TypeSentence(dialogueText,sentence));

        Dialogue currentDialogue = dialogues[currentScene];
        string[] choices = currentDialogue.choices;

        // If at the end of each task, provide the choices
        if (sentences.Count == 0 && choices.Length > 0)
        {
            // Hide the continue button
            continueBox.SetActive(false);

            DisplayChoices(choices);
        }
    }

    // Assign choices' text into choice boxes
    private void DisplayChoices(string[] choicesList)
    {
        if(currentScene == dialogues.Length - 1)
        {
            // Display the final scene choice
            choiceBox.SetActive(true);
            choiceBox.transform.GetChild(1).gameObject.SetActive(false);
            choiceBox.transform.GetChild(2).gameObject.SetActive(false);

            StartCoroutine(TypeSentence(choice01, choicesList[0]));
        }
        else
        {
            // Display all the choices
            choiceBox.SetActive(true);
            StartCoroutine(TypeSentence(choice01, choicesList[0]));
            StartCoroutine(TypeSentence(choice02, choicesList[1]));
            StartCoroutine(TypeSentence(choice03, choicesList[2]));
        }

    }
    // The typing effect of the sentence
    IEnumerator TypeSentence(Text textBox, string sentence)
    {
        textBox.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            textBox.text += letter;
            yield return null; // wait single frame
        }
    }

    // Once the current review finsihed, move to next task
    private void nextScene()
    {
        if(currentScene < dialogues.Length-1)
        {
            // Enter next task if their are no choice
            currentScene++;
            StartDialogue(dialogues[currentScene]);

            if(currentScene != dialogues.Length - 1)
            {
                // Show the continue button only if not in end of task or the End scene
                continueBox.SetActive(true);
            }
            Debug.Log("move to next scene");
        }
        else
        {
            Debug.Log("END");
        }
    }

    // Record each face component with each different buttom click
    public void clickButtom(int choiceNo)
    {
        if (currentScene != dialogues.Length - 1)
        {
            // Record which choice is chose
            switch (choiceNo)
            {
                case 0:
                    break;
                case 1:
                    faceMontageController.SetTheComponent(changedFaceComp[currentFaceComp], 0);
                    break;
                case 2:
                    faceMontageController.SetTheComponent(changedFaceComp[currentFaceComp], 1);
                    break;
                case 3:
                    faceMontageController.SetTheComponent(changedFaceComp[currentFaceComp], 2);
                    break;
            }

            if(choiceNo != 0)
            {
                // Change to the next face component which shoud be changed
                currentFaceComp++;
            }

            DisplayNextSentence();
        }
        else
        {
            // In end scene
            ShowEndScene();
        }
    }

    public void ShowEndScene()
    {
        dialogueUIBox.SetActive(false);
        normalPostAnimator.SetBool("isIn", true);

        endDialogueButton.SetActive(true);
    }

    // Show the face montage
    public void ShowFaceMontage()
    {
        // Start change background music pitch
        StartCoroutine(audioController.PitchChange());

        faceMontageController.DisplayFaceMontage();
    }
}
