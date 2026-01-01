using UnityEngine;
using static GameSystem;

public class GameManager : GameSystem
{

    public string[] inputBuffer = { "Z", "Z", "Z", "Z", "Z", "Z", "Z", "Z", "Z", "Z", "Z", "Z", "Z", "Z", "Z" };
    public int validationBuffer = 0;
    float timer = 0;
    bool playerActivated = false;


    private void Awake()
    {
        InitializeGame();
    }

    private void Start()
    {
        FillFilteredRoots();
    }


    void Update()
    {
        ActivatePlayer();
        HighlightPickeableObject();
    }


    void InitializeGame()
    {
        #region INITIALIZE AUDIOCLIPS
        for (int i = 0; i < spellBells.Length - 2; i++)
        {
            int z = i + 1;
            spellBells[i] = Resources.Load("00/Music/SpellBell" + z.ToString()) as AudioClip;
        }
        spellBells[15] = Resources.Load("00/Music/SpellBellSuccess") as AudioClip;
        spellBells[16] = Resources.Load("00/Music/SpellBellError") as AudioClip;
        #endregion
        gameID = "Game_1";
        cameraOrbit = GameObject.Find("Camera Orbit");
        highlightedMaterial = Resources.Load("Materials/HighlightGameObject") as Material;
        highlightedWrongMaterial = Resources.Load("Materials/HighlightWrongObject") as Material;
        soundEffectUI = cameraOrbit.GetComponent<AudioSource>();
        player = GameObject.Find("Player");
        gameManager = this;
        SendMessageToDiscord("c/dar 1 rhymlow");
        Instantiate(Resources.Load("10/01/01/02") as GameObject);
        Instantiate(Resources.Load("10/01/01/03") as GameObject);
    }


    void ActivatePlayer()
    {
        timer += Time.deltaTime;
        if(timer > 3.0f && playerActivated == false)
        {
            player.SetActive(true);
            playerActivated = true;
            Debug.Log("El jugador fue activado");
        }
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
