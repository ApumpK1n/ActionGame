using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Level
{
    /// <summary>
    /// 当前level中的Actor
    /// </summary>
    private List<Actor> m_Actors = new List<Actor>(16);

    private string m_CurrentLoadingLevelName;
    private Action<string, bool> m_OnSceneLoadedCallback;

    private World m_OwingWorld = null;

    public World OwingWorld { get { return m_OwingWorld; } }


    public Level()
    {
        m_CurrentLoadingLevelName = string.Empty;
        m_OnSceneLoadedCallback = null;
    }

    public void OnAddToWorld(World world)
    {
        if(world == null)
        {
            throw new ArgumentNullException(nameof (world));
        }

        m_OwingWorld = world;
        m_CurrentLoadingLevelName = string.Empty;
        m_OnSceneLoadedCallback = null;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    public void OnRemoveFromWorld()
    {
        m_OwingWorld = null;
        m_CurrentLoadingLevelName = string.Empty;
        m_OnSceneLoadedCallback = null;
    }

    public void AddActorToLevel(Actor actor)
    {
        if(actor == null)
        {
            throw new ArgumentNullException(nameof(actor));
        }

        m_Actors.Add(actor);

        actor.OnAddToLevel(this);
    }

    public void Tick(float deltaTime)
    {
        foreach (var actor in m_Actors)
        {
            actor.Tick(deltaTime);
        }
    }

    public bool LoadLevel(string levelName, LoadSceneMode loadSceneMode, Action<string, bool> callback)
    {
        if(string.IsNullOrEmpty(levelName))
        {
            UnityEngine.Debug.LogError("cannot load level for empty level name");
            return false;
        }

        if (m_CurrentLoadingLevelName == levelName)
        {
            UnityEngine.Debug.LogError(string.Format("cannot load level same time: {0}", levelName));
            return false;
        }

        m_CurrentLoadingLevelName = levelName;
        m_OnSceneLoadedCallback = callback;
        SceneManager.LoadSceneAsync(levelName, loadSceneMode);

        return true;
    }

    public void RemoveActorFromLevel(Actor actor)
    {
        if (actor != null)
        {
            m_Actors.Remove(actor);
        }
    }

    public void RemoveAllActors()
    {
        m_Actors.Clear();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="arg0"></param>
    /// <param name="arg1"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void OnSceneLoaded(Scene loadedScene, LoadSceneMode loadSceneMode)
    {
        if(loadedScene == null)
        {
            // 加载失败的
            m_OnSceneLoadedCallback?.Invoke(null, false);
        }

        if (loadedScene.name == m_CurrentLoadingLevelName)
        {
            m_OnSceneLoadedCallback?.Invoke(m_CurrentLoadingLevelName, true);
        }
        else
        {
            UnityEngine.Debug.LogWarning(string.Format("cannot load scene: {0}, {1}", m_CurrentLoadingLevelName, loadedScene.name));
            m_OnSceneLoadedCallback?.Invoke(null, false);
        }

        m_CurrentLoadingLevelName = null;
        m_OnSceneLoadedCallback = null;
    }
}
