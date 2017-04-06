﻿//2017/3/30
//by Chao
using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    //持有武器
    public Weapon weapon;
    public Gun gun;
    //角色数值
    public int maxHP = 100;
    public int maxMP = 30;
    public int maxSP = 30;

    public int HP;
    public int MP;
    public int SP;
    //经验技能
    private int Level=0;
    static private int maxLV=100;
    private int previousLV = 0;
    public int EXP=0;
    private int[] lv=new int[maxLV];//每级所需经验
    public bool upgraded = false;
    private int enemyKilled = 0;
    //金钱&点数
    public int Gold=0;
    //持有物品

    int previousHp;

    public float jumpSpeed = 3;       //delta time优化，影响起跳-落地时间
    public float jumpVelocity = 6f;  //跳跃初速度，影响自由落体最大高度
    public float jumpMultiple = 1.15f;//跳跃-高跳系数 
    public float moveSpeed = 6;     //6
    public float rumMultiple = 1.6f; //移动-奔跑系数1.6
    //角色动画状态
    private Animator anim;
    //private AnimatorStateInfo currentBaseState;
    //角色体力状态
    public bool isDamaged = false;
    public bool isDying = false;
    public bool died = false;
    //角色运动状态
    private Controller controller;
    public bool faceRight = true;
    public bool stay = false;
    public bool isStanding = false;
    public bool isWalking = false;
    public bool isClimbing = false;
    public bool isJumping = false;
    public bool isRunning = false;
    public bool isFalling = false;
    public bool isDroping = false;

    //角色攻击状态
    public bool isAttacking = false;
    public bool isFiring = false;
    public float attackSpeed = 0.05f;
    public float fireSpeed = 0.1f;
    public bool isDefancing = false;

    //角色位置状态
    public bool isGround = false;
    public bool isOnStair = false;

    void Start()
    {
        controller = GetComponent<Controller>();
        anim = transform.FindChild("Misaki").GetComponent<Animator>();

        died = false;
        HP = maxHP;
        MP = maxMP;
        SP = maxSP;
        //每级经验初始化
        lv[0] = 30;
        for (int i = 1; i < maxLV; i++)
        {
            lv[i] = lv[i - 1] + 50 + 25 * (int)(i * 0.5);
        }

    }
    private void FixedUpdate()
    {
        //if (!anim.IsInTransition(0))
        //将游戏管理器中角色状态值赋予动画组件
        //currentBaseState = anim.GetCurrentAnimatorStateInfo(0);//更新动画状态
        anim.SetBool("isStanding", isStanding);
        anim.SetBool("isRunning", isRunning);
        anim.SetBool("isWalking", isWalking);
        anim.SetBool("isJumping", isJumping);

    }
    void Update()
    {
        if (HP < previousHp&&!isDamaged)
        {
            StartCoroutine(Damaged());
        }
        if (HP <= 0&&!isDying)
        {
            StartCoroutine(Die());
        }
        if (HP > 0)
        {
            isDying = false;
            controller.enabled = true;
            anim.SetBool("isDying", isDying);//更新动画，下同
        }


        if (Input.GetButtonDown("Attack") && !isAttacking)
        {
            StartCoroutine(Attack());
        }
        if (Input.GetButtonDown("Fire") && !isFiring)
        {
            StartCoroutine(Fire());
        }
        if (EXP > lv[Level]&&!upgraded)
            StartCoroutine(Upgrade());
        previousHp = HP;
    }
    //使用HP药水
    private void UseHpPotion()
    {

    }
    //使用MP药水
    private void UseMpPotion()
    {

    }
    private IEnumerator Upgrade()
    {
        upgraded = true;
        previousLV = Level;
        anim.SetBool("isUpgrade",upgraded);
        yield return new WaitForSeconds(1f);
        ++Level;
        Debug.Log("Upgrade!");
        upgraded = false;
        anim.SetBool("isUpgrade",upgraded);
    }
    private IEnumerator Damaged()
    {
        Debug.Log("Damaged!"+HP.ToString());
        isDamaged = true;
        anim.SetBool("isDamaged", isDamaged);
        yield return new WaitForSeconds(0.2f);
        isDamaged = false;
        anim.SetBool("isDamaged",isDamaged);
    }
    private IEnumerator Die()
    {
        Debug.Log("Now DYING!!!");
        isDying = true;
        controller.enabled = false;
        anim.SetBool("isDying",isDying);
        yield return new WaitForSeconds(3f+Level*0.1f);
        if (isDying)
        {
            Debug.Log("Died!T T");
            died = true;
        }
    }
    //攻击
    private IEnumerator Attack()
    {
        Debug.Log("Attack");
        isAttacking = true;
        anim.SetBool("isAttacking", isAttacking);
        stay = true;
        yield return new WaitForSeconds(attackSpeed);
        Debug.Log("Attack End");
        isAttacking = false;
        anim.SetBool("isAttacking", isAttacking);
        stay = false;
        //yield return StartCoroutine(InnerUnityCoroutine());协程嵌套
    }
    //远程导弹攻击
    private IEnumerator Fire()
    {
        Debug.Log("Fire");
        gun.Fire();
        isFiring = true;
        anim.SetBool("isFiring", isFiring);
        stay = true;
        yield return new WaitForSeconds(fireSpeed);
        Debug.Log("Fire End");
        isFiring = false;
        anim.SetBool("isFiring", isFiring);
        stay = false;
    }
    //防御
    private void Defence()
    {

    }
    private void KillEnemy()
    {
        enemyKilled++;
        Debug.Log("Kill Total" + enemyKilled.ToString());
    }

}