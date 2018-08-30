using System.Configuration;

namespace OldCGLPB
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
                string tstr = ConfigurationManager.ConnectionStrings["oracleConnection"].ConnectionString;
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

        public static string CmsConString
        {
            get
            {
                string tstr = ConfigurationManager.ConnectionStrings["Other"].ConnectionString;

                return tstr;
            }
        }

        
       
    }
}
