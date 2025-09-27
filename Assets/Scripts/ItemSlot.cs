using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount <= GameManager.GMInstance.maxMagicInMagicfield && gameObject.name == "Magicfield" && GameManager.GMInstance.cardsInMagicfield.Contains(DragDrop.itemBeingDragged))
        {
            print("c1");

            Dropped();
        }
        else if (transform.childCount <= GameManager.GMInstance.maxTroopInBattlefield && gameObject.name == "Battlefield" && GameManager.GMInstance.cardsInBattlefield.Contains(DragDrop.itemBeingDragged))
        {
            print("c1");

            Dropped();
        }
        else if (transform.childCount <= GameManager.GMInstance.handSize && gameObject.name == "Handfield" && GameManager.GMInstance.cardsInHandfield.Contains(DragDrop.itemBeingDragged))
        {
            print("c1");

            Dropped();
        }
    }

    private void Dropped()
    {
        if (transform.childCount == 1)
        {
            DragDrop.itemBeingDragged.transform.SetParent(transform);
            DragDrop.itemBeingDragged.transform.SetSiblingIndex(0);
        }
        else if (DragDrop.itemBeingDragged.transform.position.x < transform.GetChild(0).position.x)
        {
            DragDrop.itemBeingDragged.transform.SetParent(transform);
            DragDrop.itemBeingDragged.transform.SetSiblingIndex(0);
        }
        else if (DragDrop.itemBeingDragged.transform.position.x < transform.GetChild(transform.childCount - 1).position.x)
        {
            DragDrop.itemBeingDragged.transform.SetParent(transform);
            DragDrop.itemBeingDragged.transform.SetSiblingIndex(transform.childCount - 1);
        }
        else
        {
            for (int i = 0; i < transform.childCount - 1; i++)
            {
                if (DragDrop.itemBeingDragged.transform.position.x > transform.GetChild(i).position.x && DragDrop.itemBeingDragged.transform.position.x < transform.GetChild(i + 1).position.x)
                {
                    DragDrop.itemBeingDragged.transform.SetParent(transform);
                    DragDrop.itemBeingDragged.transform.SetSiblingIndex(i + 1);
                    break;
                }
            }
        }
    }
}
