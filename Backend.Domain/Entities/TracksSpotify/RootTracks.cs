using System.Collections.Generic;

namespace Backend.Domain.Entities.TracksSpotify
{
    public class RootTracks
    {
        public string href { get; set; }
        public List<Item> items { get; set; }
        public int limit { get; set; }
        public object next { get; set; }
        public int offset { get; set; }
        public object previous { get; set; }
        public int total { get; set; }
    }
}
