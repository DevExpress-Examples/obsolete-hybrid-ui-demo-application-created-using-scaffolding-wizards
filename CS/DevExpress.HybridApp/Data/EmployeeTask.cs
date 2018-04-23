using System;
using System.Collections.Generic;
using System.Linq;

namespace DevExpress.DevAV {
    public class EmployeeTask : DatabaseObject {
        public string Subject { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public EmployeeTaskStatus Status { get; set; }
        public EmployeeTaskPriority Priority { get; set; }
        public int Completion { get; set; }
        public bool Reminder { get; set; }
        public DateTime? ReminderDateTime { get; set; }
        public virtual Employee AssignedEmployee { get; set; }
        public long? AssignedEmployeeId { get; set; }

        public override string ToString() {
            return string.Format("{0} - {1}, due {2}, {3}", Subject, Description, DueDate, Status);
        }

        public bool Overdue {
            get {
                if(Status == EmployeeTaskStatus.Completed || !DueDate.HasValue)
                    return false;
                DateTime dDate = DueDate.Value.Date.AddDays(1);
                if(DateTime.Now >= dDate)
                    return true;
                return false;
            }
        }
    }

    public enum EmployeeTaskStatus {
        NotStarted,
        Completed,
        InProgress,
        NeedAssistance,
        Deferred
    }

    public enum EmployeeTaskPriority {
        Low,
        Normal,
        High,
        Urgent
    }
}
