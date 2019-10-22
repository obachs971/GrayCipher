using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KModkit;
using System;
using System.Linq;

public class ultimateCipher : MonoBehaviour {
    
    public TextMesh[] screenTexts;
    public string[] wordList;
    public KMBombInfo Bomb;
    public KMBombModule module;
    public AudioClip[] sounds;
    public KMAudio Audio;
    public TextMesh submitText;
   
    
    private string[] matrixWordList =
      {
                "ACID",
                "BUST",
                "CODE",
                "DAZE",
                "ECHO",
                "FILM",
                "GOLF",
                "HUNT",
                "ITCH",
                "JURY",
                "KING",
                "LIME",
                "MONK",
                "NUMB",
                "ONLY",
                "PREY",
                "QUIT",
                "RAVE",
                "SIZE",
                "TOWN",
                "URGE",
                "VERY",
                "WAXY",
                "XYLO",
                "YARD",
                "ZERO",
                "ABORT",
                "BLEND",
                "CRYPT",
                "DWARF",
                "EQUIP",
                "FANCY",
                "GIZMO",
                "HELIX",
                "IMPLY",
                "JOWLS",
                "KNIFE",
                "LEMON",
                "MAJOR",
                "NIGHT",
                "OVERT",
                "POWER",
                "QUILT",
                "RUSTY",
                "STOMP",
                "TRASH",
                "UNTIL",
                "VIRUS",
                "WHISK",
                "XERIC",
                "YACHT",
                "ZEBRA",
                "ADVICE",
                "BUTLER",
                "CAVITY",
                "DIGEST",
                "ELBOWS",
                "FIXURE",
                "GOBLET",
                "HANDLE",
                "INDUCT",
                "JOKING",
                "KNEADS",
                "LENGTH",
                "MOVIES",
                "NIMBLE",
                "OBTAIN",
                "PERSON",
                "QUIVER",
                "RACHET",
                "SAILOR",
                "TRANCE",
                "UPHELD",
                "VANISH",
                "WALNUT",
                "XYLOSE",
                "YANKED",
                "ZODIAC",
                "ALREADY",
                "BROWSED",
                "CAPITOL",
                "DESTROY",
                "ERASING",
                "FLASHED",
                "GRIMACE",
                "HIDEOUT",
                "INFUSED",
                "JOYRIDE",
                "KETCHUP",
                "LOCKING",
                "MAILBOX",
                "NUMBERS",
                "OBSCURE",
                "PHANTOM",
                "QUIETLY",
                "REFUSAL",
                "SUBJECT",
                "TRAGEDY",
                "UNKEMPT",
                "VENISON",
                "WARSHIP",
                "XANTHIC",
                "YOUNGER",
                "ZEPHYRS",
                "ADVOCATE",
                "BACKFLIP",
                "CHIMNEYS",
                "DISTANCE",
                "EXPLOITS",
                "FOCALIZE",
                "GIFTWRAP",
                "HOVERING",
                "INVENTOR",
                "JEALOUSY",
                "KINSFOLK",
                "LOCKABLE",
                "MERCIFUL",
                "NOTECARD",
                "OVERCAST",
                "PERILOUS",
                "QUESTION",
                "RAINCOAT",
                "STEALING",
                "TREASURY",
                "UPDATING",
                "VERTICAL",
                "WISHBONE",
                "XENOLITH",
                "YEARLONG",
                "ZEALOTRY"
        };

    private string[][] pages;
    private string answer;
    private int page;
    private bool submitScreen;
    static int moduleIdCounter = 1;
    int moduleId;
    private bool moduleSolved;

    public KMSelectable leftArrow;
    public KMSelectable rightArrow;
    public KMSelectable submit;
    public KMSelectable[] keyboard;
    void Awake()
    {
        moduleId = moduleIdCounter++;
        leftArrow.OnInteract += delegate () { left(leftArrow); return false; };
        rightArrow.OnInteract += delegate () { right(rightArrow); return false; };
        submit.OnInteract += delegate () { submitWord(submit); return false; };
        foreach(KMSelectable keybutton in keyboard)
        {
            KMSelectable pressedButton = keybutton;
            pressedButton.OnInteract += delegate () { letterPress(pressedButton); return false; };
        }
    }
        // Use this for initialization
        void Start ()
    
