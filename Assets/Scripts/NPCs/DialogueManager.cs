using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
 
public class DialogueManager : MonoBehaviour
{

    public static DialogueManager Instance;
    
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogueArea;
    public TextMeshProUGUI buttonText;
 
    private Queue<DialogueLine> lines;
    
    public bool isDialogueActive = false;
 
    public float typingSpeed = 0.2f;
 
    public Animator animator;
  
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
 
        lines = new Queue<DialogueLine>();
        
        gameObject.SetActive(false);
    }
 
    public void StartDialogue(Dialogue dialogue)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        isDialogueActive = true;

        gameObject.SetActive(true);
        animator.Play("PopIn");
 
        lines.Clear();
 
        foreach (DialogueLine dialogueLine in dialogue.dialogueLines)
        {
            lines.Enqueue(dialogueLine);
        }
 
        DisplayNextDialogueLine();
    }
 
    public void DisplayNextDialogueLine()
    {
        FindFirstObjectByType<AudioManager>().Play("UIClick");
        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }
        else if (lines.Count == 1)
        {
            buttonText.text = "Close";
        }
 
        DialogueLine currentLine = lines.Dequeue();
        
        characterName.text = currentLine.character.name;
        
        StopAllCoroutines();
 
        StartCoroutine(TypeSentence(currentLine));
    }
 
    IEnumerator TypeSentence(DialogueLine dialogueLine)
    {
        dialogueArea.text = "";
        foreach (char letter in dialogueLine.line.ToCharArray())
        {
            dialogueArea.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
 
    void EndDialogue()
    {
        isDialogueActive = false;
        animator.Play("PopOut");
        FindFirstObjectByType<AudioManager>().Play("ScreenClosed");
        
        gameObject.SetActive(isDialogueActive);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}