using UnityEngine;
using System.Collections;

//Controls if the description is being displayed or not.
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonDesc : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	public GameObject description;		//The gameobject that contains the description image

	//When the player hovers the mouse over the icon
	public void OnPointerEnter(PointerEventData eventData)
	{
		description.GetComponent<Image>().enabled = true;
	}

	//When the player stops hovering over the icon.
	public void OnPointerExit(PointerEventData eventData)
	{
		description.GetComponent<Image>().enabled = false;
	}
}
