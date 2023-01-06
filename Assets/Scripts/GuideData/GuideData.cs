using System.Collections.Generic;

namespace Guide
{
    /// <summary>
    /// 引导的中介数据
    /// </summary>
    public class GuideData
    {
        public int GuideId;
        public GuideType GuideType;
        public int GuideGroupId;
        public object Param1;
        public object Param2;
        public ArrowDirection ArrowDirection;
        public string ClickBtnName;
        //...各种参数都可以
        
        public static Stack<GuideData> mDataPool=new Stack<GuideData>();

        public static GuideData Get(int GuideGroupId)
        {
            GuideData data= mDataPool.Count > 0 ? mDataPool.Pop() : new GuideData();
            data.GuideGroupId = GuideGroupId;
            return data;
        }

        public static void Recycle(GuideData data)
        {
            if (data!=null)
            {
               data.Reset();
               mDataPool.Push(data);
            }
        }

        public void Reset()
        {
            this.GuideId = 0;
            this.GuideType = GuideType.None;
            this.GuideGroupId = 0;
            this.Param1 = null;
            this.Param2 = null;
            this.ArrowDirection = ArrowDirection.BottomLeft;
            this.ClickBtnName = "";
        }

    }
}