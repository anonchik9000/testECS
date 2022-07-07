
using Game.Components;
using Game.Systems;
using Leopotam.EcsLite;
using System.Collections.Generic;

namespace Game
{
    public class GameWorld
    {
        private EcsSystems _systems;
        private EcsWorld _world;
        private InOutData _inOutData;

        private int _playerEntity;

        private EcsPool<MovableCmp> _movablesPool;
        private EcsPool<ButtonCmp> _buttonsPool;
        private EcsPool<DoorCmp> _doorsPool;
        public GameWorld(GameConfig config, InOutData inOutData)
        {
            _inOutData = inOutData;
            _world = new EcsWorld();
            _systems = new EcsSystems(_world);
            _systems.Add(new MovablesSys(inOutData))
                    .Add(new ButtonsSys(inOutData));

            _playerEntity = _world.NewEntity();
            _movablesPool = _world.GetPool<MovableCmp>();
            ref MovableCmp playerCmp = ref _movablesPool.Add(_playerEntity);
            playerCmp.Position = config.PlayerPosition;

            _buttonsPool = _world.GetPool<ButtonCmp>();
            _doorsPool = _world.GetPool<DoorCmp>();

            var doorCreatedIndexes = new Dictionary<int,bool>(config.DoorConfigs.Length);

            foreach (var button in config.ButtonConfigs)
            {
                int buttonEntity = _world.NewEntity();
                inOutData.ButtonsUpdate.Add(buttonEntity);

                ref ButtonCmp buttonCmp = ref _buttonsPool.Add(buttonEntity);
                buttonCmp.Position = button.Position;
                buttonCmp.Radius = button.Radius;
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
            _inOutData.DoorsUpdate.Add(doorEntity);

            ref DoorCmp doorCmp = ref _doorsPool.Add(doorEntity);
            var door = config.DoorConfigs[doorIndex];
            doorCmp.Position = door.Position;
            doorCmp.Size = door.Size;
            doorCmp.EulerRotation = door.EulerRotation;
            return doorEntity;
        }


        public void Run()
        {
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

        public MovableCmp GetPlayerData()
        {
            return _movablesPool.Get(_playerEntity);
        }

        public DoorCmp GetDoorData(int entity)
        {
            return _doorsPool.Get(entity);
        }

        public ButtonCmp GetButtonData(int entity)
        {
            return _buttonsPool.Get(entity);
        }
    }
}
