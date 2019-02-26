using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DramaScriptBehaviour : MonoBehaviour {
    [SerializeField]
    DramaScriptCore core = new DramaScriptCore();
    [TextArea(10,500)]
    public string script;

    public void Load(string script)
    {
        this.script = script;
        core.Initialize(script.Split('\n'));
    }
    public void Continue()
    {
        core.Next();
    }

	// Use this for initialization
	void Start () {
        Load(this.script);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Continue();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            core.hold = false;
            core.Run();
        }
	}
}
