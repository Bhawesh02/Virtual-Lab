using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ICSelector : MonoBehaviour
{
    public GameObject ICSelection;
    private void OnMouseDown()
    {
        ICSelection.SetActive(true);
        
    }
}
