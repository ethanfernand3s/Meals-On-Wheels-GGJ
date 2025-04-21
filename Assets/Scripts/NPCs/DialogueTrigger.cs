using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using NPCs;
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
    private int _dialogueIndex = 0;
    private FoodType[] _preferredFoodTypes;

    public Dialogue[] dialogue = new Dialogue[3];
    public GameObject playersHandSocket;

    private void Start()
    {
        _preferredFoodTypes = gameObject.GetComponent<PreferredFood>().foodTypes;
    }

    public void Interact(RaycastHit hit)
    {
        if(!(_dialogueIndex < dialogue.Length)) return;

        Pickup curPickupInstance = playersHandSocket.GetComponentInChildren<Pickup>();
            
        if (_dialogueIndex == 0)
        {
            DialogueManager.Instance.StartDialogue(dialogue[_dialogueIndex++]);
        }
        else if (curPickupInstance == null)
        {
            DialogueManager.Instance.StartDialogue(dialogue[_dialogueIndex]);
        }
        else if(_preferredFoodTypes.Contains(curPickupInstance.foodType))
        {
            _dialogueIndex++;
            DialogueManager.Instance.StartDialogue(dialogue[_dialogueIndex++]);
            Destroy(Pickup.Instance.gameObject);
        }
        else
        {
            // Display UI MSG Saying: "Not there favorite food"
        }
    }
}