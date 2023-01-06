using System.IO;
using System.Data;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Excel;

public class ExcelTool
{
  static string ExcelDataPath = Application.dataPath + "/Config";//源Excel文件夹,xlsx格式
    static string BytesDataPath = Application.dataPath + "/GameAssets/DataTable";//生成的bytes文件夹
    static string CsClassPath = Application.dataPath+"/Scripts/Config"  /*Application.dataPath + "/Scripts/DataTable"*/;//生成的c#脚本文件夹
    static string XmlDataPath = ExcelDataPath + "/tempXmlData";//生成的xml(临时)文件夹..
    static string AllCsHead = "all";//序列化结构体的数组类.类名前缀

    static char ArrayTypeSplitChar = '|';//数组类型值拆分符: int[] 1#2#34 string[] 你好#再见 bool[] true#false ...
    static bool IsDeleteXmlInFinish = true;//生成bytes后是否删除中间文件xml
    static  string Cshouzhui = "Config";
    private static List<string> AllClassName=new List<string>();
    [MenuItem("KamenTool/Excel/Excel2Cs")]
    static void Excel2Cs()
    {
        Init();
        Excel2CsOrXml(true);
    }

    [MenuItem("KamenTool/Excel/Excel2Bytes")]
    static void Excel2Xml2Bytes()
    {
        Init();
        //生成中间文件xml
        Excel2CsOrXml(false);
        //生成bytes
        WriteBytes();
    }

    static void Init()
    {
        if (!Directory.Exists(CsClassPath))
        {
            Directory.CreateDirectory(CsClassPath);
        }
        if (!Directory.Exists(XmlDataPath))
        {
            Directory.CreateDirectory(XmlDataPath);
        }
        if (!Directory.Exists(BytesDataPath))
        {
            Directory.CreateDirectory(BytesDataPath);
        }
    }

