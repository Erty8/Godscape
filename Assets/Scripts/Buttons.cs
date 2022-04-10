using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using Unity.Netcode;
using TMPro;
using Godspace.Core.Singletons;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;


public class Buttons : MonoBehaviour
{
    public RelayManager relayManager;
    [SerializeField] Canvas pauseCanvas;
    [SerializeField] Canvas feedbackCanvas;
    [SerializeField] AudioMixer mixer;
    [SerializeField] Canvas settingsCanvas;
    [SerializeField] Canvas serverCanvas;
    [SerializeField] GameObject choosePanel;
    [SerializeField] Canvas hostCanvas;
    [SerializeField] GameObject joinPanel;
    [SerializeField] Text playersInGameText;
    [SerializeField] TextMeshProUGUI god1Text;
    [SerializeField] TextMeshProUGUI god2Text;
    [SerializeField] TextMeshProUGUI god3Text;
    [SerializeField] TextMeshProUGUI god4Text;
    [SerializeField] TextMeshProUGUI player1Name;
    [SerializeField] TMP_InputField insertName;  
    public  TextMeshProUGUI joinCode;
    bool paused = false;
    List<GameObject> soundObjects = new List<GameObject>();
    List<string> godNames = new List<string> { "Munia", "Naba", "Vamor", "Carea" };
    Scene currentScene;
    // Start is called before the first frame update
    void Start()
    {
       
        god1Text = GameObject.Find("God1Name").GetComponent<TextMeshProUGUI>();
        god2Text = GameObject.Find("God2Name").GetComponent<TextMeshProUGUI>();
        god3Text = GameObject.Find("God3Name").GetComponent<TextMeshProUGUI>();
        god4Text = GameObject.Find("God4Name").GetComponent<TextMeshProUGUI>();
        player1Name = GameObject.Find("Player1name").GetComponent<TextMeshProUGUI>();
        relayManager = GameObject.Find("NetworkManager").GetComponent<RelayManager>();
        joinCode = GameObject.Find("JoinCode").GetComponent<TextMeshProUGUI>();
        Button startHostButton = GameObject.Find("HostButton").GetComponent<Button>();
        Button joinButton = GameObject.Find("JoinButton").GetComponent<Button>();
        TMP_InputField joinCodeInput = GameObject.Find("JoinCode").GetComponent<TMP_InputField>();
        insertName = GameObject.Find("InsertName").GetComponent<TMP_InputField>();
        serverCanvas = GameObject.Find("ServerCanvas").GetComponent<Canvas>();
        //pauseCanvas = GameObject.Find("Pause").gameObject.GetComponent<Canvas>();
        settingsCanvas = GameObject.Find("SettingsCanvas").GetComponent<Canvas>();
        choosePanel = GameObject.Find("ChoosePanel");
        hostCanvas = GameObject.Find("HostCanvas").GetComponent<Canvas>();
        joinPanel = GameObject.Find("JoinPanel");
        //playersInGameText = GameObject.Find("PlayersText").GetComponent<Text>();
        currentScene = SceneManager.GetActiveScene();
        var sources = FindObjectsOfType<AudioSource>();
        for (int i = 0; i < godNames.Count; i++)
        {
            string temp = godNames[i];
            int randomIndex = Random.Range(i, godNames.Count);
            godNames[i] = godNames[randomIndex];
            godNames[randomIndex] = temp;
        }
        god1Text.text = godNames[0];
        god2Text.text = godNames[1];
        god3Text.text = godNames[2];
        god4Text.text = godNames[3];
        foreach (AudioSource source in sources)
        {
            //source.outputAudioMixerGroup = mixer;
        }
        startHostButton?.onClick.AddListener(async () =>
        {
            // this allows the UnityMultiplayer and UnityMultiplayerRelay scene to work with and without
            // relay features - if the Unity transport is found and is relay protocol then we redirect all the 
            // traffic through the relay, else it just uses a LAN type (UNET) communication.
            if (RelayManager.Instance.IsRelayEnabled)
                await RelayManager.Instance.SetupRelay();

            if (NetworkManager.Singleton.StartHost())
                Debug.Log("started host");
                
            
                
            //Logger.Instance.LogInfo("Host started...");
            else
                Debug.Log("unable to start host ");
            //Logger.Instance.LogInfo("Unable to start host...");
        });
        joinButton?.onClick.AddListener(async () =>
        {
            if (RelayManager.Instance.IsRelayEnabled && !string.IsNullOrEmpty(joinCodeInput.text))
                await RelayManager.Instance.JoinRelay(joinCodeInput.text);

            if (NetworkManager.Singleton.StartClient())
                Debug.Log("joined");
            
        });
    }
    private void Awake()
    {
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(relayManager.relayHostData.JoinCode);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                pauseCanvas.enabled = false;
                feedbackCanvas.enabled = false;
                //Time.timeScale = 1;
                paused = false;
            }
            else
            {
                pauseCanvas.enabled = true;
                feedbackCanvas.enabled = true;
                //Time.timeScale = 0;
                paused = true;
            }

            //StartCoroutine(loadScene());
        }
    }
    public void resume()
    {
        pauseCanvas.enabled = false;
        feedbackCanvas.enabled = false;
        //Time.timeScale = 1;
        paused = false;
    }
    public void play()
    {
        serverCanvas.GetComponent<Canvas>().enabled = true;
        choosePanel.SetActive(true);
        
    }
    public void openHostPanel()
    {
        hostCanvas.enabled = true;
        joinCode.text = relayManager.relayHostData.JoinCode;
        player1Name.text = insertName.text ;
    }
    public void hostGame()
    {
        NetworkManager.Singleton.StartHost();
        //hostPanel.SetActive(true);
    }
    
    public void hostRelay()
    {
        //RelayManager.Instance.SetupRelay();
        
    }
    public void joinGame()
    {
        NetworkManager.Singleton.StartClient();
        //joinPanel.SetActive(true);

    }
    public void closeCanvas()
    {
        transform.root.GetComponent<Canvas>().enabled = false;
        
    }
    public void back()
    {
        transform.parent.gameObject.SetActive(false);

    }

    public void startGame()
    {
        Debug.Log("loading scene");
        StartCoroutine(loadScene());
    }
    public void reloadCurrentScene()
    {
        StartCoroutine(menu());
        //StartCoroutine(reloadScene());
    }

    public void exitGame()
    {
        Application.Quit();
    }

    public void hideButton()
    {
        gameObject.SetActive(false);
    }
    public void adjustSound(float sliderValue)
    {
        mixer.SetFloat("Master", Mathf.Log10(sliderValue) * 20);
    }
    public void openSettings()
    {
        settingsCanvas.enabled = true;
    }
    public void closeSettings()
    {
        settingsCanvas.enabled = false;
    }
    IEnumerator loadScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    IEnumerator reloadScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(currentScene.name);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    IEnumerator menu()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(0);
        Time.timeScale = 1;
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
