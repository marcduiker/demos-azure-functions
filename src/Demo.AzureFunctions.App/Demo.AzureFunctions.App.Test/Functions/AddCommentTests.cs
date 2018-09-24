using Demo.AzureFunctions.App.Functions;
using Demo.AzureFunctions.Models;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Demo.AzureFunctions.App.Test
{
    public class AddCommentTests
    {
        [Fact]
        public void CreateComment_WithJsonNotMatchingCommentType_ShouldReturnTypeOfEmptyComment()
        {
            // Arrange
            var json = @"{""property"":""value""}";
            var logger = A.Fake<ILogger>();

            // Act
            var result = AddComment.CreateComment(json, logger);

            // Assert
            result.Should().BeOfType<EmptyComment>();
        }

        [Fact]
        public void CreateComment_WithJsonMatchingCommentType_ShouldReturnTypeOfComment()
        {
            // Arrange
            var json = @"{
                ""AuthorName""  : ""Grace Hopper"",
                ""Text""        : ""I <3 this post!"",
                ""ParentId""    : ""04A785B9-F4F6-4095-ABCA-30F5F2793BCC""
                }";
            var logger = A.Fake<ILogger>();

            // Act
            var result = AddComment.CreateComment(json, logger);

            // Assert
            result.Should().BeOfType<Comment>();
        }
    }
}
