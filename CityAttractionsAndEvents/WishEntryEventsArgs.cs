﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityAttractionsAndEvents
{
   public class WishEntryEventsArgs : EventArgs
    {
        public string ImagePath { get; set; }
        public string Name { get; set; }
    }
}
