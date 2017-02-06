using UnityEngine;
using System.Collections;

public class DataBase : MonoBehaviour {
    static public bool appIsFirstOpen = true;
    static public int levelID = 0;

    static public int currentWeaponID = 0;
    static public int pistolRound = 8;
    static public int sniperRifleRound = 5;
    static public int shotgunRound = 10;
    static public int totalTarget = 0;
    static public int killedTarget = 0;
    static public bool weaponSet = false;
    static public bool isMute = false;

    static public bool hangZombieisDead = false;

    static public string d_Model {
        get {
            return (SystemInfo.deviceModel);
        }
    }


    static public bool isConsole {
        get {
            if (Input.GetJoystickNames().Length > 0){
                ControllerType.isOnConsole = true;
            }
            else {
                ControllerType.isOnConsole = false;
            }
            return (ControllerType.isOnConsole);
        }
    }

    static public string joystickName {
        get {
            if (ControllerType.isOnConsole == true) {
                return (Input.GetJoystickNames()[0]);
            }
            else {
                return ("No GamePad Detected");
            }
            
        }
    }

    static public int bullets {
        get {
            return(PlayerController.bulletNumber);
        }
    }
}
