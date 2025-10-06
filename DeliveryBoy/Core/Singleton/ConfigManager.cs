using System;

namespace DeliveryBoy.Core.Singleton
{
    public sealed class ConfigManager
    {
        private static readonly Lazy<ConfigManager> _inst = new(() => new ConfigManager());
        public static ConfigManager Instance => _inst.Value;
        private ConfigManager() { }

        public decimal EnvioGratisDesde { get; set; } = 50000m;
        public decimal IVA { get; set; } = 0.21m;
    }
}
