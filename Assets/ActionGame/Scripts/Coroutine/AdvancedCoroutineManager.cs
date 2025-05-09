using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 协程管理，不用依赖于原来的Mono
/// </summary>

public class AdvancedCoroutineManager : DestroyableSingleton<AdvancedCoroutineManager>
{
    private Dictionary<string, CoroutineData> coroutines = new Dictionary<string, CoroutineData>();
    private Dictionary<int, List<string>> priorityGroups = new Dictionary<int, List<string>>();
    private int maxSimultaneousCoroutines = 20; // Limit the number of coroutines running concurrently
    private Queue<CoroutineData> waitingQueue = new Queue<CoroutineData>();

    #region Override
    public override void OnSingletonInit()
    {
        base.OnSingletonInit();
        DontDestroyOnLoad(gameObject);
    }

    protected void OnDestroy()
    {
        List<string> keys = new List<string>(coroutines.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            string handle = keys[i];
            StopCoroutineEx(handle);
        }
        coroutines.Clear();
        priorityGroups.Clear();
        waitingQueue.Clear();

    }
    #endregion

    #region Public
    public string StartCoroutineEx(IEnumerator coroutine, int priority = 0)
    {
        string handle = Guid.NewGuid().ToString();
        var wrapper = new CoroutineWrapper(coroutine);
        var coroutineData = new CoroutineData { Handle = null, Wrapper = wrapper, Priority = priority, HandleId = handle };

        if (coroutines.Count >= maxSimultaneousCoroutines)
        {
            waitingQueue.Enqueue(coroutineData);
        }
        else
        {
            StartCoroutineData(coroutineData);
        }
        return handle;
    }

    public void StopCoroutineEx(string handle)
    {
       if (TryRemoveCoroutine(handle))
       {
            OnCoroutineStopped?.Invoke(handle);
        }
    }

    public void PauseCoroutine(string handle)
    {
        if (coroutines.TryGetValue(handle, out CoroutineData data))
        {
            data.Wrapper.Pause();
        }
    }

    public void ResumeCoroutine(string handle)
    {
        if (coroutines.TryGetValue(handle, out CoroutineData data))
        {
            data.Wrapper.Resume();
        }
    }
    #endregion


    #region Private
    private void StartCoroutineData(CoroutineData data)
    {
        Coroutine co = StartCoroutine(data.Wrapper.Run(() => CompleteCoroutine(data.HandleId), ex => ErrorCoroutine(data.HandleId, ex)));
        data.Handle = co;
        coroutines[data.HandleId] = data;
        if (!priorityGroups.ContainsKey(data.Priority))
        {
            priorityGroups[data.Priority] = new List<string>();
        }
        priorityGroups[data.Priority].Add(data.HandleId);
    }

    private void CompleteCoroutine(string handle)
    {
        TryRemoveCoroutine(handle);
        OnCoroutineComplete?.Invoke(handle);
        TryDequeueCoroutine();
    }

    private void ErrorCoroutine(string handle, Exception ex)
    {
        TryRemoveCoroutine(handle);
        Debug.LogError($"Coroutine {handle} error: {ex}");
        OnCoroutineError?.Invoke(handle, ex);
        TryDequeueCoroutine();
    }

    private bool TryRemoveCoroutine(string handle)
    {
        if (coroutines.TryGetValue(handle, out CoroutineData data))
        {
            priorityGroups[data.Priority].Remove(handle);
            coroutines.Remove(handle);
            if (data.Handle != null)
            {
                StopCoroutine(data.Handle);
            }
            return true;
        }
        return false;
    }

    private void TryDequeueCoroutine()
    {
        if (waitingQueue.Count > 0 && coroutines.Count < maxSimultaneousCoroutines)
        {
            CoroutineData data = waitingQueue.Dequeue();
            StartCoroutineData(data);
        }
    }
    #endregion

    #region Coroutine
    // Events
    public event Action<string> OnCoroutineComplete;
    public event Action<string, Exception> OnCoroutineError;
    public event Action<string> OnCoroutineStopped;

    private class CoroutineData
    {
        public Coroutine Handle;
        public CoroutineWrapper Wrapper;
        public int Priority;
        public string HandleId;
    }

    private class CoroutineWrapper
    {
        private readonly IEnumerator _coroutine;
        private bool _isPaused = false;

        public CoroutineWrapper(IEnumerator coroutine)
        {
            _coroutine = coroutine;
        }

        public IEnumerator Run(Action onComplete, Action<Exception> onError)
        {
            while (true)
            {
                if (_isPaused)
                {
                    yield return null;
                    continue;
                }

                try
                {
                    if (_coroutine.MoveNext() == false)
                    {
                        onComplete?.Invoke();
                        yield break;
                    }
                }
                catch (Exception ex)
                {
                    onError?.Invoke(ex);
                    yield break;
                }

                yield return _coroutine.Current;
            }
        }

        public void Pause()
        {
            _isPaused = true;
        }

        public void Resume()
        {
            _isPaused = false;
        }
    }
    #endregion
}
