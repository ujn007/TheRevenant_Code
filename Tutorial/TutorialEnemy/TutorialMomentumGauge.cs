using YTH.Enemies;

namespace KHJ.Tutorial
{
    public class TutorialMomentumGauge : EnemyMomentumGauge
    {
        private bool _isEx; public bool IsEx { get { return _isEx;} set { _isEx = value; } }

        public override void IncreaseMomentumGauge(float value)
        {
            if (!_isEx) return;
            base.IncreaseMomentumGauge(value);
        }
    }
}
