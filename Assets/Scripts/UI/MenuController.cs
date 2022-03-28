using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    GameObject backMenu; //Menu 'behind' this one

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ChangeMenu(GameObject menu)
	{
        if (menu != null)
        {
            menu.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    public void OpenConfirmationMenu(GameObject menu)
    {
        if (menu != null)
        {
            menu.SetActive(true);
        }
    }

    public void CloseConfirmationMenu(GameObject menu)
    {
        if (menu != null)
        {
            menu.SetActive(false);
        }
    }

    public void BackMenu()
    {
        if (backMenu != null)
        {
            backMenu.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
