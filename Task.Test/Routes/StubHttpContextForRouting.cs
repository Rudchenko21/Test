﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Task.Test.Routes
{
    public class StubHttpContextForRouting : HttpContextBase // todo make sense move all stubs to specific folder Stubs
    {
        StubHttpRequestForRouting _request;
        StubHttpResponseForRouting _response;

        public StubHttpContextForRouting(string appPath = "/", string requestUrl = "~/")
        {
            _request = new StubHttpRequestForRouting(appPath, requestUrl);
            _response = new StubHttpResponseForRouting();
        }

        public override HttpRequestBase Request
        {
            get { return _request; }
        }
        public override HttpResponseBase Response
        {
            get { return _response; }
        }
    }

    public class StubHttpRequestForRouting : HttpRequestBase // todo one class per one file
    {
        string _appPath;
        string _requestUrl; public StubHttpRequestForRouting(string appPath, string requestUrl)
        {
            _appPath = appPath;
            _requestUrl = requestUrl;
        }

        public override string ApplicationPath
        {
            get { return _appPath; }
        }

        public override string AppRelativeCurrentExecutionFilePath
        {
            get { return _requestUrl; }
        }

        public override string PathInfo
        {
            get { return ""; }
        }

        public override NameValueCollection ServerVariables
        {
            get { return new NameValueCollection(); }
        }
    }

    public class StubHttpResponseForRouting : HttpResponseBase
    {
        public override string ApplyAppPathModifier(string virtualPath)
        {
            return virtualPath;
        }
    }
}