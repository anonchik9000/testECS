using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectController : MonoBehaviour
{
    [Header("Settings")]
    public TextAsset GameConfigJson;

    [Header("View")]
    public CharacterViewer CharacterViewer;
    public DoorsViewer DoorsViewer;
    public ButtonsViewer ButtonsViewer;

    private GameWorld _game;
    private InOutData _inOutData;

    private void Start()
    {
        GameConfig config = JsonUtility.FromJson<GameConfig>(GameConfigJson.text);
        _inOutData = new InOutData();
        _inOutData.PositionChanged = false;
        _inOutData.DoorsUpdate = new List<int>();
        _inOutData.ButtonsUpdate = new List<int>();
        _game = new GameWorld(config, _inOutData);

        var playerData = _game.GetPlayerData();
        CharacterViewer.UpdateView(playerData);

        UpdateDoors();
        UpdateButtons();

    }

    private void Update()
    {
        _inOutData.MovableUpdate = false;
        _inOutData.DoorsUpdate.Clear();
        _inOutData.ButtonsUpdate.Clear();
        _inOutData.DeltaTime = Time.deltaTime;
        
        _game.Run();

        if (_inOutData.MovableUpdate)
        {
            var playerData = _game.GetPlayerData();
            CharacterViewer.UpdateView(playerData);
        }

        UpdateDoors();
        UpdateButtons();
    }

    private void UpdateButtons()
    {
        if(_inOutData.ButtonsUpdate.Count>0)
        {
            foreach(var button in _inOutData.ButtonsUpdate)
            {
                var buttonData = _game.GetButtonData(button);
                ButtonsViewer.UpdateView(button, buttonData);
            }
        }
    }

    private void UpdateDoors()
    {
        if (_inOutData.DoorsUpdate.Count > 0)
        {
            foreach (var door in _inOutData.DoorsUpdate)
            {
                var doorData = _game.GetDoorData(door);
                DoorsViewer.UpdateView(door, doorData);
            }
        }
    }

    private void OnDestroy()
    {
        _game.OnDestroy();
        _game = null;
    }

    public void SetMovePoint(Vector3 point)
    {
        _inOutData.CurrentPosition = new Vector2(point.x, point.z);
        _inOutData.PositionChanged = true;
    }
}
