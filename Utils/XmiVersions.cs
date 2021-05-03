namespace master_project.Utils
{
    public class XmiVersions
    {
        private XmiVersions(string value) { Value = value; }

        public string Value { get; set; }
        public static XmiVersions One { get { return new XmiVersions("1.1"); } }
        public static XmiVersions Two { get { return new XmiVersions("2.1"); } }
    }
}