using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LoginMenu : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text Message;
    public TMP_InputField Email;
    public TMP_InputField Password;


    //For Login
    public void Login()
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = Email.text,
            Password = Password.text,
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
    }

    //Login Success
    void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("success Login");
        Message.text = "Successfully Logged in";

        SceneManager.LoadScene(1);
    }

    //For Registration
    public void Register()
    {
        if(Password.text.Length< 6)
        {
            Message.text = "Password is too short..Should be more than 6 Letters";
            return;
        }
        var request = new RegisterPlayFabUserRequest
        {
            Email = Email.text,
            Password = Password.text,
            RequireBothUsernameAndEmail = false,
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
    }

    //Register Success
    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("Registration Success");
        Message.text = "You are registered successfully and logging in";
        SceneManager.LoadScene(1);
    }

    //Reset Password
    public void ResetPassword()
    {
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = Email.text,
            TitleId = "91BDE",
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnPasswordReset, OnError);
    }

    //On Password Reset Success
    void OnPasswordReset(SendAccountRecoveryEmailResult result)
    {
        Debug.Log("Mail sent");
        Message.text = "Password sent";
    }

    public void OnGuestLogin()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true,
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnGuestLoginSuccess, OnError);
    }

    void OnGuestLoginSuccess(LoginResult result)
    {
        Message.text = "Guest Login Sucessful";
        Debug.Log("Guest Login Success");
        SceneManager.LoadScene(1);
    }

    void OnError(PlayFabError error)
    {
        Debug.LogError("Error: " + error.GenerateErrorReport());
        Message.text = "Error: " + error.ErrorMessage;
    }
}
