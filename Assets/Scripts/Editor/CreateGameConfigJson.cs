using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class CreateGameConfigJson : Editor
{
    [MenuItem("GameConfig/Create")]
    static void CreateConfig()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        var buttons = GameObject.FindGameObjectsWithTag("Button");
        GameConfig config = new GameConfig();
        config.PlayerPosition = new Vector2(player.transform.position.x, player.transform.position.z);
        config.ButtonConfigs = new ButtonConfig[buttons.Length];
        config.DoorConfigs = new DoorConfig[buttons.Length];
        for(int i =0;i< buttons.Length;i++)
        {
            var button = buttons[i];
            ButtonConfig bConfig = new ButtonConfig();
            bConfig.DoorIndex = i;
            bConfig.Position = new Vector2(button.transform.position.x, button.transform.position.z);
            bConfig.Radius = button.transform.localScale.x;
            config.ButtonConfigs[i] = bConfig;

            var door = button.transform.GetChild(0);
            DoorConfig dConfig = new DoorConfig();
            dConfig.Position = door.position;
            dConfig.Size = door.lossyScale;
            dConfig.EulerRotation = door.eulerAngles;
            config.DoorConfigs[i] = dConfig;
        }
        TextAsset text = new TextAsset(JsonUtility.ToJson(config));
        AssetDatabase.CreateAsset(text, "Assets/Contents/Config.asset");
        AssetDatabase.SaveAssets();
    }
}
