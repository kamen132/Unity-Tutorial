using Guide;
using UnityEngine;

namespace GuideBehavior
{
    public abstract class BaseGuideBehavior : IGuideBehavior
    {
        //创建引导所需遮罩 当然也可以选择不创建 不如弱引导
        protected bool CreateMask()
        {
            return true;
        }

        //遮罩销毁
        protected void DestroyMask()
        {

        }

        public virtual void StartGuide(GuideData guideData, object data)
        {

        }

        public virtual void EndGuide()
        {

        }

        //刷新接口
        public virtual void Refresh(object data)
        {

        }
    }
}