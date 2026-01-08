using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardScript : MonoBehaviour
{
    [SerializeField] Image BgImg, faceImg;
    [SerializeField] GridManager gridManager;
    [SerializeField] Button selectBtn;
    //[SerializeField] GameObject blockObj;
    [SerializeField] Animation anim;

    const string SCALEUP = "scaleUpCardAnim";
    const string SCALEDOWN = "ScaleDownAnim";

    public int Id { get; private set; }

    public int spriteId { get; private set; }

    private void OnEnable()
    {
        selectBtn.onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        selectBtn.onClick.RemoveAllListeners();
    }

    public void SetData(int cardId, int faceId)
    {
        Id = cardId;
        spriteId = faceId;

        BgImg.sprite = gridManager.frontSprite;
        faceImg.sprite = gridManager.GetSprite(faceId);
        selectBtn.gameObject.SetActive(false);
        faceImg.gameObject.SetActive(true);
    }

    void OnClick()
    {
        if (gridManager.IsBusy) return;
        Show();
        gridManager.CardSelected(this);
    }

    public void Show()
    {
        StartCoroutine(OpenTile());
    }

    public void Hide()
    {
        StartCoroutine(CloseTile());
    }

    public void ResetData()
    {
        Id = -1;
        spriteId = -1;
    }

    IEnumerator OpenTile()
    {
        anim.Play(SCALEDOWN);
        SoundManager.instance.PlaySound(SoundType.cardFlip);
        yield return new WaitForSeconds(0.2f);
        selectBtn.gameObject.SetActive(false);
        BgImg.sprite = gridManager.frontSprite;
        faceImg.gameObject.SetActive(true);
        anim.Play(SCALEUP);
        
    }

    IEnumerator CloseTile()
    {
        anim.Play(SCALEDOWN);
        SoundManager.instance.PlaySound(SoundType.cardFlip);
        yield return new WaitForSeconds(0.2f);
        selectBtn.gameObject.SetActive(true);
        BgImg.sprite = gridManager.bgImgSprite;
        faceImg.gameObject.SetActive(false);
        anim.Play(SCALEUP);

    }


    public void Disable()
    {
        StartCoroutine(DisableTileAnim());
    }

    IEnumerator DisableTileAnim()
    {
        anim.Play(SCALEDOWN);
        yield return new WaitForSeconds(0.2f);
        gameObject.SetActive(false);
    }

}
