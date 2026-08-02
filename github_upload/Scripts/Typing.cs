using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Typing : MonoBehaviour
{
    public Text tutorialTxt;
    public string[] tutorialDialogue;
    public string[] dialogues;
    public int talkindex;
    public Animator tomyAnimator;

    public float delayTime;
    public void SetStart()
    {
        tomyAnimator.enabled = true;
        StartTyping(tutorialDialogue);
    }
    public void EndAni()
    {
        tomyAnimator.enabled = false;
    }
    public void StartTyping(string[] talks)
    {
        if (tutorialDialogue != null && tutorialDialogue.Length !=0)
        {
            dialogues = talks;

            StartCoroutine(TypingStart(dialogues[talkindex]));
        }
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
        yield return new WaitForSeconds(delayTime);
        NextTyping();
    }
    public void NextTyping()
    {
        //tutorialTxt.text = null;
        talkindex++;

        if (talkindex == dialogues.Length)
        {
            EndTyping();
            return;
        }
        else
            tutorialTxt.text = null;
        StartCoroutine(TypingStart(dialogues[talkindex]));
    }
    public void EndTyping()
    {
        EndAni();
        talkindex = 0;
        Debug.Log("대화끝");
    }
}
