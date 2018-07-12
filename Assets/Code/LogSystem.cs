using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class LogSystem {

    static string fileName = "LOG\\" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".txt";

    public static void LogOnConsole(string message) {
        if(GameManager.GetDebugMode())
            Debug.Log(message);
    }

    public static void LogOnFile(string message) {
        File.AppendAllText(fileName, System.DateTime.Now.ToString("HH:mm:ss") + " |LT: " + GameManager.GetTime().ToString("0000.00") + " |=|Message: " + message + System.Environment.NewLine);
    }
}
