﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace SevenTest.SOAP {
    using System.Data;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="SOAP.Bdc2Fc_CLFSoap")]
    public interface Bdc2Fc_CLFSoap {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/HelloWorld", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string HelloWorld();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/FC_CLF_ZTXX", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataSet FC_CLF_ZTXX(string syqr, string qzbh);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/FC_CLF_YZXX", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataSet FC_CLF_YZXX(string ywzh);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/FC_CLF_FZXX", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataSet FC_CLF_FZXX(string ywzh);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface Bdc2Fc_CLFSoapChannel : SevenTest.SOAP.Bdc2Fc_CLFSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class Bdc2Fc_CLFSoapClient : System.ServiceModel.ClientBase<SevenTest.SOAP.Bdc2Fc_CLFSoap>, SevenTest.SOAP.Bdc2Fc_CLFSoap {
        
        public Bdc2Fc_CLFSoapClient() {
        }
        
        public Bdc2Fc_CLFSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public Bdc2Fc_CLFSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public Bdc2Fc_CLFSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public Bdc2Fc_CLFSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string HelloWorld() {
            return base.Channel.HelloWorld();
        }
        
        public System.Data.DataSet FC_CLF_ZTXX(string syqr, string qzbh) {
            return base.Channel.FC_CLF_ZTXX(syqr, qzbh);
        }
        
        public System.Data.DataSet FC_CLF_YZXX(string ywzh) {
            return base.Channel.FC_CLF_YZXX(ywzh);
        }
        
        public System.Data.DataSet FC_CLF_FZXX(string ywzh) {
            return base.Channel.FC_CLF_FZXX(ywzh);
        }
    }
}
