﻿using Domian.Common;
using System;

namespace Domian.Entities
{
    public class City : Entity
    {
        public string Name { get; set; }

        public Guid CountryId { get; set; }
        public Country Country { get; set; }
    }
}