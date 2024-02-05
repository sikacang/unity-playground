using Cathei.BakingSheet;
using Cathei.BakingSheet.Unity;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using static Cathei.BakingSheet.Examples.GoogleSheetTools;

public static class GoogleSheetRetriever
{
    private static string credentialAssetName = "clever-reserve-370214-0b068d956cb3.json";

    [MenuItem("Google Sheet Retriever/Retrieve Data")]
    private static async void ImportGoogleSheetData()
    {
        var sheetContainer = new DataSheetContainer(UnityLogger.Default);

        string googleSheetId = "1JWq-OMMq1irGyadzc6FnJFA1_-QvfEBpSYhaPDpGb0k";

        var assetPath = Path.Combine(Application.streamingAssetsPath, credentialAssetName);
        var googleCredential = File.ReadAllText(assetPath);
        
        var googleConverter = new GoogleSheetConverter(googleSheetId, googleCredential);
        
        await sheetContainer.Bake(googleConverter);

        var jsonPath = Path.Combine(Application.streamingAssetsPath, "Google Sheet");
        var jsonConverter = new PrettyJsonConverter(jsonPath);

        await sheetContainer.Store(jsonConverter);
        AssetDatabase.Refresh();
        Debug.Log("Google sheet converted.");
    }
}
