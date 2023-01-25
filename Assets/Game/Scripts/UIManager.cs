using System.Collections;
using System.Collections.Generic;
using UnityChan;
using UnityEngine;
using UnityEngine.UI;
using UnityLibrary;

public class UIManager : MonoBehaviour
{

    public static UIManager instance;
    public Text textCredits;

    public GameObject menuPanel;
    public GameObject creditsPanel;
    public GameObject gamePanel;
    public GameObject loadingPanel;
    public GameObject resultsPanel;
    public GameObject informationPanel;
    public Sprite[] spritesMuteButton;
    public Image imageMuted;
    public Slider loadChargeSlide;

    public bool IsLoadScreen = false;
    public bool IsInformationOpen = false;
    public bool CanFinishText = true;

    public FaceUpdate unityChanFace;

    public OpenAI openAiScript;

    public AudioSource buttonPressSound;
    public AudioSource typeWriteSound;
    

    private string gpt3Response = string.Empty;
    public bool IsMuted = true;
    public AudioSource[] allAudioSource;


    IEnumerator WriteTextAnimation;


    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
            //    DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this);
        }

    }

    private void Start()
    {
        IsMuted = true;
        CanFinishText = true;
        if (openAiScript != null)
        {
            openAiScript.onGPTExtComplete.AddListener(OnGPT3Complete);
        }
        else
        {
            Debug.LogError("OpenAI Script is not attached");
        }
    }


    public void HomeButton()
    {
        menuPanel.SetActive(true);
        creditsPanel.SetActive(false);
        gamePanel.SetActive(false); 
        loadingPanel.SetActive(false);
        resultsPanel.SetActive(false);
        unityChanFace.IsTalkChan = false;
        CanFinishText = true;
        unityChanFace.ChangeFace("angry1@unitychan");
        openAiScript.inputResults.text = "";
        StopCoroutine(unityChanFace.RepeatTalkUnityChan());
        StopCoroutine(TextWrite(""));
        StopAllCoroutines();

    }

    public void PlayButton()
    {
        menuPanel.SetActive(false);
        gamePanel.SetActive(true);
    }

    public void CreditsButton()
    {
        menuPanel.SetActive(false);
        creditsPanel.SetActive(true);
        StartCoroutine(TextWriteCredits());
    }

    public void SubmitButton()
    {
        gamePanel.SetActive(false);
        loadingPanel.SetActive(true);
        IsLoadScreen = true;
        unityChanFace.ChangeFace("SURPRISE");
    }

    public void LoadNextResult()
    {
        loadingPanel.SetActive(false);
        resultsPanel.SetActive(true);
        unityChanFace.IsTalkChan = true;
        StartCoroutine(unityChanFace.RepeatTalkUnityChan());

        gpt3Response = PlayerPrefs.GetString("Response");
        WriteTextAnimation = TextWrite(gpt3Response);
        openAiScript.inputPrompt.text = "";
        StartCoroutine(WriteTextAnimation);




    }

    public void OkButton()
    {
        resultsPanel.SetActive(false);
        gamePanel.SetActive(true);
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void informationButton()
    {
        if (!IsInformationOpen)
        {
        informationPanel.SetActive(true);
            IsInformationOpen = true;
        }
        else
        {
        informationPanel.SetActive(false);
            IsInformationOpen = false;

        }

    }

    public IEnumerator TextWrite(string generatedText)
    {

        print(generatedText);
        if (CanFinishText)
        {
        foreach (char word in generatedText)
        {
            openAiScript.inputResults.text += word.ToString();
            typeWriteSound.Play();
          
            yield return new WaitForSeconds(0.1f);
        }

        }


    }


    private void OnGPT3Complete (string response)
    {
        gpt3Response = response.ToString();
    }

    public IEnumerator TextWriteCredits()
    {
        string creditsText = textCredits.text;
        textCredits.text = "";
        foreach (char word in creditsText)
        {
            textCredits.text += word.ToString();
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void PressButtonSound()
    {
        buttonPressSound.Play();
    }

    public void ChangeSpriteMuteButton()
    {
        if (IsMuted)
        {
            imageMuted.sprite =  spritesMuteButton[0];
            IsMuted = false;
            foreach (AudioSource audioSource in allAudioSource)
            {
                audioSource.mute = true;
            }
        }
        else
        {
            imageMuted.sprite =  spritesMuteButton[1];
             IsMuted = true;
            foreach (AudioSource audioSource in allAudioSource)
            {
                audioSource.mute = false;
            }

        }
    }

    public void GetAllResponseButton()
    {
        CanFinishText = false;
        StopAllCoroutines();
        openAiScript.inputResults.text = gpt3Response;

    }

    public void BackToAskQuestionButton()
    {
        unityChanFace.IsTalkChan = false;
        unityChanFace.ChangeFace("angry1@unitychan");
        openAiScript.inputResults.text = "";
        StopCoroutine(unityChanFace.RepeatTalkUnityChan());
        StopCoroutine(TextWrite(""));
        StopAllCoroutines();
        resultsPanel.SetActive(false);
        gamePanel.SetActive(true);
        CanFinishText = true;
        


    }


}
