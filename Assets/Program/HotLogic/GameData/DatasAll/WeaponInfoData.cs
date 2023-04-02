using System;
using System.Collections.Generic;
using Platform;
using UnityEngine;

namespace HotLogic
{
    public class WeaponInfoDataAll : BaseGameDataAll
    {
        public Dictionary<int, WeaponInfoData> Data;
        public static string DataPath = "WeaponInfo.txt";
        
        public override void Init()
        {
	        Data = new Dictionary<int, WeaponInfoData>();
	        ABResources.LoadResAsync<TextAsset>(DataPath, (a, request) =>
	        {
		        this.LoadData(a.text);
		        request.Release();
	        }, ABResources.MatchMode.TextAsset);
        }

        private void LoadData(string dataStr)
		{
	        string[] datas = dataStr.Split('\r');
	        for (int i = 5; i < datas.Length; i++)
	        {//数据从第五行开始
		        try
		        {
			        var item = new WeaponInfoData(datas[i]);
			        Data[item.Id] = item;
		        }
		        catch (Exception e)
		        {
			        GameDebug.Log("WeaponInfoData 初始化错误 at line:" + i);
		        }
	        }
		}
          
    }
    public class WeaponInfoData
    {
		/// <summary>		/// 唯一标识		/// </summary>		public int Id;		/// <summary>		/// 武器名称		/// </summary>		public string Name;		/// <summary>		/// 武器攻击力		/// </summary>		public float Attack;		/// <summary>		/// 武器防御力		/// </summary>		public float Defense;

		public WeaponInfoData(string data)
		{
			var split = data.Split('\t');
			Id = int.Parse(split[0]);			Name = split[1];			Attack = float.Parse(split[2]);			Defense = float.Parse(split[3]);
		}
    }
}