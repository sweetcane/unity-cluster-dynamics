using System.Collections.Generic;
using GameFramework;
using UnityEngine;
using UnityEngine.Pool;

namespace Core.Particle
{
    public class ParticleManager : SingletonBehaviour<ParticleManager>
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private int defaultCapacity = 256;

        private ObjectPool<GameObject> _pool;
        private readonly HashSet<GameObject> _active = new();

        protected override void Awake()
        {
            base.Awake();

            _pool = new ObjectPool<GameObject>(
                createFunc: () =>
                {
                    var obj = Instantiate(prefab, transform);
                    return obj;
                },
                actionOnGet: obj =>
                {
                    obj.SetActive(true);
                    _active.Add(obj);
                },
                actionOnRelease: obj =>
                {
                    _active.Remove(obj);
                    obj.SetActive(false);
                },
                actionOnDestroy: obj =>
                {
                    if (obj != null) Destroy(obj);
                },
                defaultCapacity: defaultCapacity
            );
        }

        public GameObject Generate(Vector3 position, Quaternion? rotation = null)
        {
            var obj = _pool.Get();
            obj.transform.SetPositionAndRotation(position, rotation ?? Quaternion.identity);
            return obj;
        }

        public void Clear()
        {
            var temp = new List<GameObject>(_active);

            foreach (var obj in temp)
            {
                if (obj == null) continue;
                _pool.Release(obj);
            }
        }
    }
}