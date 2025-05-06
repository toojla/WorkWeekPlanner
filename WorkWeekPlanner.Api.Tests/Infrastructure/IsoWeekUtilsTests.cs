using FluentAssertions;
using WorkWeekPlanner.Api.Infrastructure;

namespace WorkWeekPlanner.Api.Tests.Infrastructure
{
    public class IsoWeekUtilsTests
    {
        [Theory]
        [InlineData("2023-01-01", 2022, 52)] // ISO week starts on Monday, Jan 1, 2023 is part of the last week of 2022
        [InlineData("2023-01-02", 2023, 1)]  // First Monday of 2023
        [InlineData("2023-12-31", 2023, 52)] // Last day of 2023
        [InlineData("2025-04-30", 2025, 18)]
        public void GetIso8601WeekOfYear_ShouldReturnCorrectYearAndWeek(string date, int expectedYear, int expectedWeek)
        {
            // Arrange
            var dateTime = DateTime.Parse(date);

            // Act
            var (year, week) = IsoWeekUtils.GetIso8601WeekOfYear(dateTime);

            // Assert
            year.Should().Be(expectedYear);
            week.Should().Be(expectedWeek);
        }

        [Theory]
        [InlineData(2023, 1, "2023-01-02")]  // First Monday of the first week of 2023
        [InlineData(2023, 52, "2023-12-25")] // First Monday of the last week of 2023
        [InlineData(2022, 52, "2022-12-26")] // First Monday of the last week of 2022
        public void FirstDateOfWeekIso8601_ShouldReturnCorrectDate(int year, int week, string expectedDate)
        {
            // Arrange
            var expectedDateTime = DateTime.Parse(expectedDate);

            // Act
            var result = IsoWeekUtils.FirstDateOfWeekIso8601(year, week);

            // Assert
            result.Should().Be(expectedDateTime);
        }
    }
}