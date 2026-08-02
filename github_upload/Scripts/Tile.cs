using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tile : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Sprite tileSprite;
    private PuzzleManager puzzleManager;
    public Vector3 correctPosition;
    public Vector3 myPosition;

    public bool IsCorrected { private set; get; } = false;
    public bool C;

    private int numeric;
    public int Numeric
    {
        set
        {
            numeric = value;
            switch (puzzleManager.roundData)
            {
                case 0:
                    tileSprite = puzzleManager.tileSprites_3x3[numeric - 1];
                    break;
                case 1:
                    tileSprite = puzzleManager.twoRound_tileSprites_3x3[numeric - 1];
                    break;
                case 2:
                    tileSprite = puzzleManager.threeRound_tileSprites_3x3[numeric - 1];
                    break;
                case 3:
                    tileSprite = puzzleManager.fourRound_tileSprites_3x3[numeric - 1];
                    break;
            }
        }
        get => numeric;
    }


    public void Setup(PuzzleManager board, int hideNumeric, int numeric)
    {
        this.puzzleManager = board;

        Numeric = numeric;
        GetComponent<Image>().sprite = tileSprite;
    }

    public void SetCorrectPosition()
    {
        correctPosition = GetComponent<RectTransform>().localPosition;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // 클릭했을 때 행동
        puzzleManager.IsMoveTile(this);
    }

    public void OnMoveTo(Vector3 end)
    {
        StartCoroutine("MoveTo", end);
    }

    private IEnumerator MoveTo(Vector3 end)
    {
        float current = 0;
        float percent = 0;
        float moveTime = 0.1f;
        Vector3 start = GetComponent<RectTransform>().localPosition;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / moveTime;

            GetComponent<RectTransform>().localPosition = Vector3.Lerp(start, end, percent);

            yield return null;
        }

        IsCorrected = correctPosition == GetComponent<RectTransform>().localPosition ? true : false;

        puzzleManager.IsGameOver();
    }
    public void startSetting()
    {
        IsCorrected = correctPosition == GetComponent<RectTransform>().localPosition ? true : false;
    }
    public void ResetTile()
    {
        IsCorrected = false;

    }
}
