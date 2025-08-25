using Opsive.BehaviorDesigner.Runtime;
using System.Collections.Generic;
using UnityEngine;
using YTH.Enemies;

namespace KHJ.Tutorial
{
    public class TutorialEnemyGroup : MonoBehaviour
    {
        public Dictionary<int, BaseEnemy> enemyDic = new();

        private void Awake()
        {
            BaseEnemy[] enemies = GetComponentsInChildren<BaseEnemy>();
            int index = 0;

            foreach (var enemy in enemies)
            {
                enemyDic.Add(index, enemy);
                index++;
            }
        }

        public void EnableEnemyBT(int index, bool isEn)
        {
            if (enemyDic[index].TryGetComponent(out BehaviorTree enemyBt))
            {
                enemyBt.SetVariableValue("CanAttack", isEn, Opsive.GraphDesigner.Runtime.Variables.SharedVariable.SharingScope.GameObject);
            }
        }

        public void EnableEnemyBT(BaseEnemy enemy, bool isEn)
        {
            if (enemy.TryGetComponent(out BehaviorTree enemyBt))
            {
                enemyBt.SetVariableValue("CanAttack", isEn, Opsive.GraphDesigner.Runtime.Variables.SharedVariable.SharingScope.GameObject);
            }
        }

        public BaseEnemy GetEnemy(int index) => enemyDic[index];
    }
}
