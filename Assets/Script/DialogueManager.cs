using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Sprites;
using UnityEngine.SceneManagement;
using TMPro;

public class DialogueManager : MonoBehaviour {

    /* keeps track off all the sentence used in the DIalogue*/
    private Queue<string> sentences;  /* Queue uses a FIFO system */

   
    
    public Text nametext;
    public Text dialogueText;
    public GameObject image;
    

	// Use this for initialization
	void Start () {

        sentences = new Queue<string>(); /* for initialization  */
    
		
	}



    public void StartDialogue (Dialogue dialogue)
    {
        /* Debug.Log ("starting:" + dialogue.name);*/

    
        nametext.text = dialogue.name;
    

        sentences.Clear();


        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplaNextSentence();
  

    }


    public void DisplaNextSentence()
    {
        if (sentences.Count == 0) /* if there are no dilaogues left then end the function */
        {
            EndDialogue();
            return;
        }
        string sentence = sentences.Dequeue();

        StopAllCoroutines();  /* if the user presses continue before all the letter in the last dialogue is completely animated , it stops the function and starts the new dialgue*/
        StartCoroutine(TypeSentence(sentence)); /* animates the next dialoge */
    }
    IEnumerator TypeSentence(String sentence) /* helps to print the text letters one by onne */
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null; /* the animation takes the single frame to load */
        }


    }
    public  void EndDialogue()
    {
        SceneManager.LoadScene(1);
    }
}
