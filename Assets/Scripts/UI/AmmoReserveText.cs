using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoReserveText : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject _ammoTextContainer;
    [SerializeField] TMP_Text _ammoReserveText;
    [SerializeField] TMP_Text _ammoInventoryText;


    public void SetText(string ammoReserve, string ammoInventory)
    {
        _ammoInventoryText.text = ammoInventory;
        _ammoReserveText.text = ammoReserve;
    }

    public void ActivateAmmoCountText()
    {
        _ammoTextContainer.SetActive(true);
    }

    public void DeactivateAmmoCountText()
    {
        _ammoTextContainer.SetActive(false);
    }
}
