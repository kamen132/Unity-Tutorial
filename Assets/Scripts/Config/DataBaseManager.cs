using System.Collections.Generic;
using Table;

public class DataBaseManager : Singleton<DataBaseManager>
{
    protected override void OnInitialize()
    {
        base.OnInitialize();
        AddConfig();
        InitConfig();
    }

     private List<BaseConfig> Configs=new List<BaseConfig>();
     public void AddConfig()
     {
        Configs.Add(new GuideGourpConfig());
        Configs.Add(new GuideStepConfig());
       }

    public void InitConfig()
    {
         foreach (var data in Configs)
         {
            data.Init();
         }
    }
}
