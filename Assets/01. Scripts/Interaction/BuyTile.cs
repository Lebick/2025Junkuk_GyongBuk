using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyTile : MonoBehaviour, IInteractable, IMouseOver
{
    private GamePlayManager gamePlayManager;

    public float testRange;

    public FarmTile myTile;

    public Color enableColor;
    public Color disableColor;
    public SkinnedMeshRenderer[] previewMeshs;

    public GameObject buyUI;
    public Text buyText;

    private void Start()
    {
        gamePlayManager = GamePlayManager.Instance;
    }

    private void Update()
    {
        foreach (SkinnedMeshRenderer mat in previewMeshs)
        {
            for (int i = 0; i < mat.materials.Length; i++)
            {
                mat.materials[i].color = Color.Lerp(mat.materials[i].color, IsBuyAbleRange() ? enableColor : disableColor, Time.deltaTime * 10f);
            }
        }

        buyUI.SetActive(IsBuyAbleRange());
        buyText.text = $"{1000 * Mathf.Pow(2, gamePlayManager.activeFarmTiles.Count):N0}$";
    }

    public bool Interaction()
    {
        if (IsBuyAbleRange())
        {
            int price = 1000 * (int)Mathf.Pow(2, gamePlayManager.activeFarmTiles.Count);

            if (gamePlayManager.money < price)
            {
                UIManager.Instance.SetAlert("돈이 읎어");
            }
            else
            {
                myTile.BuyTile();
                gamePlayManager.money -= price;
                return true;
            }
        }

        return false;
    }

    private bool IsBuyAbleRange()
    {
        foreach (var tile in gamePlayManager.activeFarmTiles)
        {
            if (Vector3.Distance(tile.transform.position, transform.position) <= testRange)
            {
                return true;
            }
        }
        return false;
    }

    public void MouseOutEvent()
    {
        if (IsBuyAbleRange())
            UIManager.Instance.SetAlert(string.Empty);
    }

    public void MouseOverEvent()
    {
        if (IsBuyAbleRange())
            UIManager.Instance.SetAlert("구매하기");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, testRange);
    }
}
