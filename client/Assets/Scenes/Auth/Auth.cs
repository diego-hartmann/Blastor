using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class Auth : MonoBehaviour
{
    [SerializeField] private string apiPAth = "";
    private string Url(string _endpoint) => $"{this.apiPAth}/{_endpoint}";

    [SerializeField] private TextMeshProUGUI alertText = null;
    [SerializeField] private Button loginButton = null;
    [SerializeField] private Button createButton = null;
    [SerializeField] private TMP_InputField usernameInputField = null;
    [SerializeField] private TMP_InputField passwordInputField = null;

    [SerializeField] private SceneLoader SceneLoaderScript = null;


    private void EnterHub(){
        SceneLoaderScript.LoadHub();
    }

    public void OnLoginButton()
    {
        SetAlert("Signing in...", false);
        StartCoroutine(TryToLogin());
    }

    public void OnCreateButton()
    {
        SetAlert("Creating account...", false);
        StartCoroutine(TryToCreate());
    }

    private void SetAlert(string mess, bool interactable = true)
    {
        alertText.text = mess;
        ActivateButtons(interactable);
    }
    

    private WWWForm CreateAuthForm(string username, string password){
        WWWForm form = new WWWForm();
        form.AddField("rUsername", username);
        form.AddField("rPassword", password);
        return form;
    }

    private bool AreCredentialsValid(string username, string password){
        CredentialsValidator validator = new CredentialsValidator(username, password, SetAlert);
        return validator.Validate();
    }

    private IEnumerator TryToLogin()
    {
        string username = usernameInputField.text, password = passwordInputField.text;

        if (!AreCredentialsValid(username, password)) yield break;

        UnityWebRequest request = UnityWebRequest.Post(Url("login"), CreateAuthForm(username, password));
        
        var Handler = request.SendWebRequest();

        float startTime = 0;
        while (!Handler.isDone)
        {
            startTime += Time.deltaTime;
            if (startTime > 10.0f) break; 

            yield return null;
        }
        
        if(request.result == UnityWebRequest.Result.Success){
            LogininResponse response = JsonUtility.FromJson<LogininResponse>(request.downloadHandler.text);
            switch(response.code){
                case 0: 
                    var data = response.data;
                    PlayerData.Load(data._id, data.username, data.record, data.kills, data.medals, data.items, data.isAdmin); ;
                    SetAlert($"Welcome {data.username}!", false);
                    EnterHub();
                    break;
                case 1: SetAlert("Invalid credentials."); break;
                default: SetAlert("Corruption detected.", false); break;
            }
            yield break;
        }

        SetAlert("Unable to connect to server.");
        yield return null;
    }

    
    private IEnumerator TryToCreate()
    {
        string username = usernameInputField.text, password = passwordInputField.text;

        if (!AreCredentialsValid(username, password)) yield break;

        UnityWebRequest request = UnityWebRequest.Post(Url("create"), CreateAuthForm(username, password));

        var Handler = request.SendWebRequest();

        float startTime = 0;
        while (!Handler.isDone){
            startTime += Time.deltaTime;
            if (startTime > 10.0f) break;

            yield return null;
        }

        if (request.result == UnityWebRequest.Result.Success){
            CreateResponse response = JsonUtility.FromJson<CreateResponse>(request.downloadHandler.text);
            switch(response.code){
                case 0: 
                    var data = response.data;
                    PlayerData.Load(data._id, data.username, data.record, data.kills, data.medals, data.items, data.isAdmin); ;
                    SetAlert($"Welcome {data.username}!", false);
                    EnterHub();
                    break;
                case 1: 
                    SetAlert("Invalid credentials.");
                    break;
                case 2: 
                    SetAlert("Username is already taken.");
                    break;
                case 3: 
                    SetAlert("Unsafe password.");
                break;
                default:
                    SetAlert("Corruption detected.");
                    break;
            }
            yield break;
        }

        SetAlert("Unable to connect to server.");
        yield return null;
    }


    private void ActivateButtons(bool active){
        createButton.interactable = active;
        loginButton.interactable = active;
    }
}
