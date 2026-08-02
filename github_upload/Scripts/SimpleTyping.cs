using System.Collections;
using TMPro;
using UnityEngine;

public class SimpleTyping : MonoBehaviour
{
    public TextMeshProUGUI txt;
    public AudioSource talkSound;
    public AudioClip talkClip;
    public string dialogue;

    public void OnEnable()
    {
        StartTyping(dialogue);
    }

    public void StartTyping(string talks)
    {
        if (gameObject.activeSelf)
        {
            dialogue = talks;
            talkSound.clip = talkClip;
            talkSound.Play();
            StartCoroutine(TypingStart(dialogue));
        }
        else
            return;
    }

    IEnumerator TypingStart(string talk)
    {
        txt.text = null;

        if (talk.Contains("  ")) talk = talk.Replace("  ", "\n");

        for (int i = 0; i < talk.Length; i++)
        {
            txt.text += talk[i];
            yield return new WaitForSeconds(0.05f);
        }
        while (talkSound.isPlaying)
        {
            yield return null;
        }
        yield return new WaitForSeconds(.5f);
    }
}
