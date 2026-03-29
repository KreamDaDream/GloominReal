using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class GameData
{

    public string[] inventory;
    public Vector3 point;
    public int Coin;
    public bool Solved;

    public bool bossKilled;
    public bool hatTaken;

    public bool swordTaken;
    public bool cutscenePlayed;
    public bool pirateTalkedTo;

    public GameData()
    {
        this.point = new Vector3(-8, -47, 0);
        this.inventory = new string[] { "", "", "", "", "", "", "", "", "Sword", "Chainmail Shirt" };
        this.Coin = 0;
        this.Solved = false;
        this.bossKilled = false;
        this.hatTaken = false;
        this.swordTaken = false;
        this.cutscenePlayed = false;
        this.pirateTalkedTo = false;
    }
}
