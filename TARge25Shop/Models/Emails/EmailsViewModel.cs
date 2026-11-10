namespace TARge25Shop.Models.Emails
{
    public class EmailsViewModel
    {
        public string To { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public IFormFileCollection Attachment { get; set; }
    }
}
