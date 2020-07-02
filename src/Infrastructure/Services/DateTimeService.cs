﻿using Application.Interfaces.Services;
using System;

namespace Infrastructure.Services
{
    class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.UtcNow;
    }
}