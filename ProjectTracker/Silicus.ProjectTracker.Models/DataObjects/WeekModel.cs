using System;
using System.ComponentModel.DataAnnotations;

namespace Silicus.ProjectTracker.Models.DataObjects
{
    public class WeekModel
    {
        [Key]
        public int WeekId { get; set; }

        public DateTime BeginningOfWeek { get; set; }

        public DateTime EndOfWeek { get { return this.BeginningOfWeek.AddDays(6); } }

        public int Number { get; set; }

        public string Text { get; set; }

        public override string ToString()
        {
            return DateTime.Now > BeginningOfWeek && DateTime.Now < EndOfWeek
                ? String.Format(
                    "Week {0} or current week: {1} - {2}",
                    this.Number,
                    this.BeginningOfWeek.ToShortDateString(),
                    this.EndOfWeek.ToShortDateString())
                : String.Format(
                    "Week {0}: {1} - {2}",
                    this.Number,
                    this.BeginningOfWeek.ToShortDateString(),
                    this.EndOfWeek.ToShortDateString());
        }
    }
}
