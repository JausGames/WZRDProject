using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Infos")]
    [SerializeField] private int nbPlayers;
    [SerializeField] private List<Inputs.PlayerInputHandlerMenu> inputs = new List<Inputs.PlayerInputHandlerMenu>();
    [SerializeField] GameObject MatchManagerPrefab;
    [SerializeField] List<GameObject> playerTypes = new List<GameObject>();

    [Header("Menus")]
    [SerializeField] List<GameObject> menus = new List<GameObject>();

    [Header("Menus - Player")]
    [SerializeField] List<GameObject> players = new List<GameObject>();
    [SerializeField] private List<Text> playerText = new List<Text>();
    [SerializeField] private List<Image> playerPicture = new List<Image>();
    [SerializeField] private List<Button> PlayerSelect = new List<Button>();
    [SerializeField] private Button next;
    [SerializeField] private Button quit;
    [SerializeField] float ANIM_TIME = 0.5f;
    [SerializeField] float animTime = 0f;

    [Header("Menus - Settings")]
    [SerializeField] private Button play;
    [SerializeField] private Button back;
    [Header("Menus - Maps")]
    [SerializeField] MapHandler mapHandler;


    void Start()
    {
        ChangeMenu(0);
        next.onClick.AddListener(delegate { ChangeMenu(2); });
        back.onClick.AddListener(delegate { ChangeMenu(1); });
        play.onClick.AddListener(ChangeScene);
        quit.onClick.AddListener(QuitGame);

        for (int i = 0; i < playerText.Count; i++)
        {
            Debug.Log(playerText[i].text);
            playerText[i].text = playerTypes[0].name;
            playerPicture[i].sprite = playerTypes[0].GetComponent<SpellManager>().GetPicture();
        }
        /*for (int i = 0; i < PlayerSelect.Count; i++)
        {
            Debug.Log("Debug ok : " + i);
            PlayerSelect[i].onClick.AddListener(delegate { ChangePlayer(i); });
        }*/
        PlayerSelect[0].onClick.AddListener(delegate { ChangePlayer(0); });
        PlayerSelect[1].onClick.AddListener(delegate { ChangePlayer(1); });
        PlayerSelect[2].onClick.AddListener(delegate { ChangePlayer(2); });
        PlayerSelect[3].onClick.AddListener(delegate { ChangePlayer(3); });
        PlayerSelect[4].onClick.AddListener(delegate { ChangePlayer(4); });
        PlayerSelect[5].onClick.AddListener(delegate { ChangePlayer(5); });
        PlayerSelect[6].onClick.AddListener(delegate { ChangePlayer(6); });
        PlayerSelect[7].onClick.AddListener(delegate { ChangePlayer(7); });

    }
    private void Update()
    {
        var oldNbPlayer = nbPlayers;
        List<Inputs.PlayerInputHandlerMenu> inputLocal = new List<Inputs.PlayerInputHandlerMenu>(); 
        inputLocal.AddRange(FindObjectsOfType<Inputs.PlayerInputHandlerMenu>());
        inputs.Clear();
        inputs.AddRange(inputLocal);
        nbPlayers = inputs.Count;
        if (nbPlayers != oldNbPlayer) animTime = Time.time + ANIM_TIME;

        if (animTime < Time.time) return;

        if (inputs.Count >= 2) next.interactable = true;
        else if (inputs.Count >= 1 && menus[0].activeSelf) ChangeMenu(1);
        else next.interactable = false;
        var nb4UI = Mathf.Max(nbPlayers, 1);
        var UIPadding = 0.30f - ((float)nbPlayers * 0.05f);
        for (int i = 0; i < inputs.Count; i++)
        {
            players[i].SetActive(true);
            players[i].GetComponent<RectTransform>().anchorMin = Vector2.Lerp( new Vector2(UIPadding + ((1f - UIPadding * 2f) / nb4UI) * (float) i, 0f), players[i].GetComponent<RectTransform>().anchorMin, 0.8f);
            players[i].GetComponent<RectTransform>().anchorMax = Vector2.Lerp( new Vector2(UIPadding + ((1f - UIPadding * 2f) / nb4UI) * (float) (i + 1f), 1f), players[i].GetComponent<RectTransform>().anchorMax, 0.8f); ;
            PlayerSelect[2 * i].interactable = true;
            PlayerSelect[2 * i + 1].interactable = true;
        }
        for (int i = inputs.Count; i < 4; i++)
        {
            players[i].SetActive(false);
            PlayerSelect[2 * i].interactable = false;
            PlayerSelect[2 * i + 1].interactable = false;
        }
    }

    private void ChangeScene()
    {
        if (MatchManager.instance == null) Instantiate(MatchManagerPrefab);
        var map = mapHandler.GetMapByName("Map01");
        var spawnwPos = map.GetPositions();
        PlayerManager.instance.SetSpawnPositions(spawnwPos);
        var list = new List<GameObject>();
        for (int i = 0; i < inputs.Count; i++)
        {
            list.Add(FindNamePlayer(playerText[i].text));
        }
        Debug.Log("ChangeScene");
        MatchManager.instance.SetPlayers(list);
        SceneManager.LoadScene("BaseGame");
    }


    public void ChangeMenu(int menuOrder)
    {
        foreach (GameObject menu in menus)
        {
            menu.SetActive(false);
        }
        menus[menuOrder].SetActive(true);
    }
    public void DisplayMenu(bool value)
    {
        if (!value)
            foreach (GameObject menu in menus)
            {
                menu.SetActive(value);
            }
        else menus[menus.Count - 1].SetActive(value);

        this.enabled = value;
    }

    private void ChangePlayer(int nb)
    {
        Debug.Log("Debug ok : ChangePlayer, nb = " + nb);
        switch (nb)
        {
            case 0:
                if (FindNameID(playerText[0].text) == 0) return;
                playerText[0].text = FindPlayerName(playerTypes[FindNameID(playerText[0].text) - 1]);
                playerPicture[0].sprite = FindSprite(FindNameID(playerText[0].text) - 1, -1);
                break;
            case 1:
                if (FindNameID(playerText[0].text) == playerTypes.Count - 1) return;
                playerText[0].text = FindPlayerName(playerTypes[FindNameID(playerText[0].text) + 1]);
                playerPicture[0].sprite = FindSprite(FindNameID(playerText[0].text) + 1, 1);
                break;
            case 2:
                if (FindNameID(playerText[1].text) == 0) return;
                playerText[1].text = FindPlayerName(playerTypes[FindNameID(playerText[1].text) - 1]);
                playerPicture[1].sprite = FindSprite(FindNameID(playerText[1].text) - 1, -1);
                break;
            case 3:
                if (FindNameID(playerText[1].text) == playerTypes.Count - 1) return;
                playerText[1].text = FindPlayerName(playerTypes[FindNameID(playerText[1].text) + 1]);
                playerPicture[1].sprite = FindSprite(FindNameID(playerText[1].text) + 1, 1);
                break;
            case 4:
                if (FindNameID(playerText[2].text) == 0) return;
                playerText[2].text = FindPlayerName(playerTypes[FindNameID(playerText[2].text) - 1]);
                playerPicture[2].sprite = FindSprite(FindNameID(playerText[2].text) - 1, -1);
                break;
            case 5:
                if (FindNameID(playerText[2].text) == playerTypes.Count - 1) return;
                playerText[2].text = FindPlayerName(playerTypes[FindNameID(playerText[2].text) + 1]);
                playerPicture[2].sprite = FindSprite(FindNameID(playerText[2].text) + 1, 1);
                break;
            case 6:
                if (FindNameID(playerText[3].text) == 0) return;
                playerText[3].text = FindPlayerName(playerTypes[FindNameID(playerText[3].text) - 1]);
                playerPicture[3].sprite = FindSprite(FindNameID(playerText[3].text) - 1, -1);
                break;
            case 7:
                if (FindNameID(playerText[3].text) == playerTypes.Count - 1) return;
                playerText[3].text = FindPlayerName(playerTypes[FindNameID(playerText[3].text) + 1]);
                playerPicture[3].sprite = FindSprite(FindNameID(playerText[3].text) + 1, 1);
                break;
            default:
                Debug.Log("Player selection Switch Case not correct");
                break;
        }
    }
    private string FindPlayerName(GameObject player)
    {
        return player.name;
    }
    private Sprite FindSprite(int playerNb, int prevOrNext)
    {
        if (playerNb >= playerTypes.Count || playerNb <= 0) return playerTypes[playerNb - prevOrNext].GetComponent<SpellManager>().GetPicture();
        else return playerTypes[playerNb - prevOrNext].GetComponent<SpellManager>().GetPicture();
    }
    private GameObject FindNamePlayer(string player)
    {
        foreach(GameObject obj in playerTypes)
        {
            if (obj.name == player) return obj;
        }
        return null;
    }
    private int FindNameID(string name)
    {
        foreach(GameObject obj in playerTypes)
        {
            if (name == obj.name) return playerTypes.IndexOf(obj);
        }
        return -1;
    }
    private void QuitGame()
    {
        Application.Quit();
    }
}
