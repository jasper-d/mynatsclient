using FluentAssertions;
using MyNatsClient.Internals;
using Xunit;

namespace UnitTests
{
    public class NatsServerInfoParseTests : UnitTests
    {
        [Fact]
        public void Should_be_able_to_parse_server_info_with_ipv6_addresses()
        {
            var serverInfo = "{\"server_id\":\"vvm7f5eDAgSJNSeLeDcF5Y\",\"version\":\"1.4.0\",\"proto\":1,\"go\":\"go1.11.4\",\"host\":\"0.0.0.0\",\"port\":4222,\"auth_required\":true,\"max_payload\":30000000,\"client_id\":17,\"connect_urls\":[\"42.47.11.11:4222\",\"[2001:0:47ef:15de:421:2eef:f500:fcf4]:4222\",\"42.47.11.22:4222\",\"[2001:0:47ef:15de:74:f83f:d78c:d4e3]:4222\",\"42.47.11.21:4222\",\"[2001:0:47ef:15de:30fa:f43f:d78c:d4e3]:4222\"]}";

            NatsServerInfo.Parse(serverInfo);
        }
    
        [Fact]
        public void Should_be_able_to_parse_server_info_When_protocol_0_data_is_returned()
        {
            var parsed = NatsServerInfo.Parse("{\"server_id\":\"Vwp6WDR1NIEuFr0CQ9PtMa\",\"version\":\"0.8.0\",\"go\":\"go1.6.2\",\"host\":\"0.0.0.0\",\"port\":4222,\"auth_required\":false,\"ssl_required\":false,\"tls_required\":false,\"tls_verify\":false,\"max_payload\":1048576}");

            parsed.ServerId.Should().Be("Vwp6WDR1NIEuFr0CQ9PtMa");
            parsed.Version.Should().Be("0.8.0");
            parsed.Go.Should().Be("go1.6.2");
            parsed.Host.Should().Be("0.0.0.0");
            parsed.Port.Should().Be(4222);
            parsed.AuthRequired.Should().BeFalse();
            parsed.SslRequired.Should().BeFalse();
            parsed.TlsRequired.Should().BeFalse();
            parsed.TlsVerify.Should().BeFalse();
            parsed.MaxPayload.Should().Be(1048576);

            parsed = NatsServerInfo.Parse("{\"auth_required\":true,\"ssl_required\":true,\"tls_required\":true,\"tls_verify\":true}");
            parsed.AuthRequired.Should().BeTrue();
            parsed.SslRequired.Should().BeTrue();
            parsed.TlsRequired.Should().BeTrue();
            parsed.TlsVerify.Should().BeTrue();
        }

        [Fact]
        public void Should_be_able_to_parse_server_info_When_protocol_1_data_is_returned()
        {
            var parsed = NatsServerInfo.Parse("{\"server_id\":\"Vwp6WDR1NIEuFr0CQ9PtMa\",\"version\":\"0.8.0\",\"go\":\"go1.6.2\",\"host\":\"0.0.0.0\",\"port\":4222,\"auth_required\":false,\"ssl_required\":false,\"tls_required\":false,\"tls_verify\":false,\"max_payload\":1048576,\"connect_urls\":[\"ubuntu01:4302\",\"ubuntu01:4303\"]}");

            parsed.ServerId.Should().Be("Vwp6WDR1NIEuFr0CQ9PtMa");
            parsed.Version.Should().Be("0.8.0");
            parsed.Go.Should().Be("go1.6.2");
            parsed.Host.Should().Be("0.0.0.0");
            parsed.Port.Should().Be(4222);
            parsed.AuthRequired.Should().BeFalse();
            parsed.SslRequired.Should().BeFalse();
            parsed.TlsRequired.Should().BeFalse();
            parsed.TlsVerify.Should().BeFalse();
            parsed.MaxPayload.Should().Be(1048576);
            parsed.ConnectUrls.Should().Contain("ubuntu01:4302", "ubuntu01:4303");

            parsed = NatsServerInfo.Parse("{\"auth_required\":true,\"ssl_required\":true,\"tls_required\":true,\"tls_verify\":true}");
            parsed.AuthRequired.Should().BeTrue();
            parsed.SslRequired.Should().BeTrue();
            parsed.TlsRequired.Should().BeTrue();
            parsed.TlsVerify.Should().BeTrue();
        }

        [Fact]
        public void Should_be_able_to_parse_server_info_When_array_is_passed_in_middle()
        {
            var parsed = NatsServerInfo.Parse("{\"server_id\":\"Vwp6WDR1NIEuFr0CQ9PtMa\",\"connect_urls\":[\"ubuntu01:4302\",\"ubuntu01:4303\"],\"max_payload\":1048576}");

            parsed.ServerId.Should().Be("Vwp6WDR1NIEuFr0CQ9PtMa");
            parsed.ConnectUrls.Should().Contain("ubuntu01:4302", "ubuntu01:4303");
            parsed.MaxPayload.Should().Be(1048576);
        }

        [Fact]
        public void Should_be_able_to_parse_server_info_When_array_has_empty_value()
        {
            var parsed = NatsServerInfo.Parse("{\"server_id\":\"Vwp6WDR1NIEuFr0CQ9PtMa\",\"connect_urls\":[\"ubuntu01:4302\",,\"ubuntu01:4303\"]}");

            parsed.ServerId.Should().Be("Vwp6WDR1NIEuFr0CQ9PtMa");
            parsed.ConnectUrls.Should().Contain("ubuntu01:4302", "ubuntu01:4303");
        }

        [Fact]
        public void Should_be_able_to_parse_server_info_When_trailing_comma_arrives()
        {
            var parsed = NatsServerInfo.Parse("{\"server_id\":\"Vwp6WDR1NIEuFr0CQ9PtMa\",\"connect_urls\":[\"ubuntu01:4302\",\"ubuntu01:4303\"],\"max_payload\":1048576,}");
            parsed.ServerId.Should().Be("Vwp6WDR1NIEuFr0CQ9PtMa");
            parsed.ConnectUrls.Should().Contain("ubuntu01:4302", "ubuntu01:4303");
            parsed.MaxPayload.Should().Be(1048576);

            parsed = NatsServerInfo.Parse("{\"server_id\":\"Vwp6WDR1NIEuFr0CQ9PtMa\",\"max_payload\":1048576,\"connect_urls\":[\"ubuntu01:4302\",\"ubuntu01:4303\"],}");
            parsed.ServerId.Should().Be("Vwp6WDR1NIEuFr0CQ9PtMa");
            parsed.MaxPayload.Should().Be(1048576);
            parsed.ConnectUrls.Should().Contain("ubuntu01:4302", "ubuntu01:4303");
        }
    }
}
