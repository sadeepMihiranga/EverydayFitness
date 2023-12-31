﻿namespace WorkoutService.DTOs
{
    public class WorkoutDTO : BaseDTO
    {
        public string Name { get; set; }
        public WorkoutTypeDTO Type { get; set; }
        public DateTime Date { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Weight { get; set; }
        public int Reps { get; set; }
        public int Sets { get; set; }
        public bool IsRecurring { get; set; }
        public string? RecurringType { get; set; }
        public string? RecurrsionDate { get; set; }
        public string Comment { get; set; }
        public UserDTO? User { get; set; }
    }
}
