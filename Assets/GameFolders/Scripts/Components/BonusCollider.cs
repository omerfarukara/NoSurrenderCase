using System;
using System.Collections;
using System.Collections.Generic;
using GameFolders.Scripts.Controllers.AI;
using GameFolders.Scripts.Controllers.Player;
using Sirenix.OdinInspector;
using UnityEngine;

public class BonusCollider : MonoBehaviour
{
    [SerializeField] private bool isPlayer, isAI;

    [ShowIf("isAI")] [SerializeField] internal AIController _aiController;
    [ShowIf("isPlayer")] [SerializeField] internal PlayerController _playerController;

}