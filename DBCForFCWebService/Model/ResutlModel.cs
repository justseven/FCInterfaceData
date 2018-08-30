using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace DBCForFCWebService.Model
{
    public class ResutlModel
    {
        public bool IsSuccess { get; set; }
        public int MessageLength { get; set; }
        public string Message { get; set; }
        
        public string Token { get; set; }
        public List<DJ_SFD> Data { get; set; }
    }
}