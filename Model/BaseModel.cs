﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.Model
{
    public class BaseModel
    {
        private string _buscode = Helper.GetAppSettings("BusCode");

        /// <summary>
        ///引用商户表Business的商户编号字段buscode的值
        /// <summary>
        public string buscode
        {
            get { return _buscode; }
            set { _buscode = value; }
        }
    }
}
