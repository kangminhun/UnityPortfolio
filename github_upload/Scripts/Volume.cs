using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Volume : MonoBehaviour
{
    public AudioSource audioSource;
    public Slider volumeSlider;

    void Start()
    {
        // 오디오 소스와 슬라이더를 연결합니다.
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        // 슬라이더의 값이 변경될 때 이벤트를 수신합니다.
        volumeSlider.onValueChanged.AddListener(delegate { ChangeVolume(); });
    }

    // 볼륨 변경 함수
    void ChangeVolume()
    {
        if (audioSource != null)
        {
            // 슬라이더의 값을 읽어 오디오 소스의 볼륨으로 설정합니다.
            audioSource.volume = volumeSlider.value;
        }
    }
}
