﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace XZFCPlug.ServiceReference1 {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference1.GetConfigSoap")]
    public interface GetConfigSoap {
        
        // CODEGEN: 命名空间 http://tempuri.org/ 的元素名称 HelloWorldResult 以后生成的消息协定未标记为 nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/HelloWorld", ReplyAction="*")]
        XZFCPlug.ServiceReference1.HelloWorldResponse HelloWorld(XZFCPlug.ServiceReference1.HelloWorldRequest request);
        
        // CODEGEN: 命名空间 http://tempuri.org/ 的元素名称 paramaters 以后生成的消息协定未标记为 nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetBDCConfig", ReplyAction="*")]
        XZFCPlug.ServiceReference1.GetBDCConfigResponse GetBDCConfig(XZFCPlug.ServiceReference1.GetBDCConfigRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class HelloWorldRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="HelloWorld", Namespace="http://tempuri.org/", Order=0)]
        public XZFCPlug.ServiceReference1.HelloWorldRequestBody Body;
        
        public HelloWorldRequest() {
        }
        
        public HelloWorldRequest(XZFCPlug.ServiceReference1.HelloWorldRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute()]
    public partial class HelloWorldRequestBody {
        
        public HelloWorldRequestBody() {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class HelloWorldResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="HelloWorldResponse", Namespace="http://tempuri.org/", Order=0)]
        public XZFCPlug.ServiceReference1.HelloWorldResponseBody Body;
        
        public HelloWorldResponse() {
        }
        
        public HelloWorldResponse(XZFCPlug.ServiceReference1.HelloWorldResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class HelloWorldResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string HelloWorldResult;
        
        public HelloWorldResponseBody() {
        }
        
        public HelloWorldResponseBody(string HelloWorldResult) {
            this.HelloWorldResult = HelloWorldResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetBDCConfigRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetBDCConfig", Namespace="http://tempuri.org/", Order=0)]
        public XZFCPlug.ServiceReference1.GetBDCConfigRequestBody Body;
        
        public GetBDCConfigRequest() {
        }
        
        public GetBDCConfigRequest(XZFCPlug.ServiceReference1.GetBDCConfigRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class GetBDCConfigRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string paramaters;
        
        public GetBDCConfigRequestBody() {
        }
        
        public GetBDCConfigRequestBody(string paramaters) {
            this.paramaters = paramaters;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetBDCConfigResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetBDCConfigResponse", Namespace="http://tempuri.org/", Order=0)]
        public XZFCPlug.ServiceReference1.GetBDCConfigResponseBody Body;
        
        public GetBDCConfigResponse() {
        }
        
        public GetBDCConfigResponse(XZFCPlug.ServiceReference1.GetBDCConfigResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class GetBDCConfigResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string GetBDCConfigResult;
        
        public GetBDCConfigResponseBody() {
        }
        
        public GetBDCConfigResponseBody(string GetBDCConfigResult) {
            this.GetBDCConfigResult = GetBDCConfigResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface GetConfigSoapChannel : XZFCPlug.ServiceReference1.GetConfigSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GetConfigSoapClient : System.ServiceModel.ClientBase<XZFCPlug.ServiceReference1.GetConfigSoap>, XZFCPlug.ServiceReference1.GetConfigSoap {
        
        public GetConfigSoapClient() {
        }
        
        public GetConfigSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public GetConfigSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public GetConfigSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public GetConfigSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        XZFCPlug.ServiceReference1.HelloWorldResponse XZFCPlug.ServiceReference1.GetConfigSoap.HelloWorld(XZFCPlug.ServiceReference1.HelloWorldRequest request) {
            return base.Channel.HelloWorld(request);
        }
        
        public string HelloWorld() {
            XZFCPlug.ServiceReference1.HelloWorldRequest inValue = new XZFCPlug.ServiceReference1.HelloWorldRequest();
            inValue.Body = new XZFCPlug.ServiceReference1.HelloWorldRequestBody();
            XZFCPlug.ServiceReference1.HelloWorldResponse retVal = ((XZFCPlug.ServiceReference1.GetConfigSoap)(this)).HelloWorld(inValue);
            return retVal.Body.HelloWorldResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        XZFCPlug.ServiceReference1.GetBDCConfigResponse XZFCPlug.ServiceReference1.GetConfigSoap.GetBDCConfig(XZFCPlug.ServiceReference1.GetBDCConfigRequest request) {
            return base.Channel.GetBDCConfig(request);
        }
        
        public string GetBDCConfig(string paramaters) {
            XZFCPlug.ServiceReference1.GetBDCConfigRequest inValue = new XZFCPlug.ServiceReference1.GetBDCConfigRequest();
            inValue.Body = new XZFCPlug.ServiceReference1.GetBDCConfigRequestBody();
            inValue.Body.paramaters = paramaters;
            XZFCPlug.ServiceReference1.GetBDCConfigResponse retVal = ((XZFCPlug.ServiceReference1.GetConfigSoap)(this)).GetBDCConfig(inValue);
            return retVal.Body.GetBDCConfigResult;
        }
    }
}
