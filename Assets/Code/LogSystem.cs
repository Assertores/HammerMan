using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LogSystem {
    public static void LogOnConsole(string message) {
        if(GameManager.GetDebugMode())
            Debug.Log(message);
    }
}
