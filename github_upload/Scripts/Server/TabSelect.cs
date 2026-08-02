using UnityEngine;
using UnityEngine.UI;

public class TabSelect : MonoBehaviour
{
    public InputField[] inputFields;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // 현재 포커스된 InputField 찾기
            GameObject selectedObject = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;

            if (selectedObject != null)
            {
                InputField currentInputField = selectedObject.GetComponent<InputField>();

                if (currentInputField != null)
                {
                    // 현재 포커스된 InputField의 인덱스 찾기
                    int currentIndex = System.Array.IndexOf(inputFields, currentInputField);

                    // 다음 비활성화되지 않은 InputField로 포커스 이동
                    int nextIndex = FindNextActiveIndex(currentIndex);

                    if (nextIndex >= 0 && nextIndex < inputFields.Length)
                    {
                        InputField nextInputField = inputFields[nextIndex];
                        nextInputField.Select();
                        nextInputField.ActivateInputField();
                    }
                }
            }
        }
    }

    // 다음 비활성화되지 않은 InputField의 인덱스 찾기
    int FindNextActiveIndex(int currentIndex)
    {
        for (int i = currentIndex + 1; i < inputFields.Length; i++)
        {
            if (IsInputFieldActive(inputFields[i]))
            {
                return i;
            }
        }

        return -1;
    }

    // InputField가 활성화되어 있는지 확인
    bool IsInputFieldActive(InputField inputField)
    {
        Transform currentTransform = inputField.transform;

        while (currentTransform != null)
        {
            if (!currentTransform.gameObject.activeInHierarchy)
            {
                return false;
            }

            currentTransform = currentTransform.parent;
        }

        return true;
    }
}