    static void WriteCs(string className, string[] names, string[] types, string[] descs)
    {
        try
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("using System;");
            stringBuilder.AppendLine("using System.Collections.Generic;");
            stringBuilder.AppendLine("using System.IO;");
            stringBuilder.AppendLine("using System.Runtime.Serialization.Formatters.Binary;");
            stringBuilder.AppendLine("using System.Xml.Serialization;");
            stringBuilder.AppendLine("using UnityEngine;");
            stringBuilder.Append("\n");
            stringBuilder.AppendLine("namespace Table");
            stringBuilder.AppendLine("{");
            stringBuilder.AppendLine("    public class " + className + Cshouzhui + ":BaseConfig");
            stringBuilder.AppendLine("    {");
            stringBuilder.AppendLine("        private static List<" + className + "> sampleGuide;");
            stringBuilder.Append("\n");
            stringBuilder.AppendLine("        public static List<" + className + "> GetAllData()");
            stringBuilder.AppendLine("        {");
            stringBuilder.AppendLine("            return sampleGuide;");
            stringBuilder.AppendLine("        }");
            stringBuilder.Append("\n");
            stringBuilder.AppendLine("        public override void Init()");
            stringBuilder.AppendLine("        {");
            stringBuilder.AppendLine("            if (sampleGuide==null)");
            stringBuilder.AppendLine("            {");
            stringBuilder.AppendLine("                 sampleGuide = " + className + ".LoadBytes();");
            stringBuilder.AppendLine("            }");
            stringBuilder.AppendLine("         }");
            stringBuilder.Append("\n");
            stringBuilder.AppendLine("         public static " + className + " Get(int id)");
            stringBuilder.AppendLine("         {");
            stringBuilder.AppendLine("              foreach (var data in sampleGuide)");
            stringBuilder.AppendLine("              {");
            stringBuilder.AppendLine("                   if (data.id==id)");
            stringBuilder.AppendLine("                   {");
            stringBuilder.AppendLine("                       return data;");
            stringBuilder.AppendLine("                    }");
            stringBuilder.AppendLine("                }");
            stringBuilder.AppendLine("                return null;");
            stringBuilder.AppendLine("          }");
            stringBuilder.AppendLine("    }");
            stringBuilder.Append("\n");
            stringBuilder.Append("\n");
            stringBuilder.AppendLine("    [Serializable]");
            stringBuilder.AppendLine("    public class " + className);
            stringBuilder.AppendLine("    {");
            for (int i = 0; i < names.Length; i++)
            {
                stringBuilder.AppendLine("        /// <summary>");
                stringBuilder.AppendLine("        /// " + descs[i]);
                stringBuilder.AppendLine("        /// </summary>");
                stringBuilder.AppendLine("        [XmlAttribute(\"" + names[i] + "\")]");
                string type = types[i];
                if (type.Contains("[]"))
                {
                    //type = type.Replace("[]", "");
                    //stringBuilder.AppendLine("        public List<" + type + "> " + names[i] + ";");
                    //可选代码：
                    //用_name字段去反序列化，name取_name.item的值,直接返回list<type>。
                    //因为xml每行可能有多个数组字段，这样就多了一层变量item，所以访问的时候需要.item才能取到list<type>
                    //因此用额外的一个变量直接返回List<type>。
                    type = type.Replace("[]", "");
                    stringBuilder.AppendLine("        public List<" + type + "> " + names[i] + "");
                    stringBuilder.AppendLine("        {");
                    stringBuilder.AppendLine("            get");
                    stringBuilder.AppendLine("            {");
                    stringBuilder.AppendLine("                if (_" + names[i] + " != null)");
                    stringBuilder.AppendLine("                {");
                    stringBuilder.AppendLine("                    return _" + names[i] + ".item;");
                    stringBuilder.AppendLine("                }");
                    stringBuilder.AppendLine("                return null;");
                    stringBuilder.AppendLine("            }");
                    stringBuilder.AppendLine("        }");
                    stringBuilder.AppendLine("        [XmlElementAttribute(\"" + names[i] + "\")]");
                    stringBuilder.AppendLine("        public " + type + "Array _" + names[i] + ";");
                }
                else
                {
                    stringBuilder.AppendLine("        public " + type + " " + names[i] + ";");
                }

                stringBuilder.Append("\n");
            }

            stringBuilder.AppendLine("        public static List<" + className + "> LoadBytes()");
            stringBuilder.AppendLine("        {");
            stringBuilder.AppendLine("            string bytesPath = " + "Application.dataPath +" + "\"/GameAssets/DataTable/" + className + ".bytes\";");
            stringBuilder.AppendLine("            if (!File.Exists(bytesPath))");
            stringBuilder.AppendLine("                return null;");
            stringBuilder.AppendLine("            using (FileStream stream = new FileStream(bytesPath, FileMode.Open))");
            stringBuilder.AppendLine("            {");
            stringBuilder.AppendLine("                BinaryFormatter binaryFormatter = new BinaryFormatter();");
            stringBuilder.AppendLine("                all" + className + " table = binaryFormatter.Deserialize(stream) as all" + className + ";");
            stringBuilder.AppendLine("                return table." + className + "s;");
            stringBuilder.AppendLine("            }");
            stringBuilder.AppendLine("        }");
            stringBuilder.AppendLine("    }");
            stringBuilder.Append("\n");
            stringBuilder.AppendLine("    [Serializable]");
            stringBuilder.AppendLine("    public class " + AllCsHead + className);
            stringBuilder.AppendLine("    {");
            stringBuilder.AppendLine("        public List<" + className + "> " + className + "s;");
            stringBuilder.AppendLine("    }");
            stringBuilder.AppendLine("}");




            string csPath = CsClassPath + "/" + className + Cshouzhui + ".cs";
            if (File.Exists(csPath))
            {
                File.Delete(csPath);
            }

            using (StreamWriter sw = new StreamWriter(csPath))
            {
                sw.Write(stringBuilder);
                Debug.Log("生成:" + csPath);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("写入CS失败:" + e.Message);
            throw;
        }
    }


