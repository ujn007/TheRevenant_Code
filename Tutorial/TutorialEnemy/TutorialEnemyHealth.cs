using Main.Runtime.Combat.Core;
using YTH;

namespace KHJ
{
    public class TutorialEnemyHealth : EnemyHealth
    {
        private bool _isCanDeath; public bool IsCanDeath { get { return _isCanDeath; } set { _isCanDeath = value; } }

        public override bool ApplyDamage(GetDamagedInfo getDamagedInfo)
        {
            if(!_isCanDeath) getDamagedInfo.damage = 0;
            return base.ApplyDamage(getDamagedInfo);
        }

        public override bool ApplyOnlyDamage(float damage)
        {
            if (!_isCanDeath) damage = 0;
            return base.ApplyOnlyDamage(damage);
        }
    }
}