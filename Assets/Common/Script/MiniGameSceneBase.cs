using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 미니게임 씬의 베이스 클래스<br/>
/// Start 구현시 override 해서 구현 및 base.Start 호출 요함
/// </summary>
public abstract class MiniGameSceneBase : MonoBehaviourPunCallbacks
{
    protected virtual void Start()
    {
        // 이미 방에 들어가 있고
        // 게임에 참여할 플레이어가 모두 방에 들어가있어야 한다
        if (false == PhotonNetwork.InRoom)
        {
            Debug.LogWarning("방에 참여해 있지 않습니다");
            return;
        }

        if (PhotonNetwork.IsMasterClient)
        {
            ReadyNetworkScene();
        }

        ReadyPlayerClient();

        // 로딩 완료 통지
        PhotonNetwork.LocalPlayer.SetLoad(true);
    }

    /// <summary>
    /// 마스터 클라이언트에서 수행할 씬 초기화 작업<br/>
    /// 룸 오브젝트 생성 등
    /// </summary>
    protected abstract void ReadyNetworkScene();

    /// <summary>
    /// 각 클라이언트에서 수행할 씬 초기화 작업<br/>
    /// 플레이어 생성, RPC가 아닌 초기화 함수 호출 등
    /// </summary>
    protected abstract void ReadyPlayerClient();

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps.ContainsKey(CustomProperty.LOAD))
        {
            if (CheckAllLoad())
                GameStart();
        }
    }

    private bool CheckAllLoad()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
            if (!player.GetLoad()) return false;
        return true;
    }

    /// <summary>
    /// 각 클라이언트에서 게임 시작 시점에 할 작업(예: 게임 타이머 시작)
    /// </summary>
    protected abstract void GameStart();
}