﻿using System;
using UnityEngine;

public class Player : MonoBehaviour
{
   // private const string _httpServerAddress = "https://localhost:44348/";
    private const string _httpServerAddress = "http://localhost:59995/";
    private static  string jugador = "Anonimo";

    public string Jugador
    {
        get { return jugador; }
        set { jugador = value; }

    }

    public string HttpServerAddress
    {
        get
        {
            return _httpServerAddress;
        } 
    }

    private string _token;
    public string Token
    {
        get { return _token; }
        set { _token = value; }
    }

    private string _id;
    public string Id
    {
        get { return _id; }
        set { _id = value; }
    }

    private string _name;
    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    private DateTime _birthday;
    public DateTime BirthDay
    {
        get { return _birthday; }
        set { _birthday = value; }
    }

    private string _email;
    public string Email
    {
        get { return _email; }
        set { _email = value; }
    }

}
