namespace Napos.Models
{
    public class SettingsForm
    {
        /// <summary>
        /// null - System
        /// false - Light
        /// true - Dark
        /// </summary>
        public bool? Theme { get; set; }
    }

    public class SecuritySettingsForm
    {
        public string PrivateKey { get; set; }
    }
}
