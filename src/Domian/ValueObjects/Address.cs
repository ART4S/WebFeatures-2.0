﻿using Domian.Common;
using Domian.Entities;
using System;
using System.Collections.Generic;

namespace Domian.ValueObjects
{
    public class Address : ValueObject
    {
        public string StreetName { get; set; }
        public string PostalCode { get; set; }

        public Guid CityId { get; set; }
        public City City { get; set; }

        protected override IEnumerable<object> GetComparisionValues()
        {
            yield return StreetName;
            yield return CityId;
            yield return PostalCode;
        }
    }
}