// sample code by unitycoder.com

using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

[System.Serializable]
public class OnGPTExtComplete : UnityEvent<string>
{
}

namespace UnityLibrary
{
    public class OpenAI : MonoBehaviour
    {
        const string url = "https://api.openai.com/v1/completions";

        public string modelName = "text-davinci-003";

        public InputField inputPrompt;
        public InputField inputResults;
        public GameObject loadingIcon;
        string generatedText;
        string apiKey = null;
        bool isRunning = false;

        public OnGPTExtComplete onGPTExtComplete;


        private void Awake()
        {
            if(onGPTExtComplete == null)
            {
                onGPTExtComplete = new OnGPTExtComplete();
            }
        }


        void Start()
        {
            LoadAPIKey();
        }

        public void Execute()
        {
            if (isRunning)
            {
                Debug.LogError("Already running");
                return;
            }
            isRunning = true;
            loadingIcon.SetActive(true);
            
            // fill in request data
            RequestData requestData = new RequestData()
            {
                model = modelName,
                prompt = inputPrompt.text + ", con un max_tokens 3000, respondeme como si fueras una bola 8 magica, sin ser una bola 8 magica",
                temperature = 0.7f,
                max_tokens = 250,
                top_p = 1,
                frequency_penalty = 0,
                presence_penalty = 0
            };

            string jsonData = JsonUtility.ToJson(requestData);

            byte[] postData = System.Text.Encoding.UTF8.GetBytes(jsonData);

            UnityWebRequest request = UnityWebRequest.Post(url, jsonData);
            request.uploadHandler = new UploadHandlerRaw(postData);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + apiKey);

            UnityWebRequestAsyncOperation async = request.SendWebRequest();

            async.completed += (op) =>
            {
                if (request.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.LogError(request.error);
                }
                else
                {
                    Debug.Log(request.downloadHandler.text);
                    

                    // parse the results to get values 
                    OpenAIAPI responseData = JsonUtility.FromJson<OpenAIAPI>(request.downloadHandler.text);
                    // sometimes contains 2 empty lines at start?
                     generatedText = new string(responseData.choices[0].text.TrimStart('\n').TrimStart('\n').ToCharArray());

                    PlayerPrefs.SetString("Response", generatedText);
                    print("Respuesta gpt3: " + generatedText);

                    onGPTExtComplete.Invoke(generatedText.ToString());

                    //Read the text and put in the input
                   // inputResults.text = generatedText;

                }
                loadingIcon.SetActive(false);
                isRunning = false;
            };

        } // execute

        void LoadAPIKey()
        {
            // TODO optionally use from env.variable

            // MODIFY path to API key if needed
            var keypath = Path.Combine(Application.streamingAssetsPath, "secretkey.txt");
            if (File.Exists(keypath) == false)
            {
                Debug.LogError("Apikey missing: " + keypath);
            }

            //Debug.Log("Load apikey: " + keypath);
            apiKey = File.ReadAllText(keypath).Trim();
            Debug.Log("API key loaded, len= " + apiKey.Length);
        }


     


        public string GetAnswerText ()
        {
            return string.IsNullOrEmpty(generatedText) ? "The Variable is null yet" : generatedText;
        }


    }
}
