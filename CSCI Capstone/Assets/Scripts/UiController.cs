using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    public Image[] itemSlots; // Array to hold references to item slot images
    
    /*
    Item indecies
    PointCharges = 0
    LineCharges = 1
    VolumneCharges = 2
    */
    public int selectedItemIndex = 0; // Index of the currently selected item
    

    void Update()
    {
        // Handle scroll wheel input to change selected item
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0 && !(Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl)))
        {
            ChangeSelectedItem(scroll);
        }
    }

    void ChangeSelectedItem(float scrollDirection)
    {
        // Update the selected item index based on scroll direction
        selectedItemIndex -= (int)Mathf.Sign(scrollDirection);
        // Ensure the index stays within bounds
        selectedItemIndex = Mathf.Clamp(selectedItemIndex, 0, itemSlots.Length - 1);

        // Update selected item overlay images
        UpdateSelectedItemOverlay();
    }

    void UpdateSelectedItemOverlay()
    {
        // Activate selected item overlay for the currently selected item
        for (int i = 0; i < itemSlots.Length; i++)
        {
            bool isSelected = (i == selectedItemIndex);
            itemSlots[i].transform.GetChild(0).gameObject.SetActive(isSelected);
        }
    }
}