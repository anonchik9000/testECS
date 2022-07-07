using Game.Components;
using Leopotam.EcsLite;

namespace Game.Systems
{
    sealed class MovablesSys : IEcsRunSystem,IEcsInitSystem {

        private EcsFilter _entities;
        private EcsPool<MovableCmp> _moveblesPool;
        private EcsPool<TranformCmp> _transformsPool;
        private SharedData _shared;

        public void Init(EcsSystems systems)
        {
            _shared = systems.GetShared<SharedData>();
            EcsWorld world = systems.GetWorld();
            _entities = world.Filter<MovableCmp>().End();
            _moveblesPool = world.GetPool<MovableCmp>();
            _transformsPool = world.GetPool<TranformCmp>();
        }

        public void Run (EcsSystems systems) 
        {
            foreach (var entity in _entities)
            {
                ref MovableCmp move = ref _moveblesPool.Get(entity);
                ref TranformCmp transform = ref _transformsPool.Get(entity);
                var direction = move.EndPoint - transform.Position;
                float distance = direction.sqrMagnitude;

                if (distance > move.SqrCmpDelta)
                {
                    float dtSpeed = move.Speed * _shared.DeltaTime;
                    if (distance < dtSpeed)
                    {
                        transform.Position = move.EndPoint;
                        _moveblesPool.Del(entity);

                    }
                    else
                    {
                        direction.Normalize();
                        transform.Position += direction * dtSpeed;
                    }
                }
                else
                {
                    _moveblesPool.Del(entity);
                }
                _shared.CharacterUpdate = true;
            }   
        }
    }
}