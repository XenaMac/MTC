using System;

namespace MTC.FSP.Web.ViewModels
{
    public class BeatViewModel
    {
        public Guid BeatId { get; set; }

        public string BeatNumber { get; set; }
    }

    public class YardViewModel
    {
        public Guid YardId { get; set; }

        public string YardNumber { get; set; }
    }

    public class BeatViewModel2
    {
        public Guid Id { get; set; }

        public string Text { get; set; }

        public int Value { get; set; }
    }
}