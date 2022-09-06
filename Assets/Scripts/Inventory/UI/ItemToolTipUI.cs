using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemToolTipUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI typeText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Text valueText;
    [SerializeField] private GameObject bottomPart;

    public void SetupItemToolTip(ItemDetails itemDetails, SlotType slotType)
    {
        nameText.text = itemDetails.itemName;

        typeText.text = GetItemType(itemDetails.itemType);

        descriptionText.text = itemDetails.itemDescription;

        if (itemDetails.itemType == ItemType.Seed || itemDetails.itemType == ItemType.Commodity || itemDetails.itemType == ItemType.Furniture)
        {
            bottomPart.SetActive(true);

            var price = itemDetails.itemPrice;
            if (slotType == SlotType.Player)
            {
                price = (int)(price * itemDetails.sellPercentage);
            }

            valueText.text = price.ToString();
        }
        else
        {
            bottomPart.SetActive(false);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }

    private string GetItemType(ItemType itemType)
    {
        return itemType switch
        {
            ItemType.Seed => "����",
            ItemType.Commodity => "��Ʒ",
            ItemType.Furniture => "�Ҿ�",
            ItemType.HoeTool => "����",
            ItemType.ChopTool => "����",
            ItemType.BreakTool => "����",
            ItemType.ReapTool => "����",
            ItemType.WaterTool => "����",
            ItemType.CollectTool => "����",
            _ => "��"
        };
    }
}
