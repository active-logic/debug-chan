using UnityEditor;
using UnityEngine;

namespace Activ.Loggr.UI{
public static class Menu{

    [MenuItem("Window/Activ/Debug-Chan/Console")]
    static void OpenConsole() => LogWindow.DisplayWindow();

    [MenuItem("Window/Activ/Debug-Chan/Config")]
    static void EditConfig(){
        Debug.Log("Edit config not available yet");
    }

}}
