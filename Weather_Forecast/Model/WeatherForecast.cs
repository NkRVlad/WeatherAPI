using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Weather_Forecast
{
  
    public class List
    {
        public string Name { get; set; }
        [JsonIgnore]
        public int dt { get; set; }
        public string Date { get; set; }
        public Main Main { get; set; }
        public Wind Wind { get; set; }
        public Clouds Clouds { get; set; }
    }
  
    public class Main
    {
        public double temp { get; set; }
        public double temp_max { get; set; }
        public double temp_min { get; set; }
    }

    public class Wind
    {
        public double speed { get; set; }
    }

    public class Clouds
    {
        public int all { get; set; }
    }
}
