using System;
using System.Collections.Generic;

using System.Text;
using System.Configuration;

namespace Config
{
    public class DBConfig
    {
        public static string CmsSqlConString
        {

            get
            {
                string tstr = ConfigurationManager.ConnectionStrings["SqlDBString"].ConnectionString;
                return tstr;
            }
        }
        public static string CmsOracleConString
        {

            get
            {
                string tstr = ConfigurationManager.ConnectionStrings["OracleDBString"].ConnectionString;
                return tstr;
            }
        }

        public static string CmsAccessConString
        {

            get
            {
                
                string tstr =ConfigurationManager.ConnectionStrings["Access"].ConnectionString;
               
                return tstr;

            }
        }
       
    }
}
