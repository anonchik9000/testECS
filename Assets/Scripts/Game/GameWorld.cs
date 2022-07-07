
using Game.Components;
using Game.Systems;
using Leopotam.EcsLite;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GameWorld
    {
        private EcsSystems _systems;
        private EcsWorld _world;
        private SharedData _sharedData;

        private int _playerEntity;

        private EcsPool<TranformCmp> _transformPool;
        private EcsPool<MovableCmp> _movablesPool;
        private EcsPool<ButtonCmp> _buttonsPool;
        private EcsPool<DoorCmp> _doorsPool;
        public GameWorld(GameConfig config)
        {

            _world = new EcsWorld();
            _sharedData = new SharedData();
            _sharedData.ButtonsUpdate = new List<int>();
            _sharedData.DoorsUpdate = new List<int>();
            _sharedData.CharacterUpdate = true;
            _systems = new EcsSystems(_world, _sharedData);
            _systems.Add(new MovablesSys())
                    .Add(new ButtonsSys());

            _transformPool = _world.GetPool<TranformCmp>();
            _movablesPool = _world.GetPool<MovableCmp>();

            _playerEntity = _world.NewEntity();
            var characters = _world.GetPool<CharacterCmp>();
            characters.Add(_playerEntity);
            ref TranformCmp playerCmp = ref _transformPool.Add(_playerEntity);
            playerCmp.Position = config.PlayerPosition;

            _buttonsPool = _world.GetPool<ButtonCmp>();
            _doorsPool = _world.GetPool<DoorCmp>();

            var doorCreatedIndexes = new Dictionary<int,bool>(config.DoorConfigs.Length);

            foreach (var button in config.ButtonConfigs)
            {
                int buttonEntity = _world.NewEntity();
                _sharedData.ButtonsUpdate.Add(buttonEntity);

                ref ButtonCmp buttonCmp = ref _buttonsPool.Add(buttonEntity);
                buttonCmp.Radius = button.Radius;
                ref TranformCmp buttonTrCmp = ref _transformPool.Add(buttonEntity);
                buttonTrCmp.Position = new Vector3(button.Position.x,0, button.Position.y);

                if (button.DoorIndex >= 0 && button.DoorIndex < config.DoorConfigs.Length)
                {
                    doorCreatedIndexes.Add(button.DoorIndex,true);
                    buttonCmp.DoorEntity = CreateDoor(config, button.DoorIndex);
                }
            }
            if(doorCreatedIndexes.Count!=config.DoorConfigs.Length)
            {
                for(int i =0;i<config.DoorConfigs.Length;i++)
                {
                    if (!doorCreatedIndexes.ContainsKey(i))
                    {
                        CreateDoor(config, i);
                    }
                }
            }
            _systems.Init();
        }

        private int CreateDoor(GameConfig config,int doorIndex)
        {
            int doorEntity = _world.NewEntity();
            _sharedData.DoorsUpdate.Add(doorEntity);
            _doorsPool.Add(doorEntity);

            ref TranformCmp doorCmp = ref _transformPool.Add(doorEntity);
            var door = config.DoorConfigs[doorIndex];
            doorCmp.Position = door.Position;
            doorCmp.Size = door.Size;
            doorCmp.EulerRotation = door.EulerRotation;
            return doorEntity;
        }


        public void Run(float deltaTime)
        {
            _sharedData.ButtonsUpdate.Clear();
            _sharedData.CharacterUpdate = false;
            _sharedData.DoorsUpdate.Clear();
            _sharedData.DeltaTime = deltaTime;
            _systems?.Run();
        }

        public void OnDestroy()
        {
            if (_systems != null)
            {
                _systems.Destroy();
                _systems.GetWorld().Destroy();
                _systems = null;
            }
        }

        public SharedData GetChanges()
        {
            return _sharedData;
        }

        public TranformCmp GetPlayerData()
        {
            return _transformPool.Get(_playerEntity);
        }

        public TranformCmp GetTransformData(int entity)
        {
            return _transformPool.Get(entity);
        }

        public DoorCmp GetDoorData(int entity)
        {
            return _doorsPool.Get(entity);
        }

        public ButtonCmp GetButtonData(int entity)
        {
            return _buttonsPool.Get(entity);
        }

        public void SetPlayerEndPoint(Vector3 endPoint)
        {
            if (_movablesPool.Has(_playerEntity))
            {
                ref MovableCmp movable = ref _movablesPool.Get(_playerEntity);
                movable.EndPoint = endPoint;
            }
            else
            {
                ref MovableCmp movable = ref _movablesPool.Add(_playerEntity);
                movable.EndPoint = endPoint;
            }
        }
    }
}
