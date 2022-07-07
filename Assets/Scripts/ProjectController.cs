using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private TextAsset _gameConfigJson;

    [Header("View")]
    [SerializeField] private CharacterViewer _characterViewer;
    [SerializeField] private DoorsViewer _doorsViewer;
    [SerializeField] private ButtonsViewer _buttonsViewer;

    private GameWorld _game;
    private Vector3 _characterEndPoint;

    private void Start()
    {
        GameConfig config = JsonUtility.FromJson<GameConfig>(_gameConfigJson.text);
        _game = new GameWorld(config);

        var playerData = _game.GetPlayerData();
        _characterViewer.UpdateView(playerData,Vector3.zero);
        var changes = _game.GetChanges();
        UpdateDoors(changes);
        UpdateButtons(changes);

    }

    private void Update()
    {
        
        _game.Run(Time.deltaTime);

        var changes = _game.GetChanges();
        if (changes.CharacterUpdate)
        {
            var playerData = _game.GetPlayerData();
            _characterViewer.UpdateView(playerData, _characterEndPoint);
        }
        UpdateDoors(changes);
        UpdateButtons(changes);
    }

    private void UpdateButtons(SharedData changes)
    {
        if(changes.ButtonsUpdate.Count>0)
        {
            foreach(var button in changes.ButtonsUpdate)
            {
                var buttonData = _game.GetButtonData(button);
                var buttonTrData = _game.GetTransformData(button);
                _buttonsViewer.UpdateView(button, buttonData, buttonTrData);
            }
        }
    }

    private void UpdateDoors(SharedData changes)
    {
        if (changes.DoorsUpdate.Count > 0)
        {
            foreach (var door in changes.DoorsUpdate)
            {
                var doorData = _game.GetDoorData(door);
                var doorTrData = _game.GetTransformData(door);
                _doorsViewer.UpdateView(door, doorData, doorTrData);
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
        point.y = 0;
        _characterEndPoint = point;
        _game.SetPlayerEndPoint(point);
    }
}
