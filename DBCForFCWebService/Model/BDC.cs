using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DBCForFCWebService.Model
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2556.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class BDC : object, System.ComponentModel.INotifyPropertyChanged
    {

        private Head headField;

        private System.Data.DataSet dataField;

        private System.Data.DataSet attachField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public Head head
        {
            get
            {
                return this.headField;
            }
            set
            {
                this.headField = value;
                this.RaisePropertyChanged("head");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public System.Data.DataSet data
        {
            get
            {
                return this.dataField;
            }
            set
            {
                this.dataField = value;
                this.RaisePropertyChanged("data");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public System.Data.DataSet attach
        {
            get
            {
                return this.attachField;
            }
            set
            {
                this.attachField = value;
                this.RaisePropertyChanged("attach");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2556.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class Head : object, System.ComponentModel.INotifyPropertyChanged
    {

        private int flagField;

        private string msgField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public int flag
        {
            get
            {
                return this.flagField;
            }
            set
            {
                this.flagField = value;
                this.RaisePropertyChanged("flag");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string msg
        {
            get
            {
                return this.msgField;
            }
            set
            {
                this.msgField = value;
                this.RaisePropertyChanged("msg");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
}