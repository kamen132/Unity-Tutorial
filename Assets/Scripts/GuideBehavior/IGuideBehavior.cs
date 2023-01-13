using Guide;

namespace GuideBehavior
{
    public interface IGuideBehavior
    {
        //行为开始
        void StartGuide(GuideData guideData, object data);
        //行为结束
        void EndGuide();
    }
}