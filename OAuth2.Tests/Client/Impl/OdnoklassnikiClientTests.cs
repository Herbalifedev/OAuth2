using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using HL.OAuth2.Client;
using HL.OAuth2.Client.Impl;
using HL.OAuth2.Configuration;
using HL.OAuth2.Infrastructure;
using HL.OAuth2.Models;

namespace HL.OAuth2.Tests.Client.Impl
{
    [TestFixture]
    public class OdnoklassnikiClientTests
    {
        private const string content = "todo";

        private OdnoklassnikiClientDescendant descendant;
        private IRequestFactory factory;

        [SetUp]
        public void SetUp()
        {
            factory = Substitute.For<IRequestFactory>();
            descendant = new OdnoklassnikiClientDescendant(
                factory, Substitute.For<IClientConfiguration>());
        }

        [Test]
        public void Should_ReturnCorrectAccessCodeServiceEndpoint()
        {
            // act
            var endpoint = descendant.GetAccessCodeServiceEndpoint();

            // assert
            endpoint.BaseUri.Should().Be("http://www.odnoklassniki.ru");
            endpoint.Resource.Should().Be("/oauth/authorize");
        }

        [Test]
        public void Should_ReturnCorrectAccessTokenServiceEndpoint()
        {
            // act
            var endpoint = descendant.GetAccessTokenServiceEndpoint();

            // assert
            endpoint.BaseUri.Should().Be("http://api.odnoklassniki.ru");
            endpoint.Resource.Should().Be("/oauth/token.do");
        }

        [Test]
        public void Should_ReturnCorrectUserInfoServiceEndpoint()
        {
            // act
            var endpoint = descendant.GetUserInfoServiceEndpoint();

            // assert
            endpoint.BaseUri.Should().Be("http://api.odnoklassniki.ru");
            endpoint.Resource.Should().Be("/fb.do");
        }

        [Test]
        public void Should_ParseAllFieldsOfUserInfo_WhenCorrectContentIsPassed()
        {
            Assert.Ignore("todo");

            // act
            var info = descendant.ParseUserInfo(content);

            // assert
            info.Id.Should().Be("todo");
        }

        private class OdnoklassnikiClientDescendant : OdnoklassnikiClient
        {
            public OdnoklassnikiClientDescendant(IRequestFactory factory, IClientConfiguration configuration)
                : base(factory, configuration)
            {
            }

            public Endpoint GetAccessCodeServiceEndpoint()
            {
                return AccessCodeServiceEndpoint;
            }

            public Endpoint GetAccessTokenServiceEndpoint()
            {
                return AccessTokenServiceEndpoint;
            }

            public Endpoint GetUserInfoServiceEndpoint()
            {
                return UserInfoServiceEndpoint;
            }

            public new UserInfo ParseUserInfo(string content)
            {
                return base.ParseUserInfo(content);
            }
        } 
    }
}