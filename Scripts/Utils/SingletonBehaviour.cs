using UnityEngine;

namespace GameFramework
{
    public abstract class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
    {
        static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance != null) return _instance;

                _instance = FindObjectOfType<T>();
                if (_instance != null)
                {
                    _instance.Init();
                    return _instance;
                }

                var obj = new GameObject($"{typeof(T).Name}");
                _instance = obj.AddComponent<T>();
                _instance.Init();
                return _instance;
            }
        }

        bool _isInit = false;

        protected virtual void Awake()
        {
            if (_instance == this) return;

            if (_instance != null)
            {
                Debug.Log($"[SingletonBehaviour] Instance already exists: {typeof(T).Name}");
                Destroy(gameObject);
                return;
            }

            _instance = (T)this;
            Init();
        }

        void Init()
        {
            if (_isInit) return;

            OnInit();
            _isInit = true;
        }

        protected virtual void OnInit() { }

        protected virtual void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }
    }
}