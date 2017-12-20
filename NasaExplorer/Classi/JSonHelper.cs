using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NasaExplorer.Classi
{
    public partial class JMars1
    {
        [JsonProperty("photos")]
        public List<Photo> Photos { get; set; }
    }

    public partial class Photo
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("sol")]
        public long Sol { get; set; }

        [JsonProperty("camera")]
        public PhotoCamera Camera { get; set; }

        [JsonProperty("img_src")]
        public string ImgSrc { get; set; }

        [JsonProperty("earth_date")]
        public DateTime EarthDate { get; set; }

        [JsonProperty("rover")]
        public Rover Rover { get; set; }
    }

    public partial class Rover
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("landing_date")]
        public DateTime LandingDate { get; set; }

        [JsonProperty("launch_date")]
        public DateTime LaunchDate { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("max_sol")]
        public long MaxSol { get; set; }

        [JsonProperty("max_date")]
        public DateTime MaxDate { get; set; }

        [JsonProperty("total_photos")]
        public long TotalPhotos { get; set; }

        [JsonProperty("cameras")]
        public List<RoverCamera> Cameras { get; set; }
    }

    public partial class RoverCamera
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("full_name")]
        public string FullName { get; set; }
    }

    public partial class PhotoCamera
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("rover_id")]
        public long RoverId { get; set; }

        [JsonProperty("full_name")]
        public string FullName { get; set; }
    }

    public partial class JMars1
    {
        public static JMars1 FromJson(string json) => JsonConvert.DeserializeObject<JMars1>(json, JMars1_Converter.Settings);
    }

    public static class JMars1_Serialize
    {
        public static string ToJson(this JMars1 self) => JsonConvert.SerializeObject(self, JMars1_Converter.Settings);
    }

    public class JMars1_Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
        };
    }
}
