using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SiteBehaviour : MonoBehaviour {
    public string siteName;
    public string discription;
    public List<ConditionProbabilityEvent> conditionProbabilityEvents;
    RouteBehaviour[] _routesChache;
    public RouteBehaviour[] routes
    {
        get
        {
            if(_routesChache == null)
            {
                _routesChache = GetRoutes();
            }
            return _routesChache;
        }
    }
    public RouteBehaviour[] GetRoutes()
    {
        Transform routesParent = this.transform.Find("Routes");
        return routesParent.GetComponentsInChildren<RouteBehaviour>();
    }

	// Use this for initialization
	void Start () {
        _routesChache = GetRoutes();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnDrawGizmosSelected()
    {
        for(int i = 0; i < routes.Length; i++)
        {
            RouteBehaviour curRoute = _routesChache[i];
            Gizmos.DrawLine(curRoute.masterSite.transform.position, curRoute.targetSite.transform.position);
        }
    }
}
