﻿using Ecommerence.Domain.Commands.Common;
using System;

namespace Ecommerence.Domain.Commands.Product
{
    public class UpdateProductCommand : Command
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }
    }
}
