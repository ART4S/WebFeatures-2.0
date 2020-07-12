﻿using Application.Behaviours;
using Application.Interfaces.DataAccess;
using Application.Interfaces.Logging;
using Application.Tests.Common.Stubs.Requests;
using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Application.Tests.Unit.Behaviours
{
    public class SaveChangesBehaviourTests
    {
        [Fact]
        public async Task ShouldSaveChangesAfterNextDelegateCall()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var messages = new List<string>();

            var token = new CancellationToken();

            var context = fixture.Freeze<Mock<IDbContext>>();

            context.Setup(x => x.SaveChangesAsync(token)).Callback(() => messages.Add("save"));

            RequestHandlerDelegate<int> next = () =>
            {
                messages.Add("next");

                return Task.FromResult(0);
            };

            var sut = fixture.Create<SaveChangesBehaviour<CustomCommand<int>, int>>();

            // Act
            await sut.Handle(new CustomCommand<int>(), token, next);

            // Assert
            messages.Should().Equal(new[] { "next", "save" });
        }

        [Fact]
        public async Task ShouldHandleError_WhenSaveChangesThrows()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var context = fixture.Freeze<Mock<IDbContext>>();

            var token = new CancellationToken();

            var exception = fixture.Create<Exception>();

            context.Setup(x => x.SaveChangesAsync(token)).Callback(() => throw exception);

            var logger = fixture.Freeze<Mock<ILogger<CustomCommand<int>>>>();

            var sut = fixture.Create<SaveChangesBehaviour<CustomCommand<int>, int>>();

            // Act
            Func<Task<int>> act = () => sut.Handle(new CustomCommand<int>(), token, () => Task.FromResult(0));

            // Assert
            await act.Should().ThrowExactlyAsync<Exception>();

            logger.Verify(x => x.LogError("Error while saving changes", exception), Times.Once);

        }
    }
}