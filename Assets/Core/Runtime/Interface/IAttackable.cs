using UnityEngine;

namespace MC.Core.Interface
{
    //可放置物体
    public interface IAttackable
    {
        void Attack(Player attacker);

        //物理伤害值
        float GetHPDamage();

        //耐久度
        float GetEndurance();

        //使用耐久度
        void UseEndurance(float usedEndurance);

        bool IsUseable();

    }
}
