namespace WebApplication.Infrastructure
{
    public class SearchTerm
    {
        public string Name { get; set; }
        public string operators{ get; set; }
        public string Value { get; set; }
        public bool ValidSyntax { get; set; }
    }
}