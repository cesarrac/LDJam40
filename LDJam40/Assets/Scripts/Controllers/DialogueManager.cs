using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {
	public static DialogueManager instance {get; protected set;}
	public Queue<string> dialogue_sentences;

	public GameObject dialogWindow;
	public Text dialogue_name, dialogue_text;
	bool inDialogue;
	private void Awake(){
		instance = this;
		dialogue_sentences = new Queue<string>();
	}
	public void StartDialogue(Dialogue dialogue){
		Debug.Log("StartDialogue");
		if (inDialogue == true){
			return;
		}
		inDialogue = true;
		dialogue_sentences.Clear();
		foreach(string sentence in dialogue.sentences){
			dialogue_sentences.Enqueue(sentence);
		}
		
		dialogue_name.text = dialogue.speakerName;
		TryNextSentence();
	}
	public void TryNextSentence(){
		if (dialogue_sentences.Count <= 0){
			EndDialogue();
			return;
		}
		dialogWindow.SetActive(true);
		dialogue_text.text = dialogue_sentences.Dequeue();
		
	}
	public void EndDialogue(){
		dialogWindow.SetActive(false);
		inDialogue = false;
	}
}
