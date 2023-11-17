using Base64EncoderApi.Models;
using Microsoft.AspNetCore.Mvc;


namespace Base64EncoderApi.Tests
{
    public class EncodingControllerTests
    {
        [Fact]
        public void StartEncoding_WithValidInput_ReturnsRequestId()
        {
            // Arrange
            var controller = new EncodingController();
            var request = new EncodingRequest { Input = "test" };

            // Act
            var result = controller.StartEncoding(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var requestId = Assert.IsType<string>(okResult.Value);
            Assert.NotNull(requestId);
        }

        [Fact]
        public void StartEncoding_WithInvalidInput_ReturnsBadRequest()
        {
            // Arrange
            var controller = new EncodingController();
            var request = new EncodingRequest { Input = string.Empty };

            // Act
            var result = controller.StartEncoding(request);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void GetEncodedCharacter_WithValidRequestId_ReturnsCharacter()
        {
            // Arrange
            var controller = new EncodingController();
            var startResult = controller.StartEncoding(new EncodingRequest { Input = "test" }) as OkObjectResult;
            var requestId = startResult.Value.ToString();

            // Act
            var result = await controller.GetEncodedCharacter(requestId, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var character = Assert.IsType<string>(okResult.Value);
            Assert.Equal("d", character); // 'd' is the first character of the base64 encoded "test"
        }

        [Fact]
        public async void GetEncodedCharacter_WithInvalidRequestId_ReturnsNotFound()
        {
            // Arrange
            var controller = new EncodingController();
            var invalidRequestId = "nonexistent";

            // Act
            var result = await controller.GetEncodedCharacter(invalidRequestId, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void CancelEncoding_WithValidRequestId_ReturnsOk()
        {
            // Arrange
            var controller = new EncodingController();
            var startResult = controller.StartEncoding(new EncodingRequest { Input = "test" }) as OkObjectResult;
            var requestId = startResult.Value.ToString();

            // Act
            var result = controller.CancelEncoding(requestId);

            // Assert
            Assert.IsType<OkResult>(result);
        }
    }
}
