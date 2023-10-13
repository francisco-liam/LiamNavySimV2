using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMgr : MonoBehaviour
{
    public GameObject menu;
    public GameObject potMenu;
    public List<GameObject> menuList;

    // Start is called before the first frame update
    void Start()
    {
        menuList.Add(menu);
        menuList.Add(potMenu);
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyUp(KeyCode.Alpha1))
        {
            ToggleMenu(menu);
        }

        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            ToggleMenu(potMenu);
        }
    }

    public void ToggleMenu(GameObject currentMenu)
    {
        if(currentMenu.activeSelf)
            currentMenu.SetActive(false);
        else 
            currentMenu.SetActive(true);

        foreach(GameObject item in menuList)
        { 
            if(!GameObject.ReferenceEquals(item, currentMenu))
                item.SetActive(false);
        }
    }

    public void ResetTests()
    {
        EntityMgr.inst.ResetEntities();
        LineMgr.inst.DestroyAllLines();
        GameMgr.inst.testCases.GetComponent<TestCases>().enabled = false;
        GraphMgr.inst.DeleteAllGraphs();
    }


}
