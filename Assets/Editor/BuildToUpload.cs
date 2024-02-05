#if Debug
using System.Diagnostics;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class BuildToUpload: Editor,  IPostprocessBuildWithReport
{
    private const string Command = "firebase deploy";
    
    public int callbackOrder { get; }
    public void OnPostprocessBuild(BuildReport report)
    {
        Debug.Log("웹 호스팅 시작");
        EditorApplication.delayCall += TaskStart;
    }

    private static void TaskStart()
    {
        ExecuteCmd(Command);
        EditorApplication.delayCall -= TaskStart;
    }
    
    /// <summary>
    /// cmd : 실행할 명령어 
    /// </summary>
    private static void ExecuteCmd(string cmd)
    {
        ProcessStartInfo startInfo = new();
        startInfo.FileName = "cmd.exe";
        startInfo.Arguments = "/C " + cmd;

        Process process = new();
        process.StartInfo = startInfo;
        process.Start();
        
        process.WaitForExit();
        Debug.Log("웹 호스팅 끝");
    }
    
    [MenuItem("Tools/Deploy")]
    public static void Deploy()
    {
        ExecuteCmd(Command);
    }
    
    [MenuItem("Tools/Open Page")]
    public static void GoWebGame()
    {
        Application.OpenURL("https://bleachbird-2ae7a.firebaseapp.com/");
    }
}
#endif