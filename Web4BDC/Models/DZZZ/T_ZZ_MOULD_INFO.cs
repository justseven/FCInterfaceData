//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Web4BDC.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class T_ZZ_MOULD_INFO
    {
        public string ID { get; set; }
        public string LIST_ID { get; set; }
        public string DEPT_ID { get; set; }
        public byte[] MAP_STYLE { get; set; }
        public string MAP_STYLE_FILE_NAME { get; set; }
        public byte[] SIGNATURE { get; set; }
        public string SIGNATURE_FILE_NAME { get; set; }
        public string USER_ID { get; set; }
        public string TYPE_NO { get; set; }
        public string DATA_STATE { get; set; }
        public string STATE { get; set; }
        public Nullable<System.DateTime> CREATE_DATE { get; set; }
    
        public virtual T_ZZ_LIST_INFO T_ZZ_LIST_INFO { get; set; }
    }
}
