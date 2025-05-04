using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class World
{
    private GameDirector m_GameDirector;

    /// <summary>
    /// 当前World拥有的level
    /// </summary>
    private List<Level> m_Levels = new List<Level>(4);

    private Level m_CurrentLevel;

    public Level CurrentLevel
    {
        get { return m_CurrentLevel; }
    }

    public World()
    {
        m_CurrentLevel = null;
    }

    /// <summary>
    /// 创建world
    /// </summary>
    public static World CreateWorld()
    {
        World world = new World();

        world.InitializeWorld();

        return world;
    }

    public void SetGameDirector(GameDirector director)
    {
        if (director == null)
        {
            throw new ArgumentNullException(nameof(director));
        }

        m_GameDirector = director;
    }

    public void InitializeWorld()
    {
        m_CurrentLevel = new Level();
        m_CurrentLevel.OnAddToWorld(this);
        //m_Levels.Add(m_CurrentLevel);

        AddLevel(m_CurrentLevel);
    }

    /// <summary>
    /// 加载关卡地图
    ///
    /// TODO：这里没有完全想好，先跑通，后续重构
    /// </summary>
    public bool LoadLevel(string levelName, LoadSceneMode loadSceneMode, Action<string, bool> callback)
    {
        if (string.IsNullOrEmpty(levelName))
        {
            return false;
        }

        if (m_CurrentLevel == null)
        {
            Debug.LogError("world is not initialized");
            return false;
        }

        m_CurrentLevel.LoadLevel(levelName, loadSceneMode, callback);

        return true;
    }

    public void AddLevel(Level level)
    {
        if(level == null)
        {
            throw new ArgumentNullException(nameof(level));
        }

        m_Levels.Add(level);
    }

    public PlayerController SpawnPlayerController()
    {
        PlayerController playerController = new PlayerController();

        

        return playerController;
    }

    /// <summary>
    /// 生成主角色，加入world
    /// </summary>
    /// <returns></returns>
    public Character SpawnCharacter()
    {
        Character character = new Character();

        // 把创建的角色加入到level中
        // TODO: 给起始点
        m_CurrentLevel.AddActorToLevel(character);

        return character;
    }

    
}
