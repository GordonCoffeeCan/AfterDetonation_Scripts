using UnityEngine;
using System.Collections;

public class DebugInfoDisplay : MonoBehaviour {
    private float deltaTime = 0.0f;

    void Update() {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
    }

    void OnGUI() {
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        GUILayout.TextField("Device Model: " + DataBase.d_Model);
        GUILayout.TextField("Connected Joystick: " + DataBase.joystickName);
        GUILayout.Label("Bullet Rounds: " + DataBase.bullets);
        GUILayout.TextField(text);
        //GUILayout.TextField("Is Xperia Joypad Availabe: " + isXperiaPadAvailabe);
    }
}