    {
        submitText.text = "1";
        //Generating random word
        answer = wordList[UnityEngine.Random.Range(0, wordList.Length)].ToUpper();
        Debug.LogFormat("[Gray Cipher #{0}] Generated Word: {1}", moduleId, answer);
       
        pages = new string[1][];
        pages[0] = new string[3];
        pages[0][0] = "";
        pages[0][1] = "";
        pages[0][2] = "";
        string encrypt = graycipher(answer);
        pages[0][0] = encrypt.ToUpper();
        page = 0;
        getScreens();
    }
    string graycipher(string word)
    {
        Debug.LogFormat("[Gray Cipher #{0}] Begin Portax Encryption", moduleId);
        string encrypt = PortaxEnc(word.ToUpper());
        Debug.LogFormat("[Gray Cipher #{0}] Begin Bit Switch Encryption", moduleId);
        encrypt = BitSwitchEnc(encrypt.ToUpper());
        Debug.LogFormat("[Gray Cipher #{0}] Begin Ragbaby Encryption", moduleId);
        encrypt = RagbabyEnc(encrypt.ToUpper());
        return encrypt;
    }
    string PortaxEnc(string word)
    {
        char letter = Bomb.GetSerialNumberLetters().ToArray()[1];
        string[] slides =
        {
            "ABCDEFGHIJKLM",
            "NOPQRSTUVWXYZ",
            "ACEGIKMOQSUWY",
            "BDFHJLNPRTVXZ"
        };
        int num = slides[2].IndexOf(letter);
        if(num < 0)
            num = slides[3].IndexOf(letter);
        slides[1] = slides[1].Substring(num) + "" + slides[1].Substring(0, num);
        slides[2] = slides[2].Substring(num) + "" + slides[2].Substring(0, num);
        slides[3] = slides[3].Substring(num) + "" + slides[3].Substring(0, num);
        Debug.LogFormat("[Gray Cipher #{0}] Slides:", moduleId);
        Debug.LogFormat("[Gray Cipher #{0}] {1}", moduleId, slides[0]);
        Debug.LogFormat("[Gray Cipher #{0}] {1}", moduleId, slides[1]);
        Debug.LogFormat("[Gray Cipher #{0}] {1}", moduleId, slides[2]);
        Debug.LogFormat("[Gray Cipher #{0}] {1}", moduleId, slides[3]);
        char[] letterenc = new char[6];
        for (int aa = 0; aa < 3; aa++)
        {
            int num1 = slides[0].IndexOf(word[aa]);
            int col1 = 0;
            if(num1 < 0)
            {
                num1 = slides[1].IndexOf(word[aa]);
                col1 = 1;
            }
            int num2 = slides[2].IndexOf(word[aa + 3]);
            int col2 = 2;
            if (num2 < 0)
            {
                num2 = slides[3].IndexOf(word[aa + 3]);
                col2 = 3;
            }
            if(num1 == num2)
            {
                letterenc[aa] = slides[(col1 + 1) % 2][num1];
                letterenc[aa + 3] = slides[((col2 - 1) % 2) + 2][num2];
            }
            else
            {
                letterenc[aa] = slides[col1][num2];
                letterenc[aa + 3] = slides[col2][num1];
            }
            Debug.LogFormat("[Gray Cipher #{0}] {1}{2} -> {3}{4}", moduleId, word[aa], word[aa + 3], letterenc[aa], letterenc[aa + 3]);
        }
        string encrypt = letterenc[0] + "" + letterenc[1] + "" + letterenc[2] + "" + letterenc[3] + "" + letterenc[4] + "" + letterenc[5];
        return encrypt;
    }
    string BitSwitchEnc(string word)
    {
       
            string encrypt = "";
            string order = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string p7alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string[] scramblers = {
              "21453", "21534", "31524", "31452", "41523", "41532", "51234",
              "51423", "51432", "23154", "25134", "24153", "34152", "35124",
              "45123", "45132", "54123", "54132", "24513", "25413", "34512",
              "35214", "35412", "43512", "45213", "53412", "54213", "24531",
              "25431", "34521", "35421", "43251", "43521", "45231", "53421",
              "54231"
    };
        string[] binary = {
            "00001", "00010", "00011", "00100", "00101", "00110", "00111",
            "01000", "01001", "01010", "01011", "01100", "01101", "01110",
            "01111", "10000", "10001", "10010", "10011", "10100", "10101",
            "10110", "10111", "11000", "11001", "11010", "11011", "11100",
            "11101", "11110", "11111"
         };
        string scrambler = scramblers[order.IndexOf(Bomb.GetSerialNumber()[0])];
        string binarytrue = "" ;
        Debug.LogFormat("[Gray Cipher #{0} ] Scrambler: {1}", moduleId, scrambler);
        for (int aa = 0; aa < 6; aa++)
        { 
            string binarylet = binary[p7alphabet.IndexOf(word[aa])];
            string convbin = scrambling(binarylet, scrambler);
            for (int cc = 0; cc < binary.Length; cc++)
            {
                if (convbin.Equals(binary[cc]))
                {
                    if (cc >= 26)
                        binarytrue = binarytrue + "1";
                    else
                        binarytrue = binarytrue + "0";
                    encrypt = encrypt + "" + p7alphabet[cc % 26];
                    break;
                }
            }
            Debug.LogFormat("[Gray Cipher #{0} ] {1} -> {2} -> {3} -> {4}", moduleId, word[aa], binarylet, convbin, encrypt[aa]);
        }
        pages[0][2] = binarytrue;
        return encrypt;
    }
    string scrambling(string bin, string scrambler)
    {
        char[] c = new char[5];
        for (int aa = 0; aa < 5; aa++)
            c[scrambler[aa] - '0' - 1] = bin[aa];
        return (c[0] + "" + c[1] + "" + c[2] + "" + c[3] + "" + c[4]);
    }
    string RagbabyEnc(string word)
    {
        string kw = matrixWordList[UnityEngine.Random.Range(0, matrixWordList.Length)];
        pages[0][1] = kw.ToUpper();
        string key = getKey(kw.ToUpper(), "ABCDEFGHIJKLMNOPQRSTUVWXYZ", Bomb.GetOffIndicators().Count() % 2 == 0);
        Debug.LogFormat("[Gray Cipher #{0} ] Generated Key: {1}", moduleId, key);
        string encrypt = "";
        for (int aa = 0; aa < 6; aa++)
        {
            encrypt = encrypt + "" + key[(key.IndexOf(word[aa]) + (aa + 1)) % 26];
            Debug.LogFormat("[Gray Cipher #{0} ] {1} -> {2}", moduleId, word[aa], encrypt[aa]);
        }
        return encrypt;
    }
    string getKey(string k, string alpha, bool start)
    {
        for (int aa = 0; aa < k.Length; aa++)
        {
            for (int bb = aa + 1; bb < k.Length; bb++)
            {
                if (k[aa] == k[bb])
                {
                    k = k.Substring(0, bb) + "" + k.Substring(bb + 1);
                    bb--;
                }
            }
            alpha = alpha.Replace(k[aa].ToString(), "");
        }
        if (start)
            return (k + "" + alpha);
        else
            return (alpha + "" + k);
    }
	int correction(int p, int max)
    {
        while (p < 0)
            p += max;
        while (p >= max)
            p -= max;
        return p;
    }
    void left(KMSelectable arrow)
    {
        if(!moduleSolved)
        {
            Audio.PlaySoundAtTransform(sounds[0].name, transform);
            submitScreen = false;
            arrow.AddInteractionPunch();
            page--;
            page = correction(page, pages.Length);
            getScreens();
        }
    }
    void right(KMSelectable arrow)
    {
        if(!moduleSolved)
        {
            Audio.PlaySoundAtTransform(sounds[0].name, transform);
            submitScreen = false;
            arrow.AddInteractionPunch();
            page++;
            page = correction(page, pages.Length);
            getScreens();
        }
    }
    private void getScreens()
    {
        submitText.text = (page + 1) + "";
        screenTexts[0].text = pages[page][0];
        screenTexts[1].text = pages[page][1];
        screenTexts[2].text = pages[page][2];
        screenTexts[0].fontSize = 40;
        screenTexts[0].fontSize = 40;
        screenTexts[1].fontSize = 40;
        
            

    }
    void submitWord(KMSelectable submitButton)
    {
        if(!moduleSolved)
        {
            submitButton.AddInteractionPunch();
            if(screenTexts[2].text.Equals(answer))
            {
                Audio.PlaySoundAtTransform(sounds[2].name, transform);
                module.HandlePass();
                moduleSolved = true;
                screenTexts[2].text = "";
            }
            else
            {
                Audio.PlaySoundAtTransform(sounds[3].name, transform);
                module.HandleStrike();
                page = 0;
                getScreens();
                submitScreen = false;
            }
        }
    }
    void letterPress(KMSelectable pressed)
    {
        if(!moduleSolved)
        {
            pressed.AddInteractionPunch();
            Audio.PlaySoundAtTransform(sounds[1].name, transform);
            if (submitScreen)
            {
                if(screenTexts[2].text.Length < 6)
                {
                    screenTexts[2].text = screenTexts[2].text + "" + pressed.GetComponentInChildren<TextMesh>().text;
                }
            }
            else
            {
                submitText.text = "SUB";
                screenTexts[0].text = "";
                screenTexts[1].text = "";
                screenTexts[2].text = pressed.GetComponentInChildren<TextMesh>().text;
                submitScreen = true;
            }
        }
    }
#pragma warning disable 414
    private string TwitchHelpMessage = "Move to other screens using !{0} right|left|r|l|. Submit the decrypted word with !{0} submit qwertyuiopasdfghjklzxcvbnm";
#pragma warning restore 414
    IEnumerator ProcessTwitchCommand(string command)
    {

        if (command.EqualsIgnoreCase("right") || command.EqualsIgnoreCase("r"))
        {
            yield return null;
            rightArrow.OnInteract();
            yield return new WaitForSeconds(0.1f);

        }
        if (command.EqualsIgnoreCase("left") || command.EqualsIgnoreCase("l"))
        {
            yield return null;
            leftArrow.OnInteract();
            yield return new WaitForSeconds(0.1f);
        }
        string[] split = command.ToUpperInvariant().Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
        if (split.Length != 2 || !split[0].Equals("SUBMIT") || split[1].Length != 6) yield break;
        int[] buttons = split[1].Select(getPositionFromChar).ToArray();
        if (buttons.Any(x => x < 0)) yield break;

        yield return null;

        yield return new WaitForSeconds(0.1f);
        foreach (char let in split[1])
        {
            keyboard[getPositionFromChar(let)].OnInteract();
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.1f);
        submit.OnInteract();
        yield return new WaitForSeconds(0.1f);
    }

    private int getPositionFromChar(char c)
    {
        return "QWERTYUIOPASDFGHJKLZXCVBNM".IndexOf(c);
    }
}
