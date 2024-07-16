using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class SpreadSheetLoader : MonoBehaviour
{
    const string url = "https://docs.google.com/spreadsheets/d/12dZ52iIBE2oQs2zJw_TxLK5BY_9XkVmRRCPd9j6qeKo/export?format=csv";

    [Button]
    public async static Task<string> LoadGoogleSheetAsync()
    {
        using UnityWebRequest request = UnityWebRequest.Get(url);
        Debug.Log($"requestURL\n{url}");
        UnityWebRequestAsyncOperation op = request.SendWebRequest();

        // Wrap the completion handler in a Task
        var completionTask = new TaskCompletionSource<string>();
        op.completed += (aop1) =>
        {
            if (request.result != UnityWebRequest.Result.Success)
            {
                
            }

            string data = request.downloadHandler.text;
            Debug.Log(data);
            completionTask.SetResult(data);
        };

        return await completionTask.Task;
    }
}
