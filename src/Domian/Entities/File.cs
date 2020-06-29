﻿using Domian.Common;

namespace Domian.Entities
{
    public class File : Entity
    {
        public string Name { get; set; }
        public string ContentType { get; set; }
        public string CheckSum { get; set; }
        public byte[] Content { get; set; }
    }
}