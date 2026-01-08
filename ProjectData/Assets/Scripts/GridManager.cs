using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    [SerializeField] RectTransform rectT;
    [SerializeField] Transform parentTrans;
    [SerializeField] GridLayoutGroup grid;
    [SerializeField] CardScript cardPrefab;
    [SerializeField] List<Sprite> spriteList;
    public Sprite bgImgSprite, frontSprite;

    [Header("TurnAndMatchScore")]
    [SerializeField] TMP_Text matchTxt, turnTxt, comboTxt;

    int matchCount, turnCount, comboCount;

    int Matches { get { return matchCount; } set { matchCount = value; matchTxt.text = $"{value}"; } }
    int Turns { get { return turnCount; } set { turnCount = value; turnTxt.text = $"{value}"; } }
    int Combo { get { return comboCount; } set { comboCount = value; comboTxt.text = $"{value}"; } }

    private CardScript firstCard, secondCard;

    private List<CardScript> allCards = new List<CardScript>();

    private Queue<CardScript> pool = new Queue<CardScript>();

    int count;

    private void Awake()
    {
        //Create a ObjPool
        for (int i = 0; i < 30; i++)
        {
            CreateObj();
        }
    }

    private void Start()
    {
        //SetLayOut(4,6);
    }

    void CreateObj()
    {
        count++;
        CardScript cs = Instantiate(cardPrefab, parentTrans);
        cs.gameObject.SetActive(false);
        cs.gameObject.name = $"item_{count}";
        pool.Enqueue(cs);
    }

    private CardScript GetObj()
    {
        if (pool.Count == 0)
            CreateObj();

        CardScript cs = pool.Dequeue();
        cs.gameObject.SetActive(true);
        return cs;
    }

    private void ReturnObj(CardScript cs)
    {
        cs.transform.SetParent(parentTrans);
        cs.gameObject.SetActive(false);
        pool.Enqueue(cs);
    }


    void SetLayOut(int row, int col)
    {
        grid.enabled = true;
        grid.constraintCount = col;
        float cellHeight = (rectT.rect.height - (grid.spacing.y * row)) / row;
        float cellWidth = (rectT.rect.width - (grid.spacing.x * col)) / col;
        //Debug.Log("rect Height :" + rectT.rect.height);
        //Debug.Log("rect Height :" + rectT.rect.width);
        //Debug.Log("Cell Size 1 :" + cellWidth);
        //Debug.Log("Cell Size 2 :" + cellHeight);

        float cellSize = Mathf.Min(cellWidth, cellHeight);
        grid.cellSize = new Vector2(cellSize, cellSize);
    }


    public void SetGrid(Vector2Int size)
    {
        IsBusy = true;
        SetLayOut(size.x, size.y);
        int totalCards = size.x * size.y;
        //int totalPairs = totalCards / 2;

        List<int> numList = new List<int>();
        for (int k = 0; k < spriteList.Count; k++)
        {
            numList.Add(k);
        }

        List<(int Id, int FaceId)> cardData = new List<(int Id, int FaceId)>();

        int i = 1;
        while (totalCards > cardData.Count)
        {
            int indexValue = Random.Range(0, numList.Count);
            int fId = numList[indexValue];
            numList.RemoveAt(indexValue);
            cardData.Add((i, fId));
            Debug.Log($"id : {i}  , fId : {fId}");
            i++;
            cardData.Add((i, fId));
            Debug.Log($"id : {i}  , fId : {fId}");
            i++;
        }

        //Shuffle Cards
        for (int j = 0; j < cardData.Count; j++)
        {
            var temp = cardData[j];
            int rand = Random.Range(0, cardData.Count);
            cardData[j] = cardData[rand];
            cardData[rand] = temp;
        }

        //foreach(var card in cardData)
        //{
        //    Debug.Log(card.ToString());
        //}
        allCards = new List<CardScript>();
        foreach (var card in cardData)
        {
            CardScript cs = GetObj();
            cs.transform.SetParent(rectT);
            cs.SetData(card.Id, card.FaceId);
            allCards.Add(cs);
        }
        //grid.enabled = false;
        StartCoroutine(ShowAndCloseAllCards());
    }

    IEnumerator ShowAndCloseAllCards()
    {
        yield return new WaitForSeconds(1);
        grid.enabled = false;
        //foreach (var card in allCards)
        //{
        //    card.Show();
        //}
        foreach (var card in allCards)
        {
            card.Hide();
        }
        IsBusy = false;
        firstCard = null;
        secondCard = null;
    }

    public Sprite GetSprite(int index)
    {
        return spriteList[index];
    }

    public bool IsBusy { get; private set; }

    public void CardSelected(CardScript Card)
    {
        if (firstCard == null)
        {
            firstCard = Card;
            Debug.Log("First Card  ----------!"+ firstCard.spriteId);
            return;
        }
        else if (secondCard == null)
        {
            secondCard = Card;
            
            Debug.Log("Second Card ----------!" + secondCard.spriteId);
            StartCoroutine(CheckMatch());
        }
    }


    IEnumerator CheckMatch()
    {
        Turns++;
        IsBusy = true;
        yield return new WaitForSeconds(1f);


        if (firstCard.spriteId == secondCard.spriteId)
        {
            Matches++;
            SoundManager.instance.PlaySound(SoundType.match);
            // Add Sound For Match;
            //if (allCards.Contains(firstCard))
            //{
            //    allCards.Remove(firstCard);
            //}
            //else
            //{
            //    Debug.Log("Card1 is not in List.");
            //}

            //if (allCards.Contains(secondCard))
            //{
            //    allCards.Remove(secondCard);
            //}
            //else
            //{
            //    Debug.Log("Card2 is not in List.");
            //}
            Combo++;
            GameDataLoader.Instance.SetHighMatch(comboCount);
            firstCard.Disable();
            secondCard.Disable();


            if (allCards.Count == matchCount * 2)
            {
                // Restart Game
                yield return new WaitForSeconds(0.5f);
                //SetupGrid();
                Debug.Log("GameOver : ");
                GameDataLoader.Instance.SetBestTurn(turnCount);
                GameManager.instance.OpenGameOverPanel(turnCount);
                SoundManager.instance.PlaySound(SoundType.gameOver);
            }
        }
        else
        {
            //AddSound For MisMatch
            SoundManager.instance.PlaySound(SoundType.misMatch);
            firstCard.Hide();
            secondCard.Hide();
            Combo = 0;
        }
        firstCard = null;
        secondCard = null;
        IsBusy = false;
    }





    public void ResetOptions()
    {

        foreach (var card in allCards)
        {
            ReturnObj(card);
        }
        allCards = null;

        Matches = 0;
        Turns = 0;
        Combo = 0;
        firstCard = null;
        secondCard = null;
    }
}
