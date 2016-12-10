using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class GodScript : MonoBehaviour {

    #region member variables

    public GameObject m_selected;

    #endregion

    void Start () {
	
	}
	
	void Update () {
	
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
        if (m_selected != null)
        {
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
        }
    }
}
