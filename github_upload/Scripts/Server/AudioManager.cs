using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioClip[] clips;
    public GameObject audioSettingUi;
    public AudioSource[] videoaudioSource;
    public AudioSource[] gameaudioSource;
    public Slider videoVolumeSlider;
    public Slider gameVolumeSlider;
    public void AudioPlay(int clipvalue)
    {
        DataBase.instance.MyAudioClip = clips[clipvalue];
    }
    public void UIonoff()
    {
        if(audioSettingUi.activeSelf)
        {
            audioSettingUi.gameObject.SetActive(false);
        }
        else
        {
            audioSettingUi.gameObject.SetActive(true);
        }
    }
    void Start()
    {
        // 슬라이더의 값이 변경될 때 이벤트를 수신합니다.
        videoVolumeSlider.onValueChanged.AddListener(delegate { ChangeVolume(); });
    }

    // 볼륨 변경 함수
    void ChangeVolume()
    {
        if (videoaudioSource != null)
        {
            // 슬라이더의 값을 읽어 오디오 소스의 볼륨으로 설정합니다.
            for (int i = 0; i < videoaudioSource.Length; i++)
            {
                videoaudioSource[i].volume = videoVolumeSlider.value;
            }
        }
        if(gameaudioSource != null)
        {
            for (int i = 0; i < gameaudioSource.Length; i++)
            {
                gameaudioSource[i].volume = gameVolumeSlider.value;
            }
        }
    }
    public void MuteOn()
    {
        if (videoaudioSource != null)
        {
            // 슬라이더의 값을 읽어 오디오 소스의 볼륨으로 설정합니다.
            for (int i = 0; i < videoaudioSource.Length; i++)
            {
                videoaudioSource[i].volume = 0;
            }
        }
        if (gameaudioSource != null)
        {
            for (int i = 0; i < gameaudioSource.Length; i++)
            {
                gameaudioSource[i].volume = 0;
            }
        }
    }
}
