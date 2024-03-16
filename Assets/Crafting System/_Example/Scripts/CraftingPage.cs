using Core.UI;
using DependencyInjection;
using System.Collections.Generic;
using Tools.Inventory.Crafting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityServiceLocator;

public class CraftingPage : MonoBehaviour
{
    [SerializeField]
    private Transform craftableItemContainer;
    [SerializeField]
    private CraftableItemSlot craftableItemSlot;

    public UnityEvent<CraftItem> OnRequestCraft = new();

    [Inject]
    private CraftModel _craftModel;

    private UIPage _uiPage;

    private void Awake()
    {
        _uiPage = GetComponent<UIPage>();
        _uiPage.OnOpen.AddListener(OpenCraftingView);        
    }

    private void Start()
    {
        ViewEvent.Subscribe(OnViewEvent);
        
        ServiceLocator.Global.Get<CraftController>(out var controller);
        Debug.Log(controller);

        Assert.IsNotNull(_craftModel, "CraftModel is not set");
        Debug.Log(_craftModel);
    }

    private void OnDestroy()
    {
        ViewEvent.Unsubscribe(OnViewEvent);
    }

    public void PopulateCraftableItems(List<CraftItem> craftItems)
    {
        DestroySlot();

        foreach (var craftItem in craftItems)
        {
            var craftableItem = Instantiate(craftableItemSlot, craftableItemContainer);
            craftableItem.Setup(craftItem);
            craftableItem.EnableCraft(craftItem.IsCraftable);
            craftableItem.OnCraftPressed += OnCraftPressed;
        }
    }

    private void OnCraftPressed(CraftItem item)
    {
        OnRequestCraft?.Invoke(item);
    }

    private void DestroySlot()
    {
        foreach (Transform child in craftableItemContainer)
        {
            Destroy(child.gameObject);
        }
    }

    private void OpenCraftingView()
    {
        Assert.IsNotNull(_craftModel, "CraftModel is not set");
        PopulateCraftableItems(_craftModel.Craftables);
    }

    private void OnViewEvent(ViewEventArgs args)
    {
        if (args.ViewId != _uiPage.PageID)
            return;

        if(_craftModel == null)
        {
            Debug.LogError("CraftModel is not set");
            return;
        }

        PopulateCraftableItems(_craftModel.Craftables);
        _uiPage.OpenPage();
    }

    public void PrepareCraftModel(CraftModel model)
    {
        Debug.Log("Called" + model.Craftables.Count);
        _craftModel = model;
    }
}