    static void WriteXml(string className, string[] names, string[] types, List<string[]> datasList)
    {
        try
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            stringBuilder.AppendLine("<" + AllCsHead + className + ">");
            stringBuilder.AppendLine("<" + className + "s>");
            for (int d = 0; d < datasList.Count; d++)
            {
                stringBuilder.Append("\t<" + className + " ");
                //单行数据
                string[] datas = datasList[d];
                //填充属性节点
                for (int c = 0; c < datas.Length; c++)
                {
                    string type = types[c];
                    if (!type.Contains("[]"))
                    {
                        string name = names[c];
                        string value = datas[c];
                        stringBuilder.Append(name + "=\"" + value + "\"" + (c == datas.Length - 1 ? "" : " "));
                       
                    }
                }
                stringBuilder.Append(">\n");
                //填充子元素节点(数组类型字段)
                for (int c = 0; c < datas.Length; c++)
                {
                    string type = types[c];
                    if (type.Contains("[]"))
                    {
                        string name = names[c];
                        string value = datas[c];
                        string[] values = value.Split(ArrayTypeSplitChar);
                        stringBuilder.AppendLine("\t\t<" + name + ">");
                        for (int v = 0; v < values.Length; v++)
                        {
                            stringBuilder.AppendLine("\t\t\t<item>" + values[v] + "</item>");
                        }
                        stringBuilder.AppendLine("\t\t</" + name + ">");
                    }
                }
                stringBuilder.AppendLine("\t</" + className + ">");
            }
            stringBuilder.AppendLine("</" + className + "s>");
            stringBuilder.AppendLine("</" + AllCsHead + className + ">");

            string xmlPath = XmlDataPath + "/" + className + ".xml";
            /*if (File.Exists(xmlPath))
            {
                File.Delete(xmlPath);
            }*/
            using (StreamWriter sw = new StreamWriter(xmlPath))
            {
                sw.Write(stringBuilder);
                Debug.Log("生成文件:" + xmlPath);
//                Debug.LogError(stringBuilder);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("写入Xml失败:" + e.Message);
        }
    }

    public static List<string> excelNames = new List<string>();

    static void Excel2CsOrXml(bool isCs)
    {
        excelNames.Clear();
        AllClassName.Clear();
        string[] excelPaths = Directory.GetFiles(ExcelDataPath, "*.xlsx");
        for (int e = 0; e < excelPaths.Length; e++)
        {
            //0.读Excel
            string className; //类型名
            string[] names; //字段名
            string[] types; //字段类型
            string[] descs; //字段描述
            List<string[]> datasList; //数据

            //try
            {
                string excelPath = excelPaths[e]; //excel路径  
                className = Path.GetFileNameWithoutExtension(excelPath).ToLower();
                FileStream fileStream = File.Open(excelPath, FileMode.Open, FileAccess.Read);
                IExcelDataReader excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(fileStream);
                // 表格数据全部读取到result里
                DataSet result = excelDataReader.AsDataSet();
                if (result == null)
                {
                    Debug.LogError(excelDataReader.ExceptionMessage);
                }

                //解析一个excel有多个子表情况
                for (int i = 0; i < result.Tables.Count; i++)
                {
                    //
                    string partExcelName = result.Tables[i].TableName;
                    excelNames.Add(partExcelName);
                    // 获取表格列数
                    int columns = result.Tables[i].Columns.Count;
                    // 获取表格行数
                    int rows = result.Tables[i].Rows.Count;
                    // 根据行列依次读取表格中的每个数据
                    names = new string[columns];
                    types = new string[columns];
                    descs = new string[columns];
                    datasList = new List<string[]>();
                    for (int r = 0; r < rows; r++)
                    {
                        string[] curRowData = new string[columns];
                        for (int c = 0; c < columns; c++)
                        {
                            //解析：获取第一个表格中指定行指定列的数据
                            string value = result.Tables[i].Rows[r][c].ToString();

                            string valueType = result.Tables[i].Rows[1][c].ToString();

                            //清除前两行的变量名、变量类型 首尾空格
                            if (r < 2)
                            {
                                value = value.TrimStart(' ').TrimEnd(' ');
                            }

                            if (value == "" && valueType == "int")
                            {
                                value = "0";
                            }

                            curRowData[c] = value;
                        }

                        //解析：第一行类变量名
                        if (r == 0)
                        {
                            names = curRowData;
                        } //解析：第二行类变量类型
                        else if (r == 1)
                        {
                            types = curRowData;
                        } //解析：第三行类变量描述
                        else if (r == 2)
                        {
                            descs = curRowData;
                        }
                        else if (r == 3)
                        {

                        } //解析：第三行开始是数据
                        else
                        {
                            datasList.Add(curRowData);
                        }
                    }

                    if (isCs)
                    {
                        //写Cs
                        AllClassName.Add(partExcelName+Cshouzhui);
                        WriteCs(partExcelName, names, types, descs);
                    }
                    else
                    {
                        //写Xml
                        WriteXml(partExcelName, names, types, datasList);
                    }
                }

                if (isCs)
                {
                    WriteDataManager();
                }

            }
            // catch (System.Exception exc)
            // {
            //     Debug.LogError("请关闭Excel:" + exc.Message);
            //     return;
            // }


        }

        AssetDatabase.Refresh();
    }

