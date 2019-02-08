using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

    public Dialogue dialogue;


    public void TriggerDialogue() /* THis function fetches the variable from the inspector and passes to the DialogueManger */
    {

        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);  /* uses the singleton method*/

    }
}
