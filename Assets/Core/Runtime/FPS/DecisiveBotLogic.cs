using System.Collections.Generic;
using UnityEngine;


namespace MC.Core
{
    public enum DecisiveState
    {
        Idle, //空闲
        MoveTo, //前往某地中
        Defence,//防守
        PatrolAB,//两点间巡逻
        AttackStill,//静止状态下攻击
        AttackMoveAB,//两点间进攻
        AttackTowords//冲上去打
    }

    public class DecisiveBotLogic : BotLogic
    {
        public System.Action<IHittableCharacter> OnSpotEnemy;

        public DecisiveState decisiveState = DecisiveState.Idle;

        private Transform m_moveToTrans;

        private Transform m_defenceTrans;

        private IHittableCharacter m_attackTarget;

        public override void OnInitialized(LogicData logicData)
        {
            base.OnInitialized(logicData);
        }

        public void MoveTo(Transform moveToTrans)
        {
            m_moveToTrans = moveToTrans;

            m_logicData.navMeshAgent.SetDestination(m_moveToTrans.position);

            decisiveState = DecisiveState.MoveTo;
        }

        public void Defence(Transform defenceTrans)
        {
            m_defenceTrans = defenceTrans;

            m_logicData.navMeshAgent.SetDestination(m_defenceTrans.position);

            decisiveState = DecisiveState.Defence;
        }

        public void AttackStill(IHittableCharacter hittableCharacter)
        {
            m_attackTarget = hittableCharacter;

            decisiveState = DecisiveState.AttackStill;

            m_logicData.animationController.isAimingTarget = true;
            m_logicData.animationController.target = hittableCharacter.GetAimTransform();
        }

        public override void OnUpdated()
        {
            switch (decisiveState)
            {
                case DecisiveState.Idle:
                    break;
                case DecisiveState.MoveTo:
                    break;
                case DecisiveState.Defence:
                    break;
                case DecisiveState.PatrolAB:
                    break;
                case DecisiveState.AttackStill:
                    break;
                case DecisiveState.AttackMoveAB:
                    break;
                case DecisiveState.AttackTowords:
                    break;
                default:
                    break;
            }
        }

        public void ObserveEnemies(List<IHittableCharacter> enemies)
        {

            foreach (var enemy in enemies)
            {
                var origin = m_logicData.simpleBot.GetSpotPoint();
                var dir = enemy.GetSpotPoint() - origin;

                var isHit = Physics.Raycast(origin, dir, dir.magnitude, 1 << LayerMask.NameToLayer("Building"));


                if (!isHit)
                {
                    Debug.DrawRay(origin, dir, Color.blue);

                    OnSpotEnemy?.Invoke(enemy);
                    break;
                }
            }
        }
    }
}
