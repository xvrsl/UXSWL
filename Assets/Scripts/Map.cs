using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
[System.Serializable]
public class Map
{
    [XmlArray("Sites")]
    [XmlArrayItem("Site")]
    public List<Site> sites;
    [XmlArray("RouteMaps")]
    [XmlArrayItem("RouteMap")]
    public List<RouteMap> RouteMaps;
    [XmlIgnore]
    public Dictionary<string, Site> siteIdDictionary;
    public void Initialize()
    {
        foreach(var cur in sites)
        {
            siteIdDictionary.Add(cur.id, cur);
        }
    }

    public Site CreateSite(string id, string name, string discription, Vector3 position)
    {
        if (siteIdDictionary.ContainsKey(id))
        {
            return null;
        }
        Site newSite = new Site(id, name, discription, position);
        AddSite(newSite);
        return newSite;
    }
    public bool AddSite(Site site)
    {
        string id = site.id;
        if (siteIdDictionary.ContainsKey(id))
        {
            return false;
        }
        else
        {
            sites.Add(site);
            siteIdDictionary.Add(id, site);
            return true;
        }
    }
    public void RemoveSite(Site site)
    {
        if (sites.Contains(site))
        {
            sites.Remove(site);
        }
        if (siteIdDictionary.ContainsValue(site))
        {
            siteIdDictionary.Remove(site.id);
        }
    }

    public void Save(string path)
    {
        var serializer = new XmlSerializer(typeof(Map));
        using(var stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, this);
        }
    }
    public static Map Load(string path)
    {
        var serializer = new XmlSerializer(typeof(Map));
        using(var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as Map;
        }
    }
}

[System.Serializable]
public class Site
{
    [XmlAttribute("id")]
    public string id;
    [XmlAttribute("name")]
    public string name;
    public string discription;
    public Vector3 position;

    public Site()
    {

    }
    public Site(string id, string name, string discription, Vector3 position)
    {
        this.id = id;
        this.name = name;
        this.discription = discription;
        this.position = position;
    }
}

[System.Serializable]
public class Route
{
    [XmlAttribute("name")]
    public string name;
    public string discription;
    public string from;
    public string to;
    public Route()
    {

    }
    public Route(string from, string to)
    {
        this.from = from;
        this.to = to;
        this.name = "Unamed Route";
        this.discription = "N/A";
    }
    public Route(string name, string discription, string from, string to)
    {
        this.from = from;
        this.to = to;
        this.name = name;
        this.discription = discription;
    }
}

[System.Serializable]
public class RouteMap
{
    [XmlAttribute("name")]
    public string name;
    public string discription;
    [XmlArray("Routes")]
    [XmlArrayItem("Route")]
    public List<Route> routes;

    public Route GetRoute(string from, string to)
    {
        foreach(var curRoute in routes)
        {
            if(curRoute.from == from)
            {
                if(curRoute.to == to)
                {
                    return curRoute;
                }
            }
        }
        return null;
    }
    public Route GetRoute(string name)
    {
        foreach(var curRoute in routes)
        {
            if(curRoute.name == name)
            {
                return curRoute;
            }
        }
        return null;
    }
    public Route AddRoute(string from, string to)
    {
        Route newRoute = new Route(from, to);
        if(GetRoute(from,to)!= null)
        {
            return null;
        }
        else
        {
            routes.Add(newRoute);
            return newRoute;
        }
    }
    public void RemoveRoute(Route route)
    {
        if (routes.Contains(route))
        {
            routes.Remove(route);
        }
    }
    public void RemoveRoute(string from, string to)
    {
        RemoveRoute(GetRoute(from, to));
    }
}