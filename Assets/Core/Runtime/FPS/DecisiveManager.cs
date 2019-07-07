using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace MC.Core
{
    public class DecisiveManager : MonoBehaviour
    {
        public List<SimpleBot> friendBots = new List<SimpleBot>();

        public List<IHittableCharacter> enemies = new List<IHittableCharacter>();

        private Dictionary<IHittableCharacter, Transform> allocatedTransform = new Dictionary<IHittableCharacter, Transform>();

        private List<PlaceNode> defenceNodes = new List<PlaceNode>();
        private List<PlaceNode> attackNodes = new List<PlaceNode>();
        private List<PlaceNode> wayPointNodes = new List<PlaceNode>();

        private void Start()
        {
            defenceNodes = FindObjectsOfType<PlaceNode>().ToList().FindAll(val => val.nodeType == NodeType.DefencePoint);
            attackNodes = FindObjectsOfType<PlaceNode>().ToList().FindAll(val => val.nodeType == NodeType.AttackPoint);
            wayPointNodes = FindObjectsOfType<PlaceNode>().ToList().FindAll(val => val.nodeType == NodeType.WayPoint);

            foreach (var friendBot in friendBots)
            {
                var botLogic = friendBot.botLogic as DecisiveBotLogic;

                botLogic.OnSpotEnemy += enemy =>
                {
                    // 最简单的处理方法
                    botLogic.AttackStill(enemy);
                };
            }

            Debug.Log(friendBots.Count);
        }

        private void Update()
        {
            DeployBots();
            GetBotObservation();
        }

        private void AssignTransform(IHittableCharacter hittableCharacter, Transform trans)
        {
            // 释放之前占用的节点
            if (allocatedTransform.ContainsKey(hittableCharacter))
            {
                allocatedTransform.Remove(hittableCharacter);
            }

            allocatedTransform.Add(hittableCharacter, trans);
        }

        private Transform GetClosetDefencePoint(Vector3 pos)
        {
            var transList = defenceNodes;

            transList.Sort((a, b) =>
            {
                return Vector3.Distance(a.transform.position, pos).CompareTo(Vector3.Distance(b.transform.position, pos));
            });


            for (var i = 0; i < transList.Count; i++)
            {
                var trans = transList[i].transform;

                if (!allocatedTransform.ContainsValue(trans))
                {
                    return trans;
                }
            }

            return null;
        }

        private void DeployBots()
        {
            foreach (var friendBot in friendBots)
            {
                var botLogic = friendBot.botLogic as DecisiveBotLogic;

                if (botLogic.decisiveState == DecisiveState.Idle)
                {
                    var closetPoint = GetClosetDefencePoint(friendBot.transform.position);
                    AssignTransform(friendBot, closetPoint);
                    botLogic.Defence(closetPoint);
                }
            }
        }

        private void GetBotObservation()
        {
            foreach (var friendBot in friendBots)
            {
                var botLogic = friendBot.botLogic as DecisiveBotLogic;

                if (botLogic.decisiveState == DecisiveState.Defence)
                {
                    botLogic.ObserveEnemies(enemies);
                }
            }
        }
    }
}
