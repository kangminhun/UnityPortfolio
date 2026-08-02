using UnityEngine;
using UnityEngine.UI;

public class CharacterLoading : MonoBehaviour
{
    public Slider loadingSlider; // 슬라이더
    public Transform characterTransform; // 캐릭터의 Transform

    private void Update()
    {
        // 슬라이더 값을 이용하여 캐릭터 위치 조절
        float sliderValue = loadingSlider.value;
        float newPositionX = Mathf.Lerp(-616, 667, sliderValue); // 최솟값과 최댓값 설정 필요
        characterTransform.localPosition = new Vector3(newPositionX, characterTransform.position.y, characterTransform.position.z);
    }
}