    private static void WriteDataManager()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("using System.Collections.Generic;");
        stringBuilder.AppendLine("using Table;");
        stringBuilder.Append("\n");
        stringBuilder.AppendLine("public class DataBaseManager : Singleton<DataBaseManager>");
        stringBuilder.AppendLine("{");
        stringBuilder.AppendLine("    protected override void OnInitialize()");
        stringBuilder.AppendLine("    {");
        stringBuilder.AppendLine("        base.OnInitialize();");
        stringBuilder.AppendLine("        AddConfig();");
        stringBuilder.AppendLine("        InitConfig();");
        stringBuilder.AppendLine("    }");
        stringBuilder.Append("\n");
        stringBuilder.AppendLine("     private List<BaseConfig> Configs=new List<BaseConfig>();");
        stringBuilder.AppendLine("     public void AddConfig()");
        stringBuilder.AppendLine("     {");
        foreach (var className in AllClassName)
        {
            stringBuilder.AppendLine($"        Configs.Add(new {className}());");
            
        }
        stringBuilder.AppendLine("       }");
        //stringBuilder.AppendLine("}");
        stringBuilder.Append("\n");
        stringBuilder.AppendLine("    public void InitConfig()");
        stringBuilder.AppendLine("    {");
        stringBuilder.AppendLine("         foreach (var data in Configs)");
        stringBuilder.AppendLine("         {");
        stringBuilder.AppendLine("            data.Init();");
        stringBuilder.AppendLine("         }");
        stringBuilder.AppendLine("    }");
        stringBuilder.AppendLine("}");
        
        string csPath = CsClassPath + "/" + "DataBaseManager.cs";
        if (File.Exists(csPath))
        {
            File.Delete(csPath);
        }

        using (StreamWriter sw = new StreamWriter(csPath))
        {
            sw.Write(stringBuilder);
            Debug.Log("生成:" + csPath);
        }
    }

    static void WriteBytes()
    {
        string csAssemblyPath = Application.dataPath + "/../Library/ScriptAssemblies/Assembly-CSharp.dll";
        Assembly assembly = Assembly.LoadFile(csAssemblyPath);
        if (assembly != null)
        {
            Type[] types = assembly.GetTypes();
            for (int i = 0; i < types.Length; i++)
            {
                Type type = types[i];
                if (type.Namespace == "Table" && type.Name.Contains(AllCsHead))
                {
                    string className = type.Name.Replace(AllCsHead, "");
                    if (excelNames.Contains(className))
                    {
                        //读取xml数据
                        string xmlPath = XmlDataPath + "/" + className + ".xml";
                        if (!File.Exists(xmlPath))
                        {
                            Debug.LogError("Xml文件读取失败:" + xmlPath);
                            continue;
                        }
                        object table;
                       // Debug.LogError("xmlPath=="+xmlPath);
                        using (Stream reader = new FileStream(xmlPath, FileMode.Open))
                        {
                            //读取xml实例化table: all+classname
                            //object table = assembly.CreateInstance("Table." + type.Name);
                            XmlSerializer xmlSerializer = new XmlSerializer(type);
                            table = xmlSerializer.Deserialize(reader);
                        }
                        //obj序列化二进制
                        string bytesPath = BytesDataPath + "/" + className + ".bytes";
                        if (File.Exists(bytesPath))
                        {
                            File.Delete(bytesPath);
                        }
                        using (FileStream fileStream = new FileStream(bytesPath, FileMode.Create))
                        {
                            BinaryFormatter binaryFormatter = new BinaryFormatter();
                            binaryFormatter.Serialize(fileStream, table);
                            Debug.Log("生成:" + bytesPath);
                        }

                        if (IsDeleteXmlInFinish)
                        {
                            File.Delete(xmlPath);
                            Debug.Log("删除:" + bytesPath);
                        }
                    }
                   
                }
            }
        }

        if (IsDeleteXmlInFinish)
        {
            Directory.Delete(XmlDataPath,true);
            Debug.Log("删除:" + XmlDataPath);
        }
    }
}
