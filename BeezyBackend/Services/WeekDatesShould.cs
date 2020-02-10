using BeezyBackend.Domain.Services;
using BeezyBackend.Domain.Services.QueryModels;
using FluentAssertions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BeezyBackend.Domain.UnitTest.Services
{
    public class WeekDatesShould
    {
        [Fact]
        public void Obtain_Monday_Of_Next_Weeks()
        {
            //Arrange
            var utc = Substitute.For<IUtc>();
            utc.Now().Returns(new DateTime(2020, 2, 8));
            var weekDates = new WeekDates(utc);

            //Act
            var result = weekDates.GetWeekDates(5);

            //Assert
            result.Should().HaveCount(5);
            result.Should().ContainInOrder(new[] { 
                new DateTime(2020, 2, 10), 
                new DateTime(2020, 2, 17), 
                new DateTime(2020, 2, 24), 
                new DateTime(2020, 3, 2), 
                new DateTime(2020, 3, 9),
            });
        }
    }
}
