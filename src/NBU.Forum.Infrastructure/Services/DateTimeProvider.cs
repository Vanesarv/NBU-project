namespace NBU.Forum.Infrastructure.Services;

using Application.Services;
using System;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
