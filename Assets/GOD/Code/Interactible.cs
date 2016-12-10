using UnityEngine;
using System.Collections;

public class Interactible : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Kill()
    {
        Destroy(this.gameObject);
    }

    public void SelectMe()
    {
        GodScript gs = FindObjectOfType<GodScript>() as GodScript;
        gs.Select(this.gameObject);
    }

    public void DeselectMe()
    {
        GodScript gs = FindObjectOfType<GodScript>() as GodScript;
        gs.DeSelect();
    }
}
