using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable] /* helps to shpw the class tothe inspector for modification*/
public class Dialogue  {

    /* this is used to pass as an object to the dialogueManager when we want to start a new dialgue */
    /* THis doesnot need a monobehavior cause we dnt want the class to sit on a script */

    public string name;
    [TextArea(5,10)] /* minimuma nd maximum text area we will use*/
    public string[] sentences;
	
	
}
