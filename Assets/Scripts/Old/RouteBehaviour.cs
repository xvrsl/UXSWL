using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteBehaviour : MonoBehaviour {
    public SiteBehaviour masterSite;
    public SiteBehaviour targetSite;
    public float cost;
    public List<ConditionProbabilityEvent> conditionProbabilityEvents;
	// Use this for initialization
	void Start () {
		if(masterSite == null)
        {
            masterSite = this.transform.parent.GetComponent<SiteBehaviour>();
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(masterSite.transform.position, targetSite.transform.position);
    }
}
