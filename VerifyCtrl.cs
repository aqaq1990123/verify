using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*包体签名验证器 (PS:建议后续这个类名改成其他有意义的单词或者乱七八糟的词汇) */
public class VerifyCtrl
{
    
    private static VerifyCtrl _instance = null;
    
    public static VerifyCtrl Instance
    {
        get
        {
            if (null == _instance)
                _instance = new VerifyCtrl();
            return _instance;
        }
    }
    
    private static int MY_SIGN_HASH = 83475354;//你自己的签名信息
    public string packageName;
    public int curSignHash = 0;

    /// <summary>
    /// 是否包体签名验证通过(PS:建议后续这个类名改成其他有意义的单词或者乱七八糟的词汇) 
    /// </summary>
    /// <returns></returns>
    public bool IsCorrect()
    {
#if UNITY_ANDROID
		// 获取Android的PackageManager    
		AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject Activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
		AndroidJavaObject PackageManager = Activity.Call<AndroidJavaObject>("getPackageManager");
		// 获取当前Android应用的包名
		packageName = Activity.Call<string>("getPackageName");
		// 调用getPackageInfo方法来获取签名信息数组    
		int GET_SIGNATURES = PackageManager.GetStatic<int>("GET_SIGNATURES");
		AndroidJavaObject PackageInfo = PackageManager.Call<AndroidJavaObject>("getPackageInfo", packageName, GET_SIGNATURES);
		AndroidJavaObject[] Signatures = PackageInfo.Get<AndroidJavaObject[]>("signatures");
		
		// 获取当前的签名的哈希值，判断其与我们签名的哈希值是否一致
		if (Signatures != null && Signatures.Length > 0)
		{
			curSignHash = Signatures[0].Call<int>("hashCode");
			return curSignHash == MY_SIGN_HASH;
		}
        else 
            return false;
#else
        return true;	
#endif
    }
}
