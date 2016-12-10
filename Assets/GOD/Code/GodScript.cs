using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class GodScript : MonoBehaviour
{

    #region member variables

    public GameObject m_selected;
    public GameObject m_mountain;

    private SpeechRecognizer m_sr;

    #endregion

    void Start()
    {
        m_sr = GetComponent<SpeechRecognizer>();
    }

    public void Triggered()
    {
        m_sr.Listen();
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
            if (commTokens.Contains("MOUNTAIN") || commTokens.Contains("RAISE"))
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, Camera.main.transform.forward, out hit))
                {
                    if (hit.collider.tag == "ground")
                        Instantiate(m_mountain, hit.point, Quaternion.identity);
                }
            }
        }
    }
}
