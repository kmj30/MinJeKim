using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonAI : MonoBehaviour
{
    private Animator anim;

    // 파라미터 ID를 저장할 리스트
    private List<int> parameterHashes = new List<int>();

    // 애니메이션 해시 변수들
    int IdleSimple, IdleAgressive, IdleRestless, Walk, BattleStance,
        Bite, Drakaris, FlyingFWD, FlyingAttack, Hover, Lands, TakeOff, Die;

    void Start()
    {
        anim = GetComponent<Animator>();

        // 해시 등록 및 리스트에 추가 (한 번에 관리하기 위함)
        RegisterParameter(out IdleSimple, "IdleSimple");
        RegisterParameter(out IdleAgressive, "IdleAgressive");
        RegisterParameter(out IdleRestless, "IdleRestless");
        RegisterParameter(out Walk, "Walk");
        RegisterParameter(out BattleStance, "BattleStance");
        RegisterParameter(out Bite, "Bite");
        RegisterParameter(out Drakaris, "Drakaris");
        RegisterParameter(out FlyingFWD, "FlyingFWD");
        RegisterParameter(out FlyingAttack, "FlyingAttack");
        RegisterParameter(out Hover, "Hover");
        RegisterParameter(out Lands, "Lands");
        RegisterParameter(out TakeOff, "TakeOff");
        RegisterParameter(out Die, "Die");
    }

    // 파라미터를 등록하는 보조 함수
    void RegisterParameter(out int hashVar, string name)
    {
        hashVar = Animator.StringToHash(name);
        parameterHashes.Add(hashVar);
    }

    // 모든 파라미터를 false로 만들고 특정 하나만 true로 만드는 핵심 함수
    void SetState(int activeHash)
    {
        foreach (int hash in parameterHashes)
        {
            anim.SetBool(hash, hash == activeHash);
        }
    }

    void Update()
    {
        // 입력 키와 대응하는 상태 및 애니메이션 이름 매핑
        HandleInput(KeyCode.W, Walk, "IdleSimple", "Walk");
        HandleInput(KeyCode.Q, IdleAgressive, "IdleSimple", "IdleAgressive");
        HandleInput(KeyCode.E, IdleRestless, "IdleSimple", "IdleRestless");
        HandleInput(KeyCode.R, BattleStance, "IdleSimple", "BattleStance");
        HandleInput(KeyCode.T, Bite, "IdleSimple", "Bite");
        HandleInput(KeyCode.Y, Drakaris, "IdleSimple", "Drakaris");
        HandleInput(KeyCode.U, FlyingFWD, "IdleSimple", "FlyingFWD");
        HandleInput(KeyCode.I, FlyingAttack, "IdleSimple", "FlyingAttack");
        HandleInput(KeyCode.O, Hover, "IdleSimple", "Hover");
        HandleInput(KeyCode.A, Lands, "IdleSimple", "Lands");
        HandleInput(KeyCode.P, TakeOff, "IdleSimple", "TakeOff");
        HandleInput(KeyCode.D, Die, "IdleSimple", "Die");
    }

    // 입력 처리를 단순화하는 함수
    void HandleInput(KeyCode key, int targetHash, string startState, string activeState)
    {
        if (Input.GetKeyDown(key))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName(startState))
                SetState(targetHash);
        }
        else if (Input.GetKeyUp(key))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName(activeState))
                SetState(IdleSimple);
        }
    }
}
