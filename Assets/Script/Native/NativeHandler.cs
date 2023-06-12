using System.Runtime.InteropServices;
using UnityEngine;

public class NativeHandler : MonoBehaviour {
	[DllImport("__Internal")]
	private static extern void unityCall(string str); // 此接口在mm文件中定义

	[DllImport("__Internal")]
	private static extern string unityInvoke(string str); // 此接口在mm文件中定义

	[DllImport("__Internal")]
	private static extern void netStatus(); // 此接口在mm文件中定义

	public void Start() {
#if UNITY_IPHONE
		// var a = unityCall("aaaaaaaaaaaa");
		// Debug.Log("Unity log " + a);
		string sendJson = "{\"method\":42,\"value\":\"Hello World!\"}";
		unityCall(sendJson);
		var jsonStr = unityInvoke(sendJson);
		var jsonObj = JsonUtility.FromJson<Object>(jsonStr);
		Debug.Log(jsonObj);
#endif
	}

	public void onNativeMessage(string str) {
		Debug.Log("on receive from oc " + str);
	}
}