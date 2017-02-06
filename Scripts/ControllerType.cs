using UnityEngine;
using System.Collections;

public class ControllerType : MonoBehaviour {
    public bool isConsole = false;
    public static bool isOnConsole;

    void Awake() {
        isOnConsole = isConsole;
    }
}
