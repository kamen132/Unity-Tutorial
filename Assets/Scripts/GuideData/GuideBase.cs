using Table;

namespace Guide
{
    public abstract class GuideBase
    {
        public GuideState GuideState;
        public GuideBase(GuideGourp data)
        {
            
        }
        public virtual void StartTutorial()
        {
            
        }

        protected virtual void Dispose()
        {
            
        }

        public void TurnNextGuide()
        {
            Dispose();

        }

        public void PreStartTutorial()
        {
            
        }

        public bool CheckCondition()
        {
            return true;
        }
    }
}