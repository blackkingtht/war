using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card  {
    private string name;
    private Sprite img;
    private int energy;
    private SoldierEnum soldierType;
    private string role_info;

    public string Name
    {
        get
        {
            return name;
        }

        set
        {
            name = value;
        }
    }

    public Sprite Img
    {
        get
        {
            return img;
        }

        set
        {
            img = value;
        }
    }

    public int Energy
    {
        get
        {
            return energy;
        }

        set
        {
            energy = value;
        }
    }

    public SoldierEnum SoldierType
    {
        get
        {
            return soldierType;
        }

        set
        {
            soldierType = value;
        }
    }

    public string Role_info
    {
        get
        {
            return role_info;
        }

        set
        {
            role_info = value;
        }
    }
}
