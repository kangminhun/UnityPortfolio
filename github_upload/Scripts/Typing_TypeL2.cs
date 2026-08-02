using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Typing_TypeL2 : MonoBehaviour
{
    public Text tutorialTxt;
    public string[] tutorialDialogue;
    private string[] dialogues;
    public int talkindex;
    public Animator tommyAni;

    public AudioSource talkSound;
    public AudioClip[] talkClips;
    public void OnEnable()
    {
        if (tommyAni != null)
            tommyAni.enabled = true;
        tutorialTxt.text = "";
        Invoke("StartSet", 1.5f);
    }
    public void StartSet()
    {
        talkindex = 0;
        StartTyping(tutorialDialogue);
    }
    public void StartTyping(string[] talks)
    {
        if (gameObject.activeSelf)
        {
            dialogues = talks;
            talkSound.clip = talkClips[0];
            talkSound.Play();
            StartCoroutine(TypingStart(dialogues[talkindex]));
        }
        else
            return;
    }

    IEnumerator TypingStart(string talk)
    {
        tutorialTxt.text = null;

        if (talk.Contains("  ")) talk = talk.Replace("  ", "\n");

        for (int i = 0; i < talk.Length; i++)
        {
            tutorialTxt.text += talk[i];
            yield return new WaitForSeconds(0.05f);
        }
        while(talkSound.isPlaying)
        {
            yield return null;
        }
        yield return new WaitForSeconds(.5f);
        NextTyping();
    }
    public void NextTyping()
    {
        //tutorialTxt.text = null;
        talkindex++;

        if (talkindex >= dialogues.Length)
        {
            EndTyping();
            return;
        }
        else
            tutorialTxt.text = null;
        talkSound.clip = talkClips[talkindex];
        talkSound.Play();
        StartCoroutine(TypingStart(dialogues[talkindex]));
    }
    public void EndTyping()
    {
        talkindex = 0;
        if (tommyAni != null)
            tommyAni.enabled = false;
        Debug.Log("대화끝");
    }
}
