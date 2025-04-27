using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// 大世界场景游戏逻辑控制 管理世界中所有元素 统一Tick
/// </summary>

public class WorldLogicSystem : IGameSystem
{
    private WorldScene worldScene;

    public SystemType TypeEnum
    {
        get
        {
            return SystemType.WorldLogic;
        }
    }

    public void Dispose()
    {
        SceneManager.sceneLoaded -= SceneLoaded;
    }

    public void Setup()
    {
        Debug.Log("Setup");
        SceneManager.sceneLoaded += SceneLoaded;
    }

    public void Start()
    {

    }

    private void SceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        Debug.Log("SceneLoaded");
        GameObject[] gameObjects  = scene.GetRootGameObjects();
        foreach (GameObject gameObject in gameObjects)
        {
            WorldScene worldScene = gameObject.GetComponent<WorldScene>();
            if (worldScene != null)
            {
                this.worldScene = worldScene;
                worldScene.Setup();
            }
        }
    }

    public void LoadWorld(WorldConfig config)
    {
        foreach(var sceneConfig in config.SceneConfigs)
        {
            SceneManager.LoadScene(sceneConfig.Name, sceneConfig.LoadSceneMode);
        }
    }
    public void Tick(float deltaTime)
    {
        if (worldScene != null)
        {
            worldScene.Tick(deltaTime);
        }

    }

    public void PlayerMove(Vector2 dir)
    {
        worldScene.Player.Move(dir);
    }

    // TODO:抽象出属性
    public void PlayerAccelerate(bool isAccelerate)
    {
        worldScene.Player.SetAccelerate(isAccelerate);
    }

    public void PlayerJump()
    {
        //Game.Instance.Player.TryEnterJumpState();
    }

    public void LeftClick()
    {

    }

    // TODO: 统一执行Command接口
    public void ExecuteCommand(CommandType commandType)
    {
        worldScene.Player.ExecuteCommand(commandType);
    }
}
