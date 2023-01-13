using deVoid.UIFramework;

namespace View
{
    public class GuideMaskController:APanelController
    {
        public PanelPriority Priority
        {
            get
            {
                return PanelPriority.Tutorial;
            }
        }
        protected override void HierarchyFixOnShow()
        {
            base.HierarchyFixOnShow();
        }
    }
}