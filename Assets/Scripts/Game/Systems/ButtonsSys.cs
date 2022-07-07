using Game.Components;
using Leopotam.EcsLite;
using System.Collections.Generic;

namespace Game.Systems
{
    sealed class ButtonsSys : IEcsRunSystem,IEcsInitSystem {

        private EcsFilter _movableEntities;
        private EcsFilter _buttonEntities;

        private EcsPool<ButtonCmp> _buttonsPool;
        private EcsPool<DoorCmp> _doorsPool;
        private EcsPool<TranformCmp> _transformsPool;

        private List<int> _openingDoors;

        private SharedData _shared;


        public void Init(EcsSystems systems)
        {
            _shared = systems.GetShared<SharedData>();
            EcsWorld world = systems.GetWorld();
            _movableEntities = world.Filter<CharacterCmp>().End();

            _buttonEntities = world.Filter<ButtonCmp>().End();
            _buttonsPool = world.GetPool<ButtonCmp>();

            _doorsPool = world.GetPool<DoorCmp>();

            _transformsPool = world.GetPool<TranformCmp>();

            _openingDoors = new List<int>(_doorsPool.GetRawDenseItemsCount());
        }
        /// <summary>
        /// ¬ задании указанно, что систему столкновений не реализуем, соотвественно тупой перебор и сравнение позиции каждого двигающегос€ объекта с позицией каждой кнопки.
        /// ¬ реальности уместно построение aabb дерева, дл€ определени€ колизий в шаред логике.
        /// </summary>
        /// <param name="systems"></param>
        public void Run(EcsSystems systems)
        {
            _openingDoors.Clear();
            foreach (var buttonEntity in _buttonEntities)
            {
                ref ButtonCmp button = ref _buttonsPool.Get(buttonEntity);
                ref TranformCmp btnTransform = ref _transformsPool.Get(buttonEntity);
                bool oldPressed = button.IsPressed;
                button.IsPressed = false;
                foreach (var movableEntity in _movableEntities)
                {
                    ref TranformCmp transform = ref _transformsPool.Get(movableEntity);
                    if (button.Contains(transform.Position, btnTransform.Position))
                    {
                        button.IsPressed = true;
                        _openingDoors.Add(button.DoorEntity);
                    }
                }
                if (oldPressed != button.IsPressed)
                {
                    _shared.ButtonsUpdate.Add(buttonEntity);
                }
            }
            foreach (var doorEntity in _openingDoors)
            {
                if (doorEntity >= 0)
                {
                    ref DoorCmp door = ref _doorsPool.Get(doorEntity);
                    if (door.OpeningProgress > 0)
                    {
                        door.OpeningProgress -= door.OpeningSpeed * _shared.DeltaTime;
                        if (door.OpeningProgress < 0)
                        {
                            door.OpeningProgress = 0;
                        }
                        _shared.DoorsUpdate.Add(doorEntity);
                    }
                }
            }
        }
    }
}