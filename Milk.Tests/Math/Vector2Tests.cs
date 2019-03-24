using Milk.Math;
using Xunit;

namespace Milk.Tests.Math
{
    public sealed class Vector2Tests
    {
        [Theory]
        [InlineData(1f, 1f, 1.41421354f)]
        [InlineData(2f, 6f, 6.3245554f)]
        [InlineData(20f, 1f, 20.0249844f)]
        public void Length_ReturnsExpectedResult(float x, float y, float expectedLength)
        {
            // Arrange
            Vector2 v = new Vector2(x, y);

            // Act
            float length = v.Length();

            // Assert
            Assert.Equal(expectedLength, length);
        }

        [Fact]
        public void Normalized_ReturnsExpectedResult()
        {
            // Arrange
            Vector2 v = new Vector2(1f, 1f);

            // Act
            var normalized = v.Normalized();

            // Assert
            Assert.Equal(new Vector2(0.707106769f), normalized);
        }

        [Theory]
        [InlineData(10f, 10f, true)]
        [InlineData(10f, 5f, false)]
        public void Equality_ReturnsExpectedResult(float value1, float value2, bool equal)
        {
            // Arrange
            Vector2 v1 = new Vector2(value1);
            Vector2 v2 = new Vector2(value2);

            // Act
            bool result = v1 == v2;

            // Assert
            Assert.Equal(equal, result);
        }

        [Fact]
        public void Addition_GivenVector_ReturnsExpectedResult()
        {
            // Arrange
            Vector2 v1 = new Vector2(10, 15);
            Vector2 v2 = new Vector2(1, 12);

            // Act
            var result = v1 + v2;

            // Assert
            Assert.Equal(new Vector2(11, 27), result);
        }

        [Fact]
        public void Addition_GivenScalar_ReturnsExpectedResult()
        {
            // Arrange
            Vector2 v1 = new Vector2(10, 15);
            float scalar = 30;

            // Act
            var result = v1 + scalar;

            // Assert
            Assert.Equal(new Vector2(40, 45), result);
        }

        [Fact]
        public void Subtraction_GivenVector_ReturnsExpectedResult()
        {
            // Arrange
            Vector2 v1 = new Vector2(10, 15);
            Vector2 v2 = new Vector2(1, 12);

            // Act
            var result = v1 - v2;

            // Assert
            Assert.Equal(new Vector2(9, 3), result);
        }

        [Fact]
        public void Subtraction_GivenScalar_ReturnsExpectedResult()
        {
            // Arrange
            Vector2 v1 = new Vector2(10, 15);
            float scalar = 30;

            // Act
            var result = v1 - scalar;

            // Assert
            Assert.Equal(new Vector2(-20, -15), result);
        }

        [Fact]
        public void Multiplication_GivenVector_ReturnsExpectedResult()
        {
            // Arrange
            Vector2 v1 = new Vector2(10, 5);
            Vector2 v2 = new Vector2(1, 5);

            // Act
            var result = v1 * v2;

            // Assert
            Assert.Equal(new Vector2(10, 25), result);
        }

        [Fact]
        public void Multiplication_GivenScalar_ReturnsExpectedResult()
        {
            // Arrange
            Vector2 v1 = new Vector2(10, 15);
            float scalar = -2;

            // Act
            var result = v1 * scalar;

            // Assert
            Assert.Equal(new Vector2(-20, -30), result);
        }

        [Fact]
        public void Division_GivenVector_ReturnsExpectedResult()
        {
            // Arrange
            Vector2 v1 = new Vector2(10, 5);
            Vector2 v2 = new Vector2(1, 5);

            // Act
            var result = v1 / v2;

            // Assert
            Assert.Equal(new Vector2(10, 1), result);
        }

        [Fact]
        public void Division_GivenScalar_ReturnsExpectedResult()
        {
            // Arrange
            Vector2 v1 = new Vector2(10, 40);
            float scalar = -2;

            // Act
            var result = v1 / scalar;

            // Assert
            Assert.Equal(new Vector2(-5, -20), result);
        }
    }
}
