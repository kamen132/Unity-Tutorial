using System.Collections.Generic;
using Table;

namespace Manager
{
    public class GuideConfigManager : Singleton<GuideConfigManager>
    {
        private Dictionary<int, List<GuideStep>> mGuideGroupDict = new Dictionary<int, List<GuideStep>>();

        private void Init()
        {
            foreach (var config in GuideStep.LoadBytes())
            {
                int guideGroupId = config.id / 100;
                AddGroup(guideGroupId, config);
            }
        }

        private void AddGroup(int groupId,GuideStep config)
        {
            List<GuideStep> stepList;
            if (!mGuideGroupDict.TryGetValue(groupId,out stepList))
            {
                stepList=new List<GuideStep>();
                mGuideGroupDict.Add(groupId,stepList);
            }
            stepList.Add(config);
        }
        
        public List<GuideStep> GetGuideGroupList(int groupId)
        {
            List<GuideStep> groupList;
            mGuideGroupDict.TryGetValue(groupId, out groupList);
            return groupList;
        }
    }
}