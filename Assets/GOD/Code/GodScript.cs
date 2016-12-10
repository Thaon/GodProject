﻿using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class GodScript : MonoBehaviour
{

    #region member variables

    public GameObject m_selected;
    public GameObject m_mountain;

    #endregion

    void Start()
    {

    }

    public void Triggered()
    {
        Application.Quit();
    }

    public void Select(GameObject sel)
    {
        m_selected = sel;
    }

    public void DeSelect()
    {
        m_selected = null;
    }

    public void ParseCommand(string command)
    {
        //check if we have a target
        string upCmd = command.ToUpper();

        List<string> commTokens = upCmd.Split(' ').ToList();
        if (m_selected)
        {
            if (commTokens.Contains("SMITE") || commTokens.Contains("KILL") || commTokens.Contains("DESTROY"))
            {
                m_selected.GetComponent<Interactible>().Kill();
                m_selected = null;
            }
        }
        else
        {
            if (commTokens.Contains("MOUNTAIN"))
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, Vector3.forward, out hit))
                {
                    if (hit.collider.tag == "ground")
                        Instantiate(m_mountain, hit.point, Quaternion.identity);
                }
            }
        }
    }
}
