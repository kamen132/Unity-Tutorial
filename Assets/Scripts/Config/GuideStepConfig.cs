using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using UnityEngine;

namespace Table
{
    public class GuideStepConfig:BaseConfig
    {
        private static List<GuideStep> sampleGuide;

        public static List<GuideStep> GetAllData()
        {
            return sampleGuide;
        }

        public override void Init()
        {
            if (sampleGuide==null)
            {
                 sampleGuide = GuideStep.LoadBytes();
            }
         }

         public static GuideStep Get(int id)
         {
              foreach (var data in sampleGuide)
              {
                   if (data.id==id)
                   {
                       return data;
                    }
                }
                return null;
          }
    }


    [Serializable]
    public class GuideStep
    {
        /// <summary>
        /// 单步引导
        /// </summary>
        [XmlAttribute("id")]
        public int id;

        /// <summary>
        /// 引导描述
        /// </summary>
        [XmlAttribute("desc")]
        public string desc;

        /// <summary>
        /// 操作类型
        /// </summary>
        [XmlAttribute("GuideType")]
        public int GuideType;

        /// <summary>
        /// 操作类型参数
        /// </summary>
        [XmlAttribute("param1")]
        public string param1;

        /// <summary>
        /// 操作类型参数2
        /// </summary>
        [XmlAttribute("param2")]
        public string param2;

        /// <summary>
        /// 点击响应
        /// </summary>
        [XmlAttribute("clickType")]
        public int clickType;

        /// <summary>
        /// 如果要求点按钮时填写
        /// </summary>
        [XmlAttribute("clickBtnName")]
        public string clickBtnName;

        /// <summary>
        /// 箭头方向
        /// </summary>
        [XmlAttribute("arrowDirection")]
        public int arrowDirection;

        /// <summary>
        /// 跳过类型
        /// </summary>
        [XmlAttribute("skipType")]
        public int skipType;

        /// <summary>
        /// 跳过参数
        /// </summary>
        [XmlAttribute("skipPara")]
        public string skipPara;

        /// <summary>
        /// 是否上传服务器
        /// </summary>
        [XmlAttribute("sendServer")]
        public int sendServer;

        public static List<GuideStep> LoadBytes()
        {
            string bytesPath = Application.dataPath +"/GameAssets/DataTable/GuideStep.bytes";
            if (!File.Exists(bytesPath))
                return null;
            using (FileStream stream = new FileStream(bytesPath, FileMode.Open))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                allGuideStep table = binaryFormatter.Deserialize(stream) as allGuideStep;
                return table.GuideSteps;
            }
        }
    }

    [Serializable]
    public class allGuideStep
    {
        public List<GuideStep> GuideSteps;
    }
}
