using UnityEngine;

namespace GameFolders.Scripts.General.Data
{
    [CreateAssetMenu(fileName = "CharacterMovementData", menuName = "Data/Character Movement Data")]
    public class CharacterMovementData : ScriptableObject
    {
        [Header("{--- Player ---}")] public float playerForwardSpeed;
        public float playerTurnSpeed;
        public float playerTakeKnock;
        public float playerTakeKnockSelf;

        [Header("{--- AI ---}")] public float aiForwardSpeed;
        public float aiTakeKnock;
        public float aiTakeKnockSelf;
    }
}
