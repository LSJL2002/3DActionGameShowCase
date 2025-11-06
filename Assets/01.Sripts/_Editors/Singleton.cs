using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    private static readonly object _lock = new object();
    private static bool _applicationIsQuitting = false;

    public static T Instance
    {
        get
        {
            // 🔹 도메인 리로드 시 유령 참조 정리
            if (_instance != null && _instance.Equals(null))
                _instance = null;

            if (_applicationIsQuitting)
                return null;

            lock (_lock)
            {
                if (_instance == null)
                {
                    // 🔹 Unity 6 대응: FindAnyObjectByType (비활성 포함)
                    _instance = FindAnyObjectByType<T>(FindObjectsInactive.Include);

                    // ✅ 예외: PlayerManager는 자동 생성 금지
                    if (_instance == null && typeof(T).Name == "PlayerManager")
                    {
                        //Debug.LogWarning($"[Singleton<{typeof(T).Name}>] PlayerManager는 씬에 직접 배치해야 합니다. 자동 생성하지 않습니다.");
                        return null;
                    }

                    // 🔹 다른 싱글톤은 자동 생성
                    if (_instance == null)
                    {
                        GameObject singletonObj = new GameObject(typeof(T).Name);
                        _instance = singletonObj.AddComponent<T>();
                        singletonObj.hideFlags = HideFlags.DontSave;
                        DontDestroyOnLoad(singletonObj);
                        //Debug.Log($"[Singleton<{typeof(T).Name}>] 새 인스턴스 생성됨.");
                    }
                }

                return _instance;
            }
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject); // 중복 방지
        }
    }

    private void OnApplicationQuit()
    {
        _applicationIsQuitting = true;
    }

    protected virtual void OnDestroy()
    {
        if (_instance == this)
            _instance = null;
    }

    protected virtual void OnEnable() { }

    protected virtual void Start() { }

    protected virtual void Update() { }

    protected virtual void FixedUpdate() { }

    protected virtual void LateUpdate() { }

    protected virtual void OnDisable() { }


}
