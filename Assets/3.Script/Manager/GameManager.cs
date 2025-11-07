using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonBehaviour<GameManager>
{
    // 게임 씬 호출 이후 무조건 
    private SceneContext currentSceneContext;
    public SceneContext CurrentSceneContext => currentSceneContext;

}
