using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    [SerializeField] GameObject playerPrefab;
    public GameObject UI;
    public GameObject healtbar;
    public List<Player> players;
    public List<Player> alive;
    public Material[] materials;
    Vector3[] positions;

    Vector3[] UIPositions = new Vector3[] 
    {   new Vector3(-14*Screen.width/30, -Screen.height/4),
        new Vector3(14*Screen.width/30 , Screen.height/4),
        new Vector3(-14*Screen.width/30, Screen.height/4),
        new Vector3(14*Screen.width/30, -Screen.height/4)
    };

    #region Singleton

    private void Awake()
    {
        instance = this;
    }

    #endregion

    


    private void Update()
    {
        foreach (Player player in players)
        {
            if (!player.GetAlive()) alive.Remove(player);
        }
    }
    public void SetMatchUp()
    {
        var spells = FindObjectsOfType<Spell>();
        if (spells.Length > 0)
        {
            foreach(Spell spell in spells)
            {
                spell.DestroySpell();
            }
        }
        Debug.Log("PlayerManager, Set Match Up");
        alive.Clear();
        for (int i = 0; i < players.Count; i++)
        {
            Debug.Log("Set Match Up :" + players[i]);
            alive.Add(players[i]);
            players[i].SetAlive(true);
            players[i].ResetHealth();
            players[i].StopMotion();
            players[i].transform.localPosition = positions[i];
            players[i].transform.localRotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
            players[i].controller.SetCanMove(false);
            players[i].combat.SetCanMove(false);
            players[i].Revive();
            players[i].PlaySpawnAnim();
        }
        SetUIUp();
    }
    public void SetUIUp()
    {
        UI = transform.parent.Find("UI").gameObject;
        for (int i = 0; i < players.Count; i++)
        {
            //Create and set healthBar
            var bar = Instantiate(healtbar);
            bar.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width / 30, Screen.height / 3);
            bar.transform.SetParent(UI.transform.Find("Canvas"));
            bar.GetComponent<Healthometer>().setPlayer(players[i]);
            bar.GetComponent<Healthometer>().setMaxHealth(200f);
            //Set Color to player and Healthbar
            //players[i].SetHatColor(materials[i]);
            
            var fill = bar.transform.Find("fill").gameObject;
            fill.GetComponent<Image>().color = UIColor.instance.GetColorByString(materials[i].name);
            var hearth = bar.transform.Find("hearth").gameObject;
            hearth.GetComponent<Image>().color = UIColor.instance.GetColorByString("light" + UppercaseFirst(materials[i].name));

            //Place healthbar
            var rect = bar.GetComponent<RectTransform>();
            rect.anchoredPosition = UIPositions[i];
        }

        List<Inputs.PlayerInputHandlerMenu> inputMenu = new List<Inputs.PlayerInputHandlerMenu>();
        inputMenu.AddRange(FindObjectsOfType<Inputs.PlayerInputHandlerMenu>());
        foreach(Inputs.PlayerInputHandlerMenu menuHandler in inputMenu)
        {
            menuHandler.enabled = false;
        }

        List<Inputs.PlayerInputHandler> inputGame = new List<Inputs.PlayerInputHandler>();
        inputGame.AddRange(FindObjectsOfType<Inputs.PlayerInputHandler>());
        foreach (Inputs.PlayerInputHandler gameHandler in inputGame)
        {
            gameHandler.enabled = true;
        }
    }
    
    public void SetCanMove()
    {
        Debug.Log("PlayerManager, Set Can Move");
        foreach (Player player in players)
        {
            player.controller.SetCanMove(true);
            player.combat.SetCanMove(true);
        }

    }
    string UppercaseFirst(string s)
    {
        if (string.IsNullOrEmpty(s))
        {
            return string.Empty;
        }
        char[] a = s.ToCharArray();
        a[0] = char.ToUpper(a[0]);
        return new string(a);
    }
    public void SetSpawnPositions(Vector3[] pos)
    {
        Debug.Log("PlayerManager, SetSpawnPositions : PositionsLength = " + pos.Length );
        positions = pos;
    }
    public void SpawnPlayers(List<GameObject> pls)
    {
        Debug.Log("PlayerManager, SpawnPlayer : Count : " + pls.Count);

        Debug.Log("PlayerManager, SpawnPlayer : PositionsLength : " + positions.Length);
        for (int i = 0; i < pls.Count; i++)
        {
            Debug.Log("PlayerManager, SpawnPlayer : Instantiate Player");
            var obj = Instantiate(pls[i]);
            obj.transform.SetParent(this.transform);
            var player = obj.GetComponent<Player>();
            player.SetPlayerIndex(i);
            players.Add(player);
        }
        SetMatchUp();

    }
    public void DeletePlayers()
    {
        foreach (Player player in players)
        {
            Destroy(player.gameObject);
        }
        players.Clear();
        alive.Clear();
    }

}

