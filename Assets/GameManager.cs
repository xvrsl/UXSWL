using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameStatus status
    {
        get
        {
            return instance.gameStatus;
        }
    }
    public static GameManager instance;
    public GameStatus gameStatus;
    public Map map;
    public void LoadGameStatus(string path)
    {
        gameStatus = GameStatus.Load(path);
    }
    public void LoadMap(string path)
    {
        map = Map.Load(path);
    }

	// Use this for initialization
	void Start () {
		if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
