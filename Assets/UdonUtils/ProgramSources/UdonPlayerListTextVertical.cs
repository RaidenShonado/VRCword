// UdonPlayerListTextVertical
// By HARunaDev
// You may modify this script but do not redistribute.
// Read the README.txt at UdonUtils/License before usage.
// Report the bug at https://discord.gg/haVuksQmEk or harunadev@gmail.com or Twitter DM @tw_harunadev

// Version 1, Updated at 2021/3/6 11:00 GMT+9

using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;


public class UdonPlayerListTextVertical : UdonSharpBehaviour
{
    [Header("Do not touch these variable")]

    public Text MainTextObject;

    [Header("UdonPlayerListText Vertical")]
    [Header("by HARunaDev")]
    [Header("Version 1 (2021/3/6 11:00 GMT+9)")]
    [Header("")]
    [Header("!! Please open the 'Players' Menu and set the Count as this world's player capacity.")]

    [Tooltip("In future version, this may be automatically configured, But NOT NOW!")]
    public VRCPlayerApi[] Players;

    [Header("Hover the cursor to see the tooltip message.")]
    [Header("")]

    [Header("Player List Setting")]

    [Tooltip("Font size of the number at the player list.")]
    public int PlayerListNumberFontSize = 30;

    [Tooltip("This text will show after the player's name, when that corresponding user is the owner of the instance.")]
    public string OwnerText = "OWNER";

    [Tooltip("This color will be used on the text of the top option 'Owner Text'.")]
    public Color OwnerColor = Color.yellow;

    [Tooltip("This text will show after the player's name, when that corresponding user is equal to the user who're looking at the text.")]
    public string YourselfText = "YOU";

    [Tooltip("This color will be used on the text of the top option 'Yourself Text'.")]
    public Color YourselfColor = Color.yellow;

    [Tooltip("This text will show after the player's name, when that corresponding user is playing with HMD and VR controller(s).")]
    public string VRText = "VR";

    [Tooltip("This text will show after the player's name, when that corresponding user is playing on Mouse / Keyboard.")]
    public string PCText = "PC";

    [Header("Total Count Setting")]

    [Tooltip("Font size of the Total count at the bottom of the text.")]
    public int TotalCountFontSize = 39;

    [Tooltip("This text will show before the player count.")]
    public string TotalCountPrefix = "Total ";

    [Tooltip("Font size of the number of Total count at the bottom of the text.")]
    public int TotalCountNumberFontSize = 58;

    [Tooltip("This text will show after the player count, when the player count is only one.")]
    public string TotalCountPostfixSingular = " Player";

    [Tooltip("This text will show after the player count, when the player count is more than one.")]
    public string TotalCountPostfixPlural = " Players";

    [Tooltip("Check this box to show all possible numbers (except inside player's name) as kanji in Japanese.")]
    public bool TotalCountNumberKanji = false;
    

    void Start()
    {
        UpdatePlayerList(null);
    }

    private void UpdatePlayerList(VRCPlayerApi player)
    {
        MainTextObject.GetComponent<Text>().supportRichText = true;
        MainTextObject.GetComponent<Text>().text = GetPlayerList(player);
    }

    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        UpdatePlayerList(null);
    }

    public override void OnPlayerLeft(VRCPlayerApi player)
    {
        UpdatePlayerList(player);
    }

    string GetPlayerList(VRCPlayerApi player)
    {
        int validPlayer = 0;
        string strbuild = "";

        Players = VRCPlayerApi.GetPlayers(Players);

        if (player != null) for (int i = 0; i < Players.Length; i++)
        {
                if (Players[i] == player) Players[i] = null;
        }



        for (int i = 0; i < Players.Length ; i++)
        {
            if (Players[i] != null)
            {
                validPlayer++;
                strbuild += "<b><size=";
                strbuild += PlayerListNumberFontSize.ToString();
                strbuild += ">";
                strbuild += validPlayer;
                strbuild += "</size></b>. ";
                strbuild += Players[i].displayName;
                if (Players[i].IsUserInVR())
                {
                    strbuild += " <i>[";
                    strbuild += VRText;
                    strbuild += "]</i>";
                } else
                {
                    strbuild += " <i>[";
                    strbuild += PCText;
                    strbuild += "]</i>";
                }
                if (Networking.GetOwner(this.gameObject).Equals(Players[i]))
                {
                    strbuild += "   <b><color=#";
                    strbuild += ColorToStr(OwnerColor);
                    strbuild += ">";
                    strbuild += OwnerText;
                    strbuild += "</color></b>";
                }
                if (Networking.LocalPlayer.Equals(Players[i]))
                {
                    strbuild += "   <b><color=#";
                    strbuild += ColorToStr(YourselfColor);
                    strbuild += ">";
                    strbuild += YourselfText;
                    strbuild += "</color></b>";
                }
                strbuild += "\n";
            }

        }

        strbuild += "<size=";
        strbuild += TotalCountFontSize.ToString();
        strbuild += ">";
        strbuild += TotalCountPrefix;
        strbuild += "<b><size=";
        strbuild += TotalCountNumberFontSize.ToString();
        strbuild += ">";


        strbuild += TotalValidPlayer(validPlayer);


        if (validPlayer > 1)
        { 
            strbuild += "</size></b>";
            strbuild += TotalCountPostfixPlural;
            strbuild += "</size>";
        } else
        {
            strbuild += "</size></b>";
            strbuild += TotalCountPostfixSingular;
            strbuild += "</size>";
        }
        

        return strbuild;
    }

    string ColorToStr(Color color) {
        string r = ((int)(color.r * 255)).ToString("X2");
        string g = ((int)(color.g * 255)).ToString("X2");
        string b = ((int)(color.b * 255)).ToString("X2");
        string a = ((int)(color.a * 255)).ToString("X2");
        string result = string.Format("{0}{1}{2}{3}", r, g, b, a);
        return result;
    }

    string TotalValidPlayer(int count)
    {
        if (TotalCountNumberKanji)
        {
            string returntext = "";
            if (count > 9)
            {
                returntext += NumToKanji(count.ToString().Substring(0, 1));
                returntext += "十";
                if (NumToKanji(count.ToString().Substring(0, 1)) != "0") returntext += NumToKanji(count.ToString().Substring(1));
            } else
            {
                returntext += NumToKanji(count.ToString());
            }
            return returntext;
        } else
        {
            return count.ToString();
        }
    }

    string NumToKanji(string num)
    {
        if (num == "0")
        {
            return "零";
        } else if (num == "1")
        {
            return "一";
        } else if (num == "2")
        {
            return "二";
        } else if (num == "3")
        {
            return "三";
        } else if (num == "4")
        {
            return "四";
        } else if (num == "5")
        {
            return "五";
        } else if (num == "6")
        {
            return "六";
        } else if (num == "7")
        {
            return "七";
        } else if (num == "8")
        {
            return "八";
        } else 
        {
            return "九";
        }
    }
}
