using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Text;
using System.Collections;
using System.Globalization;
using System.Collections.Generic;

public class Register : MonoBehaviour
{
    // Cached references
    public InputField nameInputField;
    public InputField birthdateInputField;
    public InputField emailInputField;
    public InputField passwordInputField;
    public InputField confirmPasswordInputField;
    public Button registerButton;
    public Text messageBoardText;
    public Player player;
    public PlayerSerializable playerSerializable;
    public void OnRegisterButtonClick()
    {
        StartCoroutine(RegisterNewUser());
        //InsertPlayer();
        //StartCoroutine
    }

    private IEnumerator RegisterNewUser()
    {
        yield return RegisterUser();
        yield return Helper.InitializeToken(emailInputField.text, passwordInputField.text);  //Sets player.Token
        messageBoardText.text += "\nToken: " + player.Token.Substring(0,7) + "...";
        yield return Helper.GetPlayerId();  //Sets player.Id
        messageBoardText.text += "\nId: " + player.Id ;
        player.Email = emailInputField.text;
        player.Name = nameInputField.text;
        player.BirthDay = DateTime.Parse(birthdateInputField.text);
        messageBoardText.text += "\n" + player.Name + " " + player.Email + " " + player.BirthDay;// + " \n"+ player.Token;
        yield return InsertPlayer();
        messageBoardText.text += $"\nPlayer \"{player.Name}\" registered.";
        player.Id = string.Empty;
        player.Token = string.Empty;
        player.Name = string.Empty;
        player.Email = string.Empty;
        player.BirthDay = DateTime.MinValue;
    }

    private IEnumerator InsertPlayer()
    {/*
        playerSerializable.Id = player.Id;
        playerSerializable.Name = player.Name;
        playerSerializable.Email = player.Email;
        playerSerializable.BirthDay = player.BirthDay.ToString();*/
        /*
        playerSerializable.Id = player.Id;
        playerSerializable.Name = nameInputField.text;
        playerSerializable.Email = emailInputField.text;
        playerSerializable.BirthDay = player.BirthDay.ToString();
        */

        UnityWebRequest httpClient = new UnityWebRequest(player.HttpServerAddress + "api/Player/RegisterPlayer");

        String url = player.HttpServerAddress + "api/Player/RegisterPlayer";

        WWWForm usuario = new WWWForm();
        usuario.AddField("Id", player.Id);
        usuario.AddField("Name", player.Name);
        usuario.AddField("Email", player.Email);
        usuario.AddField("BirthDay", player.BirthDay.ToString());

        //Pasamos la info a array de Bytes.
        byte[] cuerpo = usuario.data;

        //Modifico la cabeçera 
        Dictionary<string, string> headers = usuario.headers;
        //Se incluye el token
        //headers["Authorization"] = player.Token;
        headers["Authorization"] = PlayerPrefs.GetString(player.Token);

        WWW newPlayer = new WWW(url, cuerpo, headers);

        yield return newPlayer;

        if (newPlayer.error != null)
        {
            messageBoardText.text += "\nError registrando el Player " + newPlayer.error;
            throw new Exception("RegisterNewPlayer > InsertPlayer: " + newPlayer.error + "\n" + player.Token + "\n"+ player.Id);
        }

        messageBoardText.text += "\nRegisterNewPlayer > InsertPlayer: " + newPlayer.text;



        /*
        using (UnityWebRequest httpClient = new UnityWebRequest(player.HttpServerAddress + "api/Player/RegisterPlayer", "POST"))
        {
            string playerData = JsonUtility.ToJson(playerSerializable);
            byte[] bodyRaw = Encoding.UTF8.GetBytes(playerData);
            httpClient.uploadHandler = new UploadHandlerRaw(bodyRaw);
            //Linea faltante
            httpClient.downloadHandler = new DownloadHandlerBuffer();
            httpClient.SetRequestHeader("Content-type", "application/json");
            httpClient.SetRequestHeader("Authorization", "bearer " + player.Token);

            yield return httpClient.SendWebRequest();

            if (httpClient.isNetworkError || httpClient.isHttpError)
            {
                messageBoardText.text += "\nError registrando el Player " + httpClient.responseCode;
                throw new Exception("RegisterNewPlayer > InsertPlayer: " + httpClient.error);
            }
			
			messageBoardText.text += "\nRegisterNewPlayer > InsertPlayer: " + httpClient.responseCode;
        }*/

    }

    private IEnumerator RegisterUser()
    {
        UnityWebRequest httpClient = new UnityWebRequest(player.HttpServerAddress + "api/Account/Register");
        /*
        AspNetUserRegister newUser = new AspNetUserRegister();
        newUser.Email = emailInputField.text;
        newUser.Password = passwordInputField.text;
        newUser.ConfirmPassword = confirmPasswordInputField.text;

        string jsonData = JsonUtility.ToJson(newUser);
        byte[] dataToSend = Encoding.UTF8.GetBytes(jsonData);
        httpClient.uploadHandler = new UploadHandlerRaw(dataToSend);

        httpClient.SetRequestHeader("Content-Type", "application/json");
        */
        // Creamos un form para guardar la informacion
        String url = player.HttpServerAddress + "/api/Account/Register";

        WWWForm usuario = new WWWForm();
        usuario.AddField("Email", emailInputField.text);
        usuario.AddField("Password", passwordInputField.text);
        usuario.AddField("ConfirmPassword", confirmPasswordInputField.text);
        var download = UnityWebRequest.Post(url, usuario);

        yield return download.SendWebRequest();


        // yield return httpClient.SendWebRequest();

        if (download.isNetworkError || download.isHttpError)
        {
            throw new Exception("OnRegisterButtonClick: Error > " + download.error);
        }

        messageBoardText.text += "\nOnRegisterButtonClick: " + download.responseCode;

        download.Dispose();
    }

}
