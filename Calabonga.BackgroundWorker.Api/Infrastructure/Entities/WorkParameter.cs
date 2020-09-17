namespace Calabonga.BackgroundWorker.Api.Infrastructure.Entities
{
    /// <summary>
    /// Work  parameters for Scheduler
    /// </summary>
    public class WorkParameter
    {
        public WorkParameter() { }

        public WorkParameter(string name, object value)
        {
            Name = name;
            Value = value;
            TypeName = value.GetType().FullName ?? value.GetType().Name;
        }

        public string? Name { get; set; }

        public string TypeName { get; set; } = null!;

        public object Value { get; set; } = null!;
    }
}
