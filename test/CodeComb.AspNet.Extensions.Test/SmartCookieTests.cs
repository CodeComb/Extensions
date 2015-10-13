using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Internal;
using Microsoft.Framework.Primitives;
using Xunit;
using Moq;

namespace CodeComb.AspNet.Extensions.Test
{
    public class SmartCookieTests
    {
        [Fact]
        public void reading_test()
        {
            // Arrange
            var req = new Mock<HttpRequest>();
            req.Setup(x => x.Cookies)
                .Returns(new ReadableStringCollection(new Dictionary<string, StringValues> { { "test", "Hello world" } }));
            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(x => x.Request)
                .Returns(req.Object);
            var accessor = new Mock<IHttpContextAccessor>();
            accessor.Setup(x => x.HttpContext)
                .Returns(httpContext.Object);

            // Act
            var cookie = new SmartCookies.SmartCookies(accessor.Object);
            var actual = cookie["test"];

            // Assert
            Assert.Equal("Hello world", actual);
        }

        [Fact]
        public void reading_null_test()
        {
            // Arrange
            var req = new Mock<HttpRequest>();
            req.Setup(x => x.Cookies)
                .Returns(new ReadableStringCollection(new Dictionary<string, StringValues> { }));
            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(x => x.Request)
                .Returns(req.Object);
            var accessor = new Mock<IHttpContextAccessor>();
            accessor.Setup(x => x.HttpContext)
                .Returns(httpContext.Object);

            // Act
            var cookie = new SmartCookies.SmartCookies(accessor.Object);
            var actual = cookie["test"];

            // Assert
            Assert.Null(actual);
        }

        [Fact]
        public void writing_test()
        {
            // Arrange
            var header = new HeaderDictionary();
            var res = new Mock<HttpResponse>();
            res.Setup(x => x.Cookies)
                .Returns(new ResponseCookies(header));
            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(x => x.Response)
                .Returns(res.Object);
            var accessor = new Mock<IHttpContextAccessor>();
            accessor.Setup(x => x.HttpContext)
                .Returns(httpContext.Object);

            // Act
            var cookie = new SmartCookies.SmartCookies(accessor.Object);
            cookie["test"] = "1";

            // Assert
            Assert.Equal("Set-Cookie", header.Keys.First());
        }
    }
}
