using UnityEngine;
using System.Collections;

public class XperiaInput : MonoBehaviour {
    #if UNITY_ANDROID
    public static  AndroidJavaObject currentConfig = null;

    static public AndroidJavaObject CurrentConfig {
        get {
            if (currentConfig == null) {
                using (AndroidJavaClass player = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
                    AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity");
                    currentConfig = activity.Call<AndroidJavaObject>("getResources").Call<AndroidJavaObject>("getConfiguration");
                }
            }
            return currentConfig;
        }
    }

    public static bool isKeypadAvailable {
        get {
            const int NAVIGATIONHIDDEN_UNDEFINED = 0;
            const int NAVIGATIONHIDDEN_NO = 1;
            //const int NAVIGATIONHIDDEN_YES = 2;

            int nav = CurrentConfig.Get<int>("navigationHidden");
            return nav == NAVIGATIONHIDDEN_NO || nav == NAVIGATIONHIDDEN_UNDEFINED;
        }
    }

    public static int KeypadHiddenStatus {
        get {
            return CurrentConfig.Get<int>("navigationHidden");
        }
    }

    public static Vector2 XperiaLeftStick {
        get {
            return GetStick(0.0f, 0.0f);
        }
    }

    public static Vector2 XperiaRightStick {
        get {
            return GetStick(AndroidInput.secondaryTouchWidth - AndroidInput.secondaryTouchHeight, 0.0f);
        }
    }

    public static Vector2 GetStick(float _x, float _y) {
        if (AndroidInput.secondaryTouchEnabled == true && isKeypadAvailable == true) {
            for (int i = 0; i < AndroidInput.touchCountSecondary; i++) {
                Vector2 touchPosition = AndroidInput.GetSecondaryTouch(i).position;
                if (touchPosition.x < _x || touchPosition.x > _x + AndroidInput.secondaryTouchHeight) {
                    continue;
                }

                return new Vector2(
                    ((touchPosition.x - _x) / AndroidInput.secondaryTouchHeight) * 2.0f - 1.0f,
                    (((touchPosition.y - _y) / AndroidInput.secondaryTouchHeight * 2.0f - 1.0f) * -1.0f)
                );
            }
        }
        return Vector2.zero;
    }
    #endif
}
