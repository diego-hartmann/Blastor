using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CredentialsValidator {

    private string username, password = "";
    private SetAlertCallback setAlertCallback = null;
    public CredentialsValidator(string _username, string _password, SetAlertCallback _setAlertCallback) { 
        username = _username;
        password = _password;
        setAlertCallback = _setAlertCallback;
    } 
        
    
    public bool Validate()
    {
        if (username == "" && password == "")
        {
            setAlertCallback("No credentials provided.");
            return false;
        };
        if (username == "")
        {
            setAlertCallback("Provide a username.");
            return false;
        }
        if (username.Length < 3)
        {
            setAlertCallback("Provide a longer username.");
            return false;
        }
        if (username.Length > 24)
        {
            setAlertCallback("Provide a shorter username.");
            return false;
        }
        if (password == "")
        {
            setAlertCallback("Provide a password.");
            return false;
        }
        if (password.Length < 3)
        {
            setAlertCallback("Provide a longer password.");
            return false;
        }
        if (username.Length > 24)
        {
            setAlertCallback("Provide a shorter password.");
            return false;
        }
        return true;
    }
}
