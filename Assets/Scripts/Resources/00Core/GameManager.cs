using UnityEngine;
using static GameSystem;

public class GameManager : GameSystem
{

    public string[] inputBuffer = { "Z", "Z", "Z", "Z", "Z", "Z", "Z", "Z", "Z", "Z", "Z", "Z", "Z", "Z", "Z" };
    public int validationBuffer = 0;


    private void Awake()
    {
        InitializeGame();
    }


    void Update()
    {
        HighlightPickeableObject();
        HighlightUsableObject();
    }


    void InitializeGame()
    {
        #region INITIALIZE GAMESYSTEM
        for (int i = 0; i < spellBells.Length - 2; i++)
        {
            int z = i + 1;
            spellBells[i] = Resources.Load("00/Music/SpellBell" + z.ToString()) as AudioClip;
        }
        spellBells[15] = Resources.Load("00/Music/SpellBellSuccess") as AudioClip;
        spellBells[16] = Resources.Load("00/Music/SpellBellError") as AudioClip;
        cameraOrbit = GameObject.Find("Camera Orbit");
        highlightedMaterial = Resources.Load("Materials/HighlightGameObject") as Material;
        highlightedWrongMaterial = Resources.Load("Materials/HighlightWrongObject") as Material;
        soundEffectUI = cameraOrbit.GetComponent<AudioSource>();
        player = GameObject.Find("Player");
        gameID = "Game_1";
        gameManager = this;
        #endregion
        if (!LoadGame())
        {
            #region CREATE A NEW GAME
            islandDay = 1;
            Instantiate(Resources.Load("10/01/01/03") as GameObject);
            Instantiate(Resources.Load("10/01/01/01") as GameObject);
            Instantiate(Resources.Load("14/01/01/01") as GameObject);
            Instantiate(Resources.Load("14/01/02/04") as GameObject);
            #endregion
        }
        SendMessageToDiscord("c/dar 1 rhymlow");
    }


    #region SpellBuffer

    public void ExecuteBuffer()
    {
        string outputBuffer = "";
        for (int i = 0; i < inputBuffer.Length; i++)
        {
            if (inputBuffer[i] != "Z")
            {
                outputBuffer += inputBuffer[i];
            }
            else
            {
                i = inputBuffer.Length;
            }
        }
        if(Resources.Load("12/" + outputBuffer + "/" + outputBuffer))
        {
            //Debug.Log(outputBuffer);
            Instantiate(Resources.Load("12/" + outputBuffer + "/" + outputBuffer));
            soundEffectUI.clip = spellBells[15];
            soundEffectUI.time = 0;
            soundEffectUI.Play();
            validationBuffer = 0;
        }
        else if (inputBuffer[0] != "Z")
        {
            //Debug.Log(outputBuffer);
            soundEffectUI.clip = spellBells[16];
            soundEffectUI.time = 0;
            soundEffectUI.Play();
            validationBuffer = 0;
        }
    }

    public void AddToBuffer(string inputToAdd, GameObject inputButton)
    {
        for(int i = 0; i < inputBuffer.Length; i++)
        {
            if (inputBuffer[i] == "Z")
            {
                if (i == 0)
                {
                    inputBuffer[i] = inputToAdd;
                    AudioSource z = inputButton.GetComponent<AudioSource>();
                    z.clip = spellBells[i];
                    z.time = 0;
                    z.Play();
                }
                else if (i > 0)
                {
                    if (inputBuffer[i - 1] != inputToAdd)
                    {
                        inputBuffer[i] = inputToAdd;
                        AudioSource z = inputButton.GetComponent<AudioSource>();
                        z.clip = spellBells[i];
                        z.time = 0;
                        z.Play();
                    }
                    i = inputBuffer.Length;
                }
            }
        }
    }

    public void CleanBuffer()
    {
        for (int i = 0; i < inputBuffer.Length; i++)
        {
            inputBuffer[i] = "Z";
        }
    }

    #endregion
}
