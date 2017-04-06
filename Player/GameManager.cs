﻿//2017/2/23
//by Chao

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Player player;
    //世界物理参数
    public static float gravity = 9.8f;

    //游戏状态
    public static bool gameOver = false;

    void Start()
    {
        gameOver = false;
        player = GameObject.Find("Player").GetComponent<Player>();

    }

    void LateUpdate()
    {
        if (player.died)
            gameOver = true;
        if (gameOver)
            SceneManager.LoadScene("Begin");
    }


}