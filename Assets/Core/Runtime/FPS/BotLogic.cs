using UnityEngine;
using UnityEngine.AI;

namespace MC.Core
{
    public class LogicData
    {
        public SimpleBot simpleBot;

        public NavMeshAgent navMeshAgent;

        public TPSAnimationController animationController;
    }

    public abstract class BotLogic : ScriptableObject
    {
        public LogicData m_logicData;

        public virtual void OnInitialized(LogicData logicData)
        {
            m_logicData = logicData;
        }

        public abstract void OnUpdated();
    }

}
