using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(RouteBehaviour))]
public class RouteRenderer : MonoBehaviour {
    public LineRenderer lineRenderer;
    public RouteBehaviour routeBehaviour;

	// Use this for initialization
	void Start () {
	    if(lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }	
        if(routeBehaviour == null)
        {
            routeBehaviour = GetComponent<RouteBehaviour>();
        }
        Refresh();
	}
	
	// Update is called once per frame
	void Update () {
        Refresh();
	}

    void Refresh()
    {
        lineRenderer.SetPosition(0, routeBehaviour.masterSite.transform.position);
        lineRenderer.SetPosition(1, routeBehaviour.targetSite.transform.position);
    }
}
