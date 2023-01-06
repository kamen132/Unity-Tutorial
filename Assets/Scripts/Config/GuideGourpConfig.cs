using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using UnityEngine;

namespace Table
{
    public class GuideGourpConfig:BaseConfig
    {
        private static List<GuideGourp> sampleGuide;

        public static List<GuideGourp> GetAllData()
        {
            return sampleGuide;
        }

        public override void Init()
        {
            if (sampleGuide==null)
            {
                 sampleGuide = GuideGourp.LoadBytes();
            }
         }

         public static GuideGourp Get(int id)
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
    public class GuideGourp
    {
        /// <summary>
        /// 引导类型
        /// </summary>
        [XmlAttribute("id")]
        public int id;

        /// <summary>
        /// 类型参数
        /// </summary>
        [XmlAttribute("guideParam")]
        public string guideParam;

        /// <summary>
        /// 引导组
        /// </summary>
        [XmlAttribute("guideGroup")]
        public int guideGroup;

        /// <summary>
        /// 触发条件

        /// </summary>
        [XmlAttribute("trigger")]
        public int trigger;

        /// <summary>
        /// 触发参数
        /// </summary>
        [XmlAttribute("triggerPara")]
        public string triggerPara;

        /// <summary>
        /// 是否强制


        /// </summary>
        [XmlAttribute("isForce")]
        public int isForce;

        public static List<GuideGourp> LoadBytes()
        {
            string bytesPath = Application.dataPath +"/GameAssets/DataTable/GuideGourp.bytes";
            if (!File.Exists(bytesPath))
                return null;
            using (FileStream stream = new FileStream(bytesPath, FileMode.Open))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                allGuideGourp table = binaryFormatter.Deserialize(stream) as allGuideGourp;
                return table.GuideGourps;
            }
        }
    }

    [Serializable]
    public class allGuideGourp
    {
        public List<GuideGourp> GuideGourps;
    }
}
