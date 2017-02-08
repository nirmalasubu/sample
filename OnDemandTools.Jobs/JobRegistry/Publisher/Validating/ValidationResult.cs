namespace OnDemandTools.Jobs.JobRegistry.Publisher.Validating
{
    public class ValidationResult
    {
        public ValidationResult(bool valid, int statusEnum, string message, bool ignoreQueue=false)
        {
            Valid = valid;
            Message = message;
            StatusEnum = statusEnum;
            IgnoreQueue = ignoreQueue;
        }

        public ValidationResult(bool valid)
        {
            Valid = valid;
            Message = string.Empty;
        }

        public bool Valid { get; private set; }
        public string Message { get; private set; }
        public int StatusEnum { get; set; }
        public bool IgnoreQueue { get; private set; }
    }
}