﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Readinizer.Backend.Domain.ModelsJson.HelperClasses
{
    public class Value
    {
        public Value()
        {
            Element = new Element();
            Name = "Undefined";
            Number = "Undefined";
        }

        [JsonProperty("Element")]
        public Element Element { get; set; } = new Element();

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Number")]
        public string Number { get; set; }
    }
}