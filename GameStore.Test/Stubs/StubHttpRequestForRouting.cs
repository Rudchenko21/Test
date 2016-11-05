using System.Collections.Specialized;
using System.Web;

namespace GameStore.Test.Stubs
{
    public class StubHttpRequestForRouting : HttpRequestBase
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
}
