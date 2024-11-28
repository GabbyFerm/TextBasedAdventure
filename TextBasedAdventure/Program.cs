using System;
using System.Drawing;
using System.IO;
using System.Xml.Linq;
using Newtonsoft.Json;
using TextBasedAdventure.Classes;
using TextBasedAdventure.Validators;

public class Program : GameManager
{
    public static void Main(string[] args)
    {
       GameManager.StartGame();
    }
}