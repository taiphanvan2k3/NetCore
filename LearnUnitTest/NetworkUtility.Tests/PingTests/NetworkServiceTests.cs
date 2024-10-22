using FluentAssertions;
using NetworkUtility.Ping;

namespace NetworkUtility.Tests.PingTests
{
    public class NetworkServiceTests
    {
        // Code convention: MethodName_StateUnderTest_ExpectedBehavior
        [Fact]
        public void NetworkService_CheckIsLocalhost_ReturnsTrue()
        {
            // Arrange: variables, objects, and other resources needed to execute the test
            var ipAddress = "127.0.0.1";

            // Act: the actual code that we are testing
            var isLocalHost = NetworkService.CheckIsLocalhost(ipAddress);

            // Assert: the verification step where we check if the system behaved as expected
            // Sử dụng xUnit (default)
            Assert.True(isLocalHost);
        }

        [Fact]
        public void NetworkService_CheckIsLocalhost_ReturnsFalse()
        {
            // Arrange
            var ipAddress = "123.292.120.55";

            // Act
            var isLocalHost = NetworkService.CheckIsLocalhost(ipAddress);

            // Sử dụng FluentAssertions (library)
            isLocalHost.Should().BeFalse();
        }

        [Fact]
        public void NetworkService_GetLocalhost_ReturnsLocalhost()
        {
            // Arrange
            var expected = "localhost";

            // Act
            var result = NetworkService.GetLocalhost();

            // Assert 
            // -> Cần phải pass 2 điều kiện: NotBeNullOrEmpty và Be(expected) sau
            result.Should().NotBeNullOrEmpty();
            result.Should().ContainAny("localhost", "127.0.0.1");
            result.Should().Contain("localhost", Exactly.Once());
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("192.168.1.1", true)]
        [InlineData("localhost", true)]
        [InlineData("123.292.120.55", false)]
        [InlineData("123.292.120.55.1", false)]
        [InlineData("123.292.120", false)]
        [InlineData("256.256.256.256", false)]
        [InlineData("abc.def.ghi.jkl", false)]
        public void NetworkService_IsValidIpAddress_ReturnsCorrectResult(string ipAddress, bool expected)
        {
            // Act
            var result = NetworkService.IsValidIpAddress(ipAddress);

            // Assert
            result.Should().Be(expected);
        }
    }
}