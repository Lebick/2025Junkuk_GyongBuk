using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour, IInteractable, IMouseOver
{
    private GamePlayManager gamePlayManager;
    private FadeManager fadeManager;

    private bool isInteraction;

    public Animator shopAnim;

    public Animator itemListAnim;
    public Animator upgradeAnim;
    public Animator buyAnim;
    public Animator sellAnim;

    public GameObject inventorySlotPrefab;
    public Transform toolInventoryTr;
    public Transform seedInventoryTr;
    public Transform cropInventoryTr;
    public Text inventoryCount;

    public Animator currentTabAnim;
    public Animator itemListTabAnim;
    public Animator buyTabAnim;
    public Animator sellTabAnim;

    public int maxInventoryCount;

    private bool isFocus;

    private ShopUI shopUI;

    private void Awake()
    {
        gamePlayManager = GamePlayManager.Instance;
        fadeManager = FadeManager.Instance;

        shopUI = UIManager.Instance.shopUI;
    }

    public bool Interaction()
    {
        if (isInteraction)
            return false;

        isInteraction = true;

        shopAnim.SetTrigger("Open");
        Time.timeScale = 0;

        return true;
    }

    public void MouseOverEvent()
    {
        UIManager.Instance.SetAlert("창고 이용하기");
    }
    public void MouseOutEvent()
    {
        UIManager.Instance.SetAlert(string.Empty);
    }
}
