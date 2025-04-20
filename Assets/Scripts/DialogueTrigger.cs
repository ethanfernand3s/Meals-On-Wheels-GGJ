using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
 
[System.Serializable]
public class DialogueCharacter
{
    public string name;
    public Sprite icon;
}
 
[System.Serializable]
public class DialogueLine
{
    public DialogueCharacter character;
    [TextArea(3, 10)]
    public string line;
}
 
[System.Serializable]
public class Dialogue
{
    public List<DialogueLine> dialogueLines = new List<DialogueLine>();
}
 
public class DialogueTrigger : MonoBehaviour, IInteractable
{
    public Dialogue[] dialogue = new Dialogue[3];
    private int _dialogueIndex = 0;
    
    
    public void Interact(RaycastHit hit)
    {
        if(!(_dialogueIndex < dialogue.Length)) return;
        
        if (_dialogueIndex == 0)
        {
            DialogueManager.Instance.StartDialogue(dialogue[_dialogueIndex++]);
        }
        else if (Pickup.Instance.heldObj == null)
        {
            DialogueManager.Instance.StartDialogue(dialogue[_dialogueIndex]);
        }
        else
        {
            _dialogueIndex++;
            DialogueManager.Instance.StartDialogue(dialogue[_dialogueIndex++]);
            Destroy(Pickup.Instance.gameObject);
        }
    }
}