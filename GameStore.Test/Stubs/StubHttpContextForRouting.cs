using System.Web;

namespace GameStore.Test.Stubs
{
    public class StubHttpContextForRouting : HttpContextBase
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

}
