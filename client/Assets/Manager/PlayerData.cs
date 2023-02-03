using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerData 
{
    public static string _id = "";
    public static string username = "";
    public static int kills = 0;
    public static int[] medals = new int[0];
    public static int[] items = new int[0];
    public static int record = 0;
    public static bool isAdmin = false;

    public static void Load(string __id, string _username, int _record, int _kills, int[] _medals, int[] _items, bool _isAdmin)
    {
        _id = __id;
        username = _username;
        record = _record;
        kills = _kills;
        medals = _medals;
        items = _items;
        isAdmin = _isAdmin;
    }
    
    public static async void Save()
    {
        try
        {
            // loading true
            // await api.saveData(kills, medals, items, playerId);
        }
        catch
        {
            // modal (err.message);
        }
    }
  
}
