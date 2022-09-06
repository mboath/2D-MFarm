using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace MFarm.Inventory
{
    public class SlotUI : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Header("�����ȡ")]
        [SerializeField] private Image slotImage;
        [SerializeField] private TextMeshProUGUI amountText;
        public Image slotHighlight;
        [SerializeField] private Button button;

        [Header("��������")]
        public SlotType slotType;

        //�Ƿ�ѡ��
        public bool isSelected;

        //�������
        public int slotIndex;

        //��Ʒ��Ϣ������
        public ItemDetails itemDetails;
        public int itemAmount;

        //������InventoryUI���
        private InventoryUI inventoryUI => GetComponentInParent<InventoryUI>(); 
        // private InventoryUI inventoryUI { get { GetComponentInParent<InventoryUI>(); } }

        private void Start()
        {
            isSelected = false;
            // public�������ʼ����������itemDetails == null���ж�
            if (itemDetails.itemID == 0)
            {
                UpdateEmptySlot();
            }
        }

        /// <summary>
        /// ���¸���UI����Ϣ
        /// </summary>
        /// <param name="item">��Ʒ</param>
        /// <param name="amount">����</param>
        public void UpdateSlot(ItemDetails item, int amount)
        {
            //��������
            itemDetails = item;
            itemAmount = amount;

            //���¸���UI
            slotImage.enabled = true;
            slotImage.sprite = item.itemIcon;
            amountText.text = amount.ToString();
            button.interactable = true;
        }

        /// <summary>
        /// ���¸���Ϊ��
        /// </summary>
        public void UpdateEmptySlot()
        {
            if (isSelected)
            {
                isSelected = false;
            }

            itemAmount = 0;

            slotImage.enabled = false;
            amountText.text = string.Empty;
            button.interactable = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (itemAmount == 0) return;

            isSelected = !isSelected;
            inventoryUI.UpdateSlotHightlight(slotIndex);

            if (slotType == SlotType.Player)
            {
                //֪ͨ��Ʒ��ѡ�е�״̬
                EventHandler.CallItemSlectedEvent(itemDetails, isSelected);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (itemAmount != 0)
            {
                inventoryUI.dragItemImage.enabled = true;
                inventoryUI.dragItemImage.sprite = slotImage.sprite;
                inventoryUI.dragItemImage.SetNativeSize();

                isSelected = true;
                inventoryUI.UpdateSlotHightlight(slotIndex);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            inventoryUI.dragItemImage.transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            inventoryUI.dragItemImage.enabled = false;

            //Debug.Log(eventData.pointerCurrentRaycast);
            if (eventData.pointerCurrentRaycast.gameObject != null)
            {
                if (eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotUI>() == null)
                    return;

                var targetSlot = eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotUI>();
                int targetIndex = targetSlot.slotIndex;

                //Player������Χ�ڽ���
                if (slotType == SlotType.Player && targetSlot.slotType == SlotType.Player)
                {
                    InventoryManager.Instance.SwapItem(slotIndex, targetIndex);
                }

                //������и�����ʾ
                inventoryUI.UpdateSlotHightlight(-1);
            }
/*            else //������Ʒ���ڵ���
            {
                if (itemDetails.canDrop)
                {
                    //����Ӧ��������
                    var pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));

                    EventHandler.CallInstantiateItemInScene(itemDetails.itemID, pos);
                }
            }*/
        }      
    }
}
