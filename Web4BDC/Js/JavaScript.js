function OutSideCheck() {
    try {
        var slbh = $("#iptPrjId").val();
        var url = 'http://10.3.4.88/FCInterfaces/PushData2WB?slbh=' + slbh + "&t" + new Date().getTime();
        var res = XmlHttpHelper.transmit(false, "get", "text", url, null, null);
        var retData ={};
        if (typeof(JSON) == 'undefined'){    
            retData = eval("("+res+")");    
        }else{    
            retData = JSON.parse(res);    
        }    
        if (retData.IsSuccess) {
            if (retData.Message != '1' &&retData.Message != '-1' && retData.Message != '对应的流程不需要推送') {
                alert("数据推送失败：" + retData.Message);
                return false;
            } else {
                //alert("数据推送成功！");
                return true;
            }
        } else {
            alert(retData.Message);
            return false;
        }
    }
    catch (e) {
        alert(e.message); 
        return false;
    }
}	



    window.open('http://10.3.4.88/FCWebService/WBInfoShow?SLBH=' + $("#iptPrjId").val(), 'newwindow', 'height=100,width=400,top=0,left=0,toolbar=no,menubar=no,scrollbars=no,resizable=no,location=no,status=no');
}