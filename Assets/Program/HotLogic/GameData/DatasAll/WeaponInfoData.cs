﻿using System;
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
		/// <summary>

		public WeaponInfoData(string data)
		{
			var split = data.Split('\t');
			Id = int.Parse(split[0]);
		}
    }
}