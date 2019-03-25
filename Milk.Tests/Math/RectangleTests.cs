using Milk.Math;
using Xunit;

namespace Milk.Tests.Math
{
    public sealed class RectangleTests
    {
        [Fact]
        public void Top_ReturnsExpectedResult()
        {
            // Arrange
            Rectangle rectangle = new Rectangle(10, 9, 8, 7);

            // Act
            int top = rectangle.Top;

            // Assert
            Assert.Equal(9, top);
        }

        [Fact]
        public void Bottom_ReturnsExpectedResult()
        {
            // Arrange
            Rectangle rectangle = new Rectangle(10, 9, 8, 7);

            // Act
            int bottom = rectangle.Bottom;

            // Assert
            Assert.Equal(16, bottom);
        }

        [Fact]
        public void Left_ReturnsExpectedResult()
        {
            // Arrange
            Rectangle rectangle = new Rectangle(10, 9, 8, 7);

            // Act
            int left = rectangle.Left;

            // Assert
            Assert.Equal(10, left);
        }

        [Fact]
        public void Right_ReturnsExpectedResult()
        {
            // Arrange
            Rectangle rectangle = new Rectangle(10, 9, 8, 7);

            // Act
            int right = rectangle.Right;

            // Assert
            Assert.Equal(18, right);
        }

        [Theory]
        [InlineData(0, 0, 0, 0, true)]
        [InlineData(20, 0, 20, 0, false)]
        public void IsEmpty_ReturnsExpectedResult(int x, int y, int width, int height, bool expectedResult)
        {
            // Arrange
            Rectangle rectangle = new Rectangle(x, y, width, height);

            // Act
            bool empty = rectangle.IsEmpty;

            // Assert
            Assert.Equal(expectedResult, empty);
        }

        [Theory]
        [InlineData(0, 0, 20, 20, true)]
        [InlineData(20, 20, 20, 20, false)]
        public void Overlaps_ReturnsExpectedResult(int x, int y, int width, int height, bool expectedResult)
        {
            // Arrange
            Rectangle rectangle = new Rectangle(10, 10, 10, 10);
            Rectangle otherRectangle = new Rectangle(x, y, width, height);

            // Act
            bool overlaps = rectangle.Overlaps(otherRectangle);

            // Assert
            Assert.Equal(expectedResult, overlaps);
        }
    }
}
