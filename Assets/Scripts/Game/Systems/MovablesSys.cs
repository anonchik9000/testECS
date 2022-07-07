using Game.Components;
using Leopotam.EcsLite;

namespace Game.Systems
{
    sealed class MovablesSys : IEcsRunSystem,IEcsInitSystem {

        private EcsFilter _entities;
        private EcsPool<MovableCmp> _pool;
        private InOutData _inOutData;
        public MovablesSys(InOutData inOutData)
        {
            _inOutData = inOutData;
        }
        public void Init(EcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _entities = world.Filter< MovableCmp>().End();
            _pool = world.GetPool<MovableCmp>();
        }

        /// <summary>
        /// Игрок один, но системе должно быть все равно на количество сущностей которые она будет обрабатывать.
        /// Один вход в цикл не несет существенных затрат по производительности, за то система может быть легко отмасштабирована на несколько игроков или других движущихся к точке сущностей.
        /// Считаю такой подход приемлемым, с учетом того что это Movable система двигающая объект к точке, а не CharacterController
        /// </summary>
        /// <param name="systems"></param>
        public void Run (EcsSystems systems) {
            if (_inOutData.PositionChanged)
            {
                foreach (var entity in _entities)
                {
                    ref MovableCmp move = ref _pool.Get(entity);
                    var direction = _inOutData.CurrentPosition - move.Position;
                    float distance = direction.sqrMagnitude;

                    if (distance > move.SqrCmpDelta)
                    {
                        float dtSpeed = move.Speed * _inOutData.DeltaTime;
                        if (distance < dtSpeed)
                        {
                            move.Position = _inOutData.CurrentPosition;
                            _inOutData.PositionChanged = false;
                            
                        }
                        else
                        {
                            direction.Normalize();
                            move.Position += direction * dtSpeed;
                        }
                    }else
                    {
                        _inOutData.PositionChanged = false;
                    }
                }

                _inOutData.MovableUpdate = true;
            }
        }
    }
}